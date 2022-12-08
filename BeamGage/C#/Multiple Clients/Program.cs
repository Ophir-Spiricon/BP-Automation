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
        private AutomatedBeamGage _bg1, _bg2;
        private bool bg_GUI = false;

        public void Run()
        {
            Console.WriteLine("Press enter to exit.\n");
            // Start BeamGage Automation client
            Console.WriteLine("Starting ClientOne GUI: {0}", bg_GUI);
            _bg1 = new AutomatedBeamGage("ClientOne", bg_GUI);
            Console.WriteLine("Starting ClientTwo GUI: {0}", bg_GUI);
            _bg2 = new AutomatedBeamGage("ClientTwo", bg_GUI);

            Console.WriteLine("Stopping data sources...");
            _bg1.DataSource.Stop();
            _bg2.DataSource.Stop();

            //string mySetupFilePath1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
            //    @"\BeamGage\Setups\clientone.bgSetup";
            //_bg1.SaveLoadSetup.LoadSetup(mySetupFilePath1);

            //string mySetupFilePath2 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
            //    @"\BeamGage\Setups\clienttwo.bgSetup";
            //_bg2.SaveLoadSetup.LoadSetup(mySetupFilePath2);

            //Select two data sources
            string[] dsList1 = _bg1.DataSource.DataSourceList;
            string[] dsList2 = _bg2.DataSource.DataSourceList;


            _bg1.DataSource.DataSource = dsList1[2];
            Console.WriteLine("1: Connected to source sn:{0}", dsList1[2]);
            _bg1.DataSource.Stop();

            _bg2.DataSource.DataSource = dsList1[3];
            Console.WriteLine("2: Connected to source sn:{0}", dsList1[3]);
            _bg2.DataSource.Stop();

            // Invoke an ultracal cycle
            Console.Write("Ultracalling ClientOne.... ");
            _bg1.Calibration.Ultracal();
            Console.WriteLine("finished\n");

            Console.Write("Ultracalling ClientTwo.... ");
            _bg2.Calibration.Ultracal();
            Console.WriteLine("finished\n");

            // Create and register for the new frame event
            new AutomationFrameEvents(_bg1.ResultsPriorityFrame).OnNewFrame += NewFrameFunction1;
            new AutomationFrameEvents(_bg2.ResultsPriorityFrame).OnNewFrame += NewFrameFunction2;

            _bg1.DataSource.Start();
            _bg2.DataSource.Start();

            Console.Read();

            // Shutdown BeamGage
            Console.WriteLine("Shutting Down ClientOne");
            _bg1.Instance.Shutdown();
            Console.WriteLine("Shutting Down ClientTwo");
            _bg2.Instance.Shutdown();

            Console.Read();
        }


        /// <summary>
        /// This method is declared above as the delegate method for the OnNewFrame event
        /// Any actions here occur within the BeamGage data aqcuisition cycle
        /// To avoid blocking BeamGage, minimize actions here, or get data and push work to other threads.
        /// </summary>
        private void NewFrameFunction1()
        {
            //Obtain the frame data and use it
            double[] frameData = _bg1.ResultsPriorityFrame.DoubleData;

            // Print the ID and Total counts of the frame
            Console.WriteLine("1: Frame # {0}, Total: {1}, Peak: {2}, Min: {3}\r", (int)_bg1.FrameInfoResults.ID, _bg1.PowerEnergyResults.Total, frameData.Max(), frameData.Min());
        }

        private void NewFrameFunction2()
        {
            //Obtain the frame data and use it
            double[] frameData = _bg2.ResultsPriorityFrame.DoubleData;

            // Print the ID and Total counts of the frame
            Console.WriteLine("2: Frame # {0}, Total: {1}, Peak: {2}, Min: {3}\r", (int)_bg2.FrameInfoResults.ID, _bg2.PowerEnergyResults.Total, frameData.Max(), frameData.Min());
        }
    }
}
