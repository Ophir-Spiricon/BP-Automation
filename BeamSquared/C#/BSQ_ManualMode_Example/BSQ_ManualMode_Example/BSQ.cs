/* A C# wrapper class for BeamSquared v2.53 Manual Mode Automation

This module is intended to be as an educational example of a wrapper class which utilizes the BeamSquared
Automation Interface.  This class should be run directly but is instead accessed by a secondary program module for
the purposes of utilizing the BeamSquared Automation Interface.  The BeamSquared Automation Interface is an API which
interfaces with the BeamSquared beam propagation analyzer application.

This module does not provide direct camera API access and such access is not provided or supported with Ophir brand
beam profiling cameras. Note that the bulk of laser beam profiling is performed in the beam profiler application, 
not the camera, and direct camera access is not useful.  For custom machine vision applications alternative products
are available commercially.

The BSQ wrapper class and all associated subclasses, enumerations, and example code are provided as-is
with no express guarantee or warranty of functionality and may be provided and updated independently of BeamGage
Professional releases. This module and its component parts are not provided as production software and are not intended
to be reused as such.

This module may be customized as needed but all client logic should be implemented as abstracted in a secondary
program module.  Feedback or suggestions are welcomed for future versions.

The latest version of Ophir beam profiling software is available for download at:
https://www.ophiropt.com/laser--measurement/software-download

Professional tier licenses for Ophir brand beam profiling cameras may be purchased by contacting:
    Ophir USA Product Support\n
    1 - 800 - 383 - 0814\n
    service.ophir.usa@mksinst.com

MKS | Ophir  The True Measure of Laser Performance™

Example:

    using (_bsq = new BSQ())
    {
    ...
    }

Todo:
    * Complete wrapper implementation of other individual BeamSquared interfaces
    * Resolve event handler delegate functionality
    * Document specific pain points of features and best practices and abstract complexity as possible
    * Flush out example docstrings
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using M2.Automation;
using M2.Automation.Interfaces;
using Microsoft.Win32.SafeHandles;

namespace BSQManualModeExample
{
    public class BSQ : IDisposable
    {
        private AutomatedBeamSquared _automatedBeamSquared;
        private bool _closeBSQOnExit;
        private bool _railControl;
        private bool _goodFrameNotifier;
        private AUltracalVerifierStatus _lastVerifierStatus;

        AutomationRunManagerEvents _runManagerEvents;
        AutomationCaptureEvents _captureEvents;
        AutomationUltracalVerifierEvents _ultracalVerifierEvents;
        AutomationAnalyzerEvents _analyzerEvents;
        AutomationRailControlEvents _railControlEvents;

        private bool _runManagerStatus;
        private bool _frameAvailable;
        private bool _curveFitAvailable;
        private bool _waitingForBlockAndUnblock;

        private double _maxAD;

        private bool _disposed = false;
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose() => Dispose(true);

        #region Public Members
        public BSQ()
        {
            _closeBSQOnExit = true;
        }

        public void Initialize(bool showGui)
        {
            _automatedBeamSquared = new AutomatedBeamSquared(showGui);

            // features
            _railControl = true;  // not needed for manual mode
            _goodFrameNotifier = true; // mandatory for manual mode

            //states
            _runManagerStatus = false;
            _frameAvailable = false;
            _curveFitAvailable = false;
            _lastVerifierStatus = AUltracalVerifierStatus.IDLE;
        }

        public void Shutdown()
        {
            _automatedBeamSquared.Instance.Shutdown();
        }

        public bool LiveMode
        {
            get
            {
                return _automatedBeamSquared.Capture.LiveMode;
            }

            set
            {
                if (_automatedBeamSquared.RunManager.RunStatus == ARunManagerStatus.READY && !_automatedBeamSquared.Capture.LiveMode)
                    Console.WriteLine("Starting live mode");
                else
                    Console.WriteLine("Stopping live mode");

                _automatedBeamSquared.Capture.LiveMode = value;
            }
        }

        public void UseCurrentFrame()
        {
            _automatedBeamSquared.RunManager.UseCurrentFrame();
        }

        public void GetNewFrame()
        {
            _automatedBeamSquared.Capture.PlusOne();
        }

        public void AbortRun()
        {
            _automatedBeamSquared.RunManager.AbortRun();
            Console.WriteLine("Run Aborted.");
        }

