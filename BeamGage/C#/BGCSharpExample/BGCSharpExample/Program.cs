using System;
using Spiricon.Automation;
using System.Linq;

namespace BGCSharpExample
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
        // Declare the BeamGage Automation client
        private AutomatedBeamGage _bg;

        public void Run()
        {
            Console.WriteLine("Press enter to exit.\n");
            // Start BeamGage Automation client
            _bg = new AutomatedBeamGage("ClientOne", true);

            Console.Write("Ultracalling.... ");
            // Invoke an ultracal cycle
            _bg.Calibration.Ultracal();
            Console.WriteLine("finished\n");

            // Create and register for the new frame event
            new AutomationFrameEvents(_bg.ResultsPriorityFrame).OnNewFrame += NewFrameFunction;
            Console.Read();

            // Shutdown BeamGage
            _bg.Instance.Shutdown();
        }


        /// <summary>
        /// This method is declared above as the delegate method for the OnNewFrame event
        /// Any actions here occur within the BeamGage data aqcuisition cycle
        /// To avoid blocking BeamGage, minimize actions here, or get data and push work to other threads.
        /// </summary>
        private void NewFrameFunction()
        {
            //Obtain the frame data and use it
            double[] frameData = _bg.ResultsPriorityFrame.DoubleData;

            // Print the ID and Total counts of the frame
            Console.WriteLine("Frame # " + (int)_bg.FrameInfoResults.ID + ", Total: " + _bg.PowerEnergyResults.Total + "\r");
            Console.WriteLine("ArrayCount: {0} Max Value: {1} Min Value: {2} {3}",
                              frameData.Length, frameData.Max(), frameData.Min(), @"                         ");  // final parameter is an ugly but simple way to overwrite any trailing characters from the previous iteration

            Console.SetCursorPosition(0, Console.CursorTop - 2);
        }
    }
}
