using System;
using BeamWatch.Automation;
using BeamWatch.Automation.Interfaces;

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
        // Declare the BeamWatch Automation client
        private AutomatedBeamWatch _bw;
        public void Run()
        {
            Console.WriteLine("Press enter to exit.\n");
            // Start BeamWatch Automation client
            _bw = new AutomatedBeamWatch(true);
            // verify that we are connected to a camera
            if (_bw.DataSource.Status == ADataSourceStatus.UNAVAILABLE)
                Console.WriteLine("No camera available");
            else
            {
                // Create and register for the new frame event
                new AutomationFrameEvents(_bw.Frame).OnNewFrame += NewFrameFunction;
                Console.WriteLine("Starting data source");
                _bw.DataSource.Start();
            }
            Console.Read();
            // Shutdown
            _bw.Instance.Shutdown();
        }
        void NewFrameFunction()
        {
            // Print the ID and waist width of the X Axis
            // note: The value for the waist will be 0.0 if a beam is not detected
            Console.Write("Frame # " + (int)_bw.FrameInfoResults.ID + ", X Axis Waist Width: " + _bw.SpatialResults.Waist(Axes.X_Axis) + "                        \r");
        }
    }
}