        public void StartRun(ARunManagerMethods runMethod)
        {
            IARunManager runManager = _automatedBeamSquared.RunManager;
            _runManagerEvents = new AutomationRunManagerEvents(runManager);
            _runManagerEvents.OnCurveFitAvailable += OnCurveFitAvailable;  //not exceptionally useful
            _runManagerEvents.OnStatusChange += OnRunManagerStatusChange;
            runManager.RunMethod = runMethod;

            _ultracalVerifierEvents = new AutomationUltracalVerifierEvents(_automatedBeamSquared.UltracalVerifier);
            _ultracalVerifierEvents.OnStatusChange += OnUltracalVerifierStatusChange;

            _analyzerEvents = new AutomationAnalyzerEvents(_automatedBeamSquared.Analyzer);
            _analyzerEvents.OnStatusChange += OnFrameAnalyzerStatusChange;

            if (_goodFrameNotifier)
            {
                _captureEvents = new AutomationCaptureEvents(_automatedBeamSquared.Capture);
                _captureEvents.OnFrameAvailable += OnGoodFrameAvailable;
            }

            if (_railControl)
            {
                _railControlEvents = new AutomationRailControlEvents(_automatedBeamSquared.RailControl);
                _railControlEvents.OnPositionSet += OnRailControlPositionSet;
            }

            runManager.StartRun();
            Console.WriteLine("Starting {0} run.", ARunManagerMethods.STEP_TABLE.ToString());

            _maxAD = Math.Pow(2, _automatedBeamSquared.QuantitativeResults.BitsPerPixel);
        }

        public void FinishRun()
        {
            _automatedBeamSquared.RunManager.FinishRun();
        }

        public void DoQuantitativeResults()
        {
            Console.WriteLine("***Quantatative Results***");
            IAQuantitativeResults frameResults = _automatedBeamSquared.QuantitativeResults;
            Console.WriteLine("BeamWidth X: {0:F}", frameResults.BeamWidthX);
            Console.WriteLine("BeamWidth Y: {0:F}", frameResults.BeamWidthY);
            Console.WriteLine("Centroid X: {0:F}", frameResults.CentroidX);
            Console.WriteLine("Centroid Y: {0:F}", frameResults.CentroidY);
            Console.WriteLine("Peak: {0:F}", frameResults.Peak);
            Console.WriteLine();
        }

        public void DoLaserResults()
        {
            Console.WriteLine("***Laser Results***");
            IALaserResults laserResults = _automatedBeamSquared.LaserResults;
            Console.WriteLine("M2 X: {0:F}", laserResults.M2X);
            Console.WriteLine("M2 Y: {0:F}", laserResults.M2Y);
            Console.WriteLine("Divergence X: {0:F}", laserResults.DivergenceX);
            Console.WriteLine("Divergence Y: {0:F}", laserResults.DivergenceY);
            Console.WriteLine("Rayleigh X: {0:F}", laserResults.RayleighX);
            Console.WriteLine("Rayleigh Y: {0:F}", laserResults.RayleighY);
            Console.WriteLine("WaistLocation X: {0:F}", laserResults.WaistLocationX);
            Console.WriteLine("WaistLocation Y: {0:F}", laserResults.WaistLocationY);
            Console.WriteLine("WaistWidth X: {0:F}", laserResults.WaistWidthX);
            Console.WriteLine("WaistWidth Y: {0:F}", laserResults.WaistWidthY);
            Console.WriteLine("Astigmatism: {0:F}", laserResults.Astigmatism);
            Console.WriteLine("Asymmetry: {0:F}", laserResults.Asymmetry);
            Console.WriteLine();
        }

        public void DoLensResults()
        {
            Console.WriteLine("***After Lens Results***");
            IALensResults lensResults = _automatedBeamSquared.LensResults;
            Console.WriteLine("WaistWidth X: {0:F}", lensResults.WaistWidthX);
            Console.WriteLine("WaistWidth Y: {0:F}", lensResults.WaistWidthY);
            Console.WriteLine();
        }

        //Not used for Manual Mode, could be replaced with own rail integration
        //private void DoRailTransit()
        //{
        //    if (_automatedBeamSquared.RunManager.RunStatus == ARunManagerStatus.READY && !_bsq.LiveMode)
        //    {
        //        Console.WriteLine("Starting live mode");
        //        _automatedBeamSquared.Capture.LiveMode = true;
        //    }

        //    Console.WriteLine("Hit the space bar to toggle between the min and max position on the rail,\n any other key to exit live mode and start the run");
        //    IARailControl railControl = _automatedBeamSquared.RailControl;
        //    while (true)
        //    {
        //        if(Console.ReadKey(false).KeyChar != ' ')
        //            break;

        //        railControl.Position = railControl.CurrentPosition == railControl.MinPosition
        //            ? railControl.MaxPosition
        //            : railControl.MinPosition;
        //    }
        //}

