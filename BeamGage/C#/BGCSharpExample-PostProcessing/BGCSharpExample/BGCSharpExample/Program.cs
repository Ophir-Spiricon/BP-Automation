using System;
using Spiricon.Automation;
using System.Linq;
using System.Threading;

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
        uint _frames;

        public void Run()
        {
            Console.WriteLine("Press enter to exit.\n");
            // Start BeamGage Automation client
            _bg = new AutomatedBeamGage("ClientOne", true);

            _bg.DataSource.Stop();

            _bg.SaveLoadSetup.LoadSetup(@".\beammaker.bgSetup");

            Console.Write("Ultracalling.... ");
            // Invoke an ultracal cycle
            _bg.Calibration.Ultracal();
            Console.WriteLine("finished\n");

            // Some pattern of data collection that fills the frame buffer
            _bg.DataSource.Start();
            Thread.Sleep(5000);
            _bg.DataSource.Stop();

            // Post-processing
            // 1. Register for the OnNeFrame event
            // 2. Iterate through the frame buffer
            // 3. In the OnNewFrame delegate function, acquire data and results
            // 4. Process results and frame data in client threads

            // set frame buffer slot to last position
            _bg.FrameBuffer.BufferSlot = 0;

            // Create and register for the new frame event
            new AutomationFrameEvents(_bg.ResultsPriorityFrame).OnNewFrame += NewFrameFunction;

            // iterate through frame buffer
            _frames = 16;
            for (uint i = 1; i <= _frames; i++)
            {
                _bg.FrameBuffer.BufferSlot = i + 1;
                Thread.Sleep(1000);  // some delay to allow processing time

                // We are not accessing frame data and results here, that is the delegate's job
            }

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
            Console.WriteLine("ArrayCount: {0} Max Value: {1} Min Value: {2}",
                              frameData.Length, frameData.Max(), frameData.Min());
        }
    }
}
