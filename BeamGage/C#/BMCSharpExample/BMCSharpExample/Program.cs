using System;
using Spiricon.Automation;

namespace BMCSharpExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner runMe = new Runner();
            runMe.Run();
        }
    }
    public class Runner
    {
        // Declare the BeamMic Automation client
        private AutomatedBeamMic _bm;
        public void Run()
        {
            Console.WriteLine("Starting BeamMic\n");
            // Start BeamMic Automation client
            _bm = new AutomatedBeamMic("ClientOne", true);
            Console.WriteLine("Hit a key to turn off Auto X");
            Console.ReadKey();
            // Turn off Auto X
            _bm.Calibration.AutoX();
            Console.WriteLine("Auto X is off, hit a key to turn it back on");
            Console.ReadKey();
            _bm.Calibration.AutoX();
            // Create and register for the new frame event
            new AutomationFrameEvents(_bm.ResultsPriorityFrame).OnNewFrame += NewFrameFunction;
            Console.WriteLine("Press enter to exit.\n");
            Console.Read();
            // Shutdown BeamMic
            _bm.Instance.Shutdown();
        }
        void NewFrameFunction()
        {
            // Print Total counts of the frame
            Console.Write("Frame total: " + _bm.PowerEnergyResults.Total + "\r");
        }
    }
}
