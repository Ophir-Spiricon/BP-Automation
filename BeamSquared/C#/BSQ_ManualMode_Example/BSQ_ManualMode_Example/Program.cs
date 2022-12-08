/* A C# example client for BeamSquared v2.53 Manual Mode Automation

This module is intended as an educational example of a client program which utilizes the BeamSquared
Automation Interface.  The BeamSquared Automation Interface is an API which interfaces with the BeamSquared beam
propagation analyzer product.  

This module does not provide direct camera API access and such access is not provided
or supported with Ophir brand beam profiling cameras.  Note that the bulk of laser beam profiling is performed in the
beam profiler application, not the camera, and direct camera access is not useful.  For custom machine vision
applications alternative products are available commercially.

This program and all associated subclasses, enumerations, and example code are provided as-is with no express
guarantee or warranty of functionality and may be provided and updated independently of BeamSquared release software.
This module and its component parts are not provided as production software and are not intended to be reused as such.

This module may be adapted as needed without attribution.  Feedback or suggestions are welcomed for future versions.

The latest version of Ophir beam profiling software is available for download at:
https://www.ophiropt.com/laser--measurement/software-download

Professional tier licenses for Ophir brand beam profiling cameras may be purchased by contacting:
    Ophir USA Product Support\n
    1 - 800 - 383 - 0814\n
    service.ophir.usa@mksinst.com

MKS | Ophir  The True Measure of Laser Performance™

Todo:
    * Additional synchronization and formatting of the Console output could be possible
    * Flush out inline documentation
    * Consider additional scenarios which utilize using more features and/or best practices
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using M2.Automation;
using M2.Automation.Interfaces;

namespace BSQManualModeExample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Runner runMe = new Runner();
            runMe.Run();
        }

        public class Runner
        {
            private BSQ _bsq;
            private List<double> _stepTable;

            public void Run()
            {
                using (_bsq = new BSQ())
                {
                    DualOutput.Init();
                    _bsq.Initialize(true);

                    SetupRun();

                    _bsq.StartRun(ARunManagerMethods.STEP_TABLE);

                    ExecuteStepTable();

                    Console.WriteLine("You may hit a key to shutdown BeamSquared at any time");

                    Console.ReadKey();
                }
            }

            private void SetupRun()
            {
                _bsq.LiveMode = true;
                SetupBSQ(_bsq.aM2Setup);
                _bsq.LiveMode = false;
            }

            private void SetupBSQ(IAM2Setup m2Setup)
            {
                //Check that Expoure property is working
                Console.WriteLine("Current Exposure: {0}", _bsq.Exposure);
                _bsq.Exposure = 200.0;
                Thread.Sleep(2000);
                _bsq.Exposure = 15.0;
                Thread.Sleep(2000);

                m2Setup.Model = "Example laser Model";
                m2Setup.SerialNumber = "Example Serial number";
                m2Setup.Comment = "This is a example project for BeamSquared";
                m2Setup.WaveLength = 632.0;
                m2Setup.LaserLocation = 147.0;
                m2Setup.FocalLength = 50.5;

                SetupManualTable();
            }

            private void SetupSuggestedPoints()
            {
                IASuggestedZGenerator generator = _bsq.aSuggestedZGenerator;
                generator.ZStart = 370;
                generator.ZEnd = 670;
                generator.Step = 20;
            }

            private void SetupManualTable()
            {
                IATableZGenerator tableGenerator = _bsq.aTableZGenerator;

                ClearStepTable();
                
                _stepTable = new List<double>(15);

                _stepTable.Add(40.0);
                _stepTable.Add(42.0);
                _stepTable.Add(44.0);
                _stepTable.Add(46.0);
                _stepTable.Add(48.0);
                _stepTable.Add(49.0);
                _stepTable.Add(50.0);
                _stepTable.Add(51.0);
                _stepTable.Add(52.0);
                _stepTable.Add(53.0);
                _stepTable.Add(54.0);
                _stepTable.Add(56.0);
                _stepTable.Add(58.0);
                _stepTable.Add(60.0);

                // An M² run requires the focal length position to calculate the divergence
                _stepTable.Add(_bsq.aM2Setup.FocalLength);

                foreach(double stepValue in _stepTable)
                    tableGenerator.Add(stepValue);
            }

            public void ClearStepTable()
            {
                IATableZGenerator tableGenerator = _bsq.aTableZGenerator;
                List<double> stepTable = tableGenerator.ZLocations;

                foreach (double step in stepTable)
                    tableGenerator.Remove(step);
            }

            public void ExecuteStepTable()
            {
                // wait to ensure that the run starts
                while (!_bsq.RunManagerStatus &&
                       _bsq.UltracalVerifierStatus == AUltracalVerifierStatus.IDLE)
                    Thread.Sleep(50);

                // wait until the user has finished z-Location move and confirms
                foreach (double stepValue in _stepTable)
                {
                    Console.WriteLine("***Suggested z-Location: {0:F}***", _bsq.CurrentPosition);
                    //Console.WriteLine("Move rail to desired z-Location, enter value in millimeters, and press Enter.");
                    while (_bsq.RunManagerStatus)
                    {
                        string line = Console.ReadLine();

                        double inputValue;
                        if (double.TryParse(line, out inputValue))
                        {
                            if (inputValue == stepValue)
                                _bsq.CurrentPosition = stepValue;
                            else
                            {
                                _bsq.CurrentPosition = inputValue;
                                Console.WriteLine("New step value set: {0}", inputValue);
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Value is not a valid integer.");
                        }
                    }

                    // wait until the Ultracal process starts, check flag from wrapper
                    while (_bsq.RunManagerStatus && _bsq.IsBlockingBeam == false)
                        Thread.Sleep(50);

                    // Wait until the verifier status returns to idle
                    Console.WriteLine("Block beam, wait until calibration completes, then unblock beam.");
                    while (_bsq.RunManagerStatus && _bsq.IsBlockingBeam == true)
                        Thread.Sleep(50);

                    Console.WriteLine("To get new frame, press N and Enter.  To accept frame, press Y and Enter.");
                    while (_bsq.RunManagerStatus)
                    {
                        string line = Console.ReadLine().ToLower();

                        if (line == "y")
                        {
                            _bsq.UseCurrentFrame();
                            break;
                        }
                        if (line == "n")
                        {
                            _bsq.GetNewFrame();
                        }
                        else
                            Console.WriteLine("Invalid key.");
                    }
                }

                //IATableZGenerator tableGenerator = _bsq.aTableZGenerator;
                //foreach (double stepValue in tableGenerator.ZLocations)
                //{
                //    //set the position immediately based on the step table
                //    _bsq.CurrentPosition = stepValue;

                //    //while(!_bsq.FrameAvailable)
                //        Thread.Sleep(50);

                //    _bsq.UseCurrentFrame();
                //}
                _bsq.FinishRun();
            }

            public static class DualOutput
            {
                private static TextWriter _current;
                private static string _fileName;


                private class OutputWriter : TextWriter
                {
                    public override Encoding Encoding
                    {
                        get
                        {
                            return _current.Encoding;
                        }
                    }

                    public override void WriteLine(string value)
                    {
                        string dateCode = DateTime.Now.ToString("s");
                        
                        _current.WriteLine(value);

                        File.AppendAllLines(_fileName, new string[] { string.Concat(dateCode, " ", value) });
                    }
                }

                public static void Init()
                {
                    String format = "yyyyddMM_HHmmss";
                    string dateCode = DateTime.Now.ToString(format);

                    if (!Directory.Exists(@".\logs\"))
                        Directory.CreateDirectory(@".\logs\");

                    _fileName = Path.Combine(@".\logs\", string.Concat("BSQMM_",  dateCode, ".txt"));

                    _current = Console.Out;
                    Console.SetOut(new OutputWriter());
                }
            }
        }
    }
}