        public double Exposure
        {
            get { return _automatedBeamSquared.DataSourceExposure.Get(); }
            set
            { 
                _automatedBeamSquared.DataSourceExposure.Set(value);
                Console.WriteLine("Exposure value set to: {0}", _automatedBeamSquared.DataSourceExposure.Get());
            }
        }

        public bool RunManagerStatus
        { get { return _runManagerStatus; } }

        public AAnalyzerStatus AnalyzerStatus
        { get { return _automatedBeamSquared.Analyzer.Status; } }

        public AUltracalVerifierStatus UltracalVerifierStatus
        { get { return _lastVerifierStatus; } }

        public bool FrameAvailable
        { get { return _frameAvailable; } }

        public bool CurveFitAvailable
        { get { return _curveFitAvailable; } }

        public bool IsBlockingBeam
        { 
            get { return _waitingForBlockAndUnblock; }
            set { _waitingForBlockAndUnblock = value; }
        }

        public double CurrentPosition
        {
            get { return (double)_automatedBeamSquared.RailControl.CurrentPosition; }
            set { _automatedBeamSquared.RailControl.PositionSet = (double)value; }
        }
        #endregion

        #region Protected Members
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_runManagerEvents != null)
                {
                    _runManagerEvents.OnStatusChange -= OnRunManagerStatusChange;
                    _runManagerEvents.OnCurveFitAvailable -= OnCurveFitAvailable;
                }

                if (_captureEvents != null)
                    _captureEvents.OnFrameAvailable -= OnGoodFrameAvailable;

                if (_ultracalVerifierEvents != null)
                    _ultracalVerifierEvents.OnStatusChange -= OnUltracalVerifierStatusChange;

                if (_analyzerEvents != null)
                    _analyzerEvents.OnStatusChange -= OnFrameAnalyzerStatusChange;

                if (_railControl)
                    _railControlEvents.OnPositionSet -= OnRailControlPositionSet;

                if (_automatedBeamSquared.RunManager.RunStatus == ARunManagerStatus.RUNNING)
                    _automatedBeamSquared.RunManager.AbortRun();

                if (_closeBSQOnExit)
                    Shutdown();

                // Dispose managed state (managed objects).
                _safeHandle?.Dispose();
            }
            _disposed = true;
        }
        #endregion

        #region Interface properties wrapper
        public IAM2Setup aM2Setup
        {
            get { return _automatedBeamSquared.Setup; }
        }

        public IATableZGenerator aTableZGenerator
        {
            get { return _automatedBeamSquared.TableZGenerator; }
        }

        public IASuggestedZGenerator aSuggestedZGenerator
        {
            get { return _automatedBeamSquared.SuggestedZGenerator; }
        }

        public IAFourCutsZGenerator aFourCutsZGenerator
        {
            get { return _automatedBeamSquared.FourCutsZGenerator; }
        }
        public IARunManager aRunManager
        {
            get { return _automatedBeamSquared.RunManager; }
        }

        public IAQuantitativeResults aQuantitativeResults
        {
            get { return _automatedBeamSquared.QuantitativeResults; }
        }

        public IALaserResults aLaserResults
        {
            get { return _automatedBeamSquared.LaserResults; }
        }

        public IALensResults aLensResults
        {
            get { return _automatedBeamSquared.LensResults; }
        }

        #endregion

        #region Event Handler delegates
        public void OnRunManagerStatusChange(object sender, RunStatusChangeEventArgs e)
        {
            if (e.Status == ARunManagerStatus.RUN_FINISHED)
            {
                // set own flag to monitor
                _runManagerStatus = false;
                Console.WriteLine("");
                Console.WriteLine("***Run Complete***");
                DoLaserResults();
                DoQuantitativeResults();
                Console.WriteLine("Hit any key to exit this example");
                return;
            }

            if(e.Status == ARunManagerStatus.RUNNING)
            {
                _runManagerStatus = true;
                Console.WriteLine("***Run Started***");
            }

            //Console.WriteLine("The run manager status has changed to " + e.Status);
        }

        public void OnGoodFrameAvailable(object sender, FrameAvailableEventArgs e)
        {
            if (e.Available)
            {
                _frameAvailable = true;
                DoQuantitativeResults();
                Console.WriteLine("A good frame is available");

                return;
            }
            _frameAvailable = false;
            Console.WriteLine("A good frame is no longer available");
        }

        public void OnCurveFitAvailable(object sender, CurveFitAvailableEventArgs e)
        {
            if (e.Available)
            {
                _curveFitAvailable = true;
                Console.WriteLine("A curve fit to the current run is now available");
                DoLaserResults();
                DoLensResults();
                return;
            }
            _curveFitAvailable = false;
            Console.WriteLine("Curve fit is no longer available");
        }

        public void OnFrameAnalyzerStatusChange(object sender, FrameAnalyzerEventArgs e)
        {
            if (e.Status == AAnalyzerStatus.BIG)
            {
                Console.WriteLine("Beam is too big");
                Console.WriteLine("Width X: {0:F} | Width Y: {1:F}",
                                  _automatedBeamSquared.QuantitativeResults.BeamWidthX,
                                  _automatedBeamSquared.QuantitativeResults.BeamWidthY);
            }

            if (e.Status == AAnalyzerStatus.SMALL)
            {
                Console.WriteLine("Beam is too small");
                Console.WriteLine("Width X: {0:F} | Width Y: {1:F}",
                                  _automatedBeamSquared.QuantitativeResults.BeamWidthX,
                                  _automatedBeamSquared.QuantitativeResults.BeamWidthY);
            }

            if (e.Status == AAnalyzerStatus.BRIGHT)
            {
                Console.WriteLine("Beam is too bright. | Peak: {0:P} |  Peak of <90% Max Recommended",
                                  (_automatedBeamSquared.QuantitativeResults.Peak / _maxAD));
            }

            if (e.Status == AAnalyzerStatus.DIM)
            {
                Console.WriteLine("Beam is too dim. | Peak: {0:P} | Peak of >50% Recommended",
                    (_automatedBeamSquared.QuantitativeResults.Peak / _maxAD));
            }

            if (e.Status == (AAnalyzerStatus.SMALL | AAnalyzerStatus.BRIGHT))
                Console.WriteLine("Beam is too small and bright | Peak: {0:P} of Max | Width X: {1:F} | Width Y: {2:F}",
                                  (_automatedBeamSquared.QuantitativeResults.Peak / _maxAD),
                                   _automatedBeamSquared.QuantitativeResults.BeamWidthX,
                                   _automatedBeamSquared.QuantitativeResults.BeamWidthY);

            if (e.Status == (AAnalyzerStatus.BIG | AAnalyzerStatus.DIM))
                Console.WriteLine("Beam is too big and dim | Peak: {0:P} of Max | Width X: {1:F} | Width Y: {2:F}",
                                  (_automatedBeamSquared.QuantitativeResults.Peak / _maxAD),
                                   _automatedBeamSquared.QuantitativeResults.BeamWidthX,
                                   _automatedBeamSquared.QuantitativeResults.BeamWidthY);

            if (e.Status == AAnalyzerStatus.ALIGNMENT)
                Console.WriteLine("Beam is not aligned parallel");

            if (e.Status == AAnalyzerStatus.NOT_ULTRACALED)
                Console.WriteLine("Baseline is not calibrated");

            if (e.Status == AAnalyzerStatus.GOOD)
            {
                Console.WriteLine("Beam is OK | Peak: {0:P} of Max | Width X: {1:F} | Width Y: {2:F}",
                                  (_automatedBeamSquared.QuantitativeResults.Peak / _maxAD),
                                   _automatedBeamSquared.QuantitativeResults.BeamWidthX,
                                   _automatedBeamSquared.QuantitativeResults.BeamWidthY);
            }

            if (e.Status == AAnalyzerStatus.UNKNOWN)
                Console.WriteLine("Frame status is unknown");

            //Console.WriteLine("The FrameAnalyzer status has changed to " + e.Status);
            return;
        }

        public void OnUltracalVerifierStatusChange(object sender, UltracalVerifierEventArgs e)
        {
            if (e.Status == AUltracalVerifierStatus.FINISHED)
                _waitingForBlockAndUnblock = true;

            if(e.Status == AUltracalVerifierStatus.IDLE)
                _waitingForBlockAndUnblock = false;
            
            if (e.Status == AUltracalVerifierStatus.VALIDATING &&
               _lastVerifierStatus == AUltracalVerifierStatus.FINISHED)
                Console.WriteLine("Validating Baseline");

            if (e.Status == AUltracalVerifierStatus.ULTRACALING &&
               _lastVerifierStatus == AUltracalVerifierStatus.VALIDATING)
                Console.WriteLine("Ultracal Started");

            if (e.Status == AUltracalVerifierStatus.FINISHED &&
               _lastVerifierStatus == AUltracalVerifierStatus.ULTRACALING)
                Console.WriteLine("Ultracal Completed");

            //if changed, save the new state for reference
            if (e.Status != _lastVerifierStatus)
                _lastVerifierStatus = e.Status;
        }

        public void OnRailControlPositionSet(object sender, RailControlPositionEventArgs e)
        {
            _automatedBeamSquared.RailControl.CurrentPosition
                sender.ToString();
                Console.WriteLine("Position was set to {0:F}", e.Position);
        }
        #endregion
    }
}
