using System;
using M2.Automation;
using M2.Automation.Interfaces;

namespace BSQCSharpExample
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
            private AutomatedBeamSquared _automatedBeamSquared;

            public void Run()
            {

                _automatedBeamSquared = new AutomatedBeamSquared(true);
                DoLiveMode();
                SetupRun(_automatedBeamSquared.Setup);
                DoRun();
                Console.WriteLine("You may hit a key to shutdown BeamSquared at any time");
                Console.ReadKey();
                _automatedBeamSquared.Instance.Shutdown();
            }

            private void DoLiveMode()
            {
                if (_automatedBeamSquared.RunManager.RunStatus == ARunManagerStatus.READY && !_automatedBeamSquared.Capture.LiveMode)
                {
                    Console.WriteLine("Starting live mode");
                    _automatedBeamSquared.Capture.LiveMode = true;
                }

                Console.WriteLine("Hit the space bar to toggle between the min and max position on the rail,\n any other key to exit live mode and start the run");
                IARailControl railControl = _automatedBeamSquared.RailControl;
                while (true)
                {
                    if(Console.ReadKey(false).KeyChar != ' ')
                        break;

                    railControl.Position = railControl.CurrentPosition == railControl.MinPosition
                        ? railControl.MaxPosition
                        : railControl.MinPosition;
                }
            }

            private void DoRun()
            {
                IARunManager runManager = _automatedBeamSquared.RunManager;
                AutomationRunManagerEvents runManagerEvents = new AutomationRunManagerEvents(runManager);
                runManagerEvents.OnCurveFitAvailable += OnCurveFitAvailable;
                runManagerEvents.OnStatusChange += OnStatusChange;
                runManager.RunMethod = ARunManagerMethods.SUGGESTED;

                new AutomationRailControlEvents(_automatedBeamSquared.RailControl).OnPositionSet += (sender, args) =>
                {
                    Console.WriteLine("Position was set to " + args.Position);
                };

                runManager.StartRun();
            }

            private void OnCurveFitAvailable(object sender, CurveFitAvailableEventArgs e)
            {
                if (e.Available)
                {
                    Console.WriteLine("A curve fit to the current run is now available");
                    DoResults();
                    return;
                }

                Console.WriteLine("Curve fit is no longer available");
            }

            private void DoResults()
            {
                IALaserResults results = _automatedBeamSquared.LaserResults;
                Console.WriteLine("Current M2 value for the x axis is " + results.M2X);
                Console.WriteLine("Current M2 value for the y axis is " + results.M2Y);
            }

            private void OnStatusChange(object sender, RunStatusChangeEventArgs e)
            {
                if (e.Status == ARunManagerStatus.RUN_FINISHED)
                {
                    Console.WriteLine("Congratulations you have finished a run!");
                    DoResults();
                    Console.WriteLine("Hit any key to exit this example");
                    return;
                }

                Console.WriteLine("The run manager status has changed to " + e.Status);
            }

            private void SetupRun(IAM2Setup m2Setup)
            {
                m2Setup.Model = "Example laser Model";
                m2Setup.SerialNumber = "Example Serial number";
                m2Setup.Comment = "This is a example project for BeamSquared";
                m2Setup.WaveLength = 632;
                m2Setup.LaserLocation = 120;
                Console.WriteLine("The calculated focal length is " + m2Setup.FocalLength);
                SetupSuggestedPoints();
            }

            private void SetupSuggestedPoints()
            {
                IASuggestedZGenerator generator = _automatedBeamSquared.SuggestedZGenerator;
                generator.ZStart = 370;
                generator.ZEnd = 670;
                generator.Step = 20;
            }
        }
    }
}
