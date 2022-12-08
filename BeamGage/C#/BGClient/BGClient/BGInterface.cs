/* This is an example of a recommended implementation of a C# interface to BeamGage.
 * By following this example, a clean abstraction is creatd from your client codebase
 * to any functionality specific to interacting with BeamGage.
 * 
 * This class should be revised/enhanced to meet the specific needs of the intended automation client.
 * */

using System;
using System.IO;
using Spiricon.BeamGage.Automation;

namespace BGClient
{
    class BGInterface
    {
        BeamGageAutomation _bg;
        IAutomationCLT _clt;
        string _instanceName = "test";

        public BGInterface()
        {
            _bg = null;
            _clt = null;
        }

        public void Connect(bool showGUI)
        {
            Console.WriteLine("Establishing a connection with the automation server...");
            _bg = new BeamGageAutomation();
            _clt = _bg.GetV5Instance(_instanceName, showGUI);

            if (_clt == null)
                Console.WriteLine("BeamGage Automation: OPEN FAILED");
            else
                Console.WriteLine("BeamGage Automation: OPEN SUCCEDED");
        }

        public void Reconnect(bool showGUI)
        {
            //So long as the previous session is still active and has encountered no critical errors, you may reconnect to the session.
            Console.WriteLine("Reestablishing a connection with the automation server...");
            _bg = new BeamGageAutomation();
            _clt = _bg.GetV5Instance(_instanceName, showGUI);

            if (_clt == null)
                Console.WriteLine("BeamGage Automation: OPEN FAILED");
            else
                Console.WriteLine("BeamGage Automation: OPEN SUCCEDED");
        }

        public void StartDataSource()
        {
            string result = string.Empty;

            IADataSource _IDataSource = (IADataSource)_clt.GetInterfaceX("AUTOMATION_DATA_SOURCE");
            _IDataSource.Start();

            if (_IDataSource.Status == ADataSourceStatus.RUNNING)
                result = "SUCCESS";
            else
                result = "FAILED";

            Console.WriteLine("StartDataSource(): {0}({1})", result, _IDataSource.Status);
        }

        public void StopDataSource()
        {
            string result = string.Empty;

            IADataSource _IDataSource = (IADataSource)_clt.GetInterfaceX("AUTOMATION_DATA_SOURCE");
            _IDataSource.Stop();

            if (_IDataSource.Status == ADataSourceStatus.PAUSED)
                result = "SUCCESS";
            else
                result = "FAILED";

            Console.WriteLine("StopDataSource(): {0}({1})", result, _IDataSource.Status);
        }

        public void SetBufferSlot(uint slotValue)
        {
            IAFrameBuffer _IAFrameBuffer = (IAFrameBuffer)_clt.GetInterfaceX("AUTOMATION_FRAME_BUFFER");
            _IAFrameBuffer.BufferSlot = slotValue;
        }

        public AutomationSaveLoadStatus LoadSetupFile(string FilePath)
        {
            Console.WriteLine("Loading Setup('{0}')", FilePath);
            IASaveLoadSetup _ISaveLoad = (IASaveLoadSetup)_clt.GetInterfaceX("AUTOMATION_SAVELOAD_SETUP");
            AutomationSaveLoadStatus _loadStatus = _ISaveLoad.LoadSetup(FilePath);
            Console.WriteLine("Load Setup('{0}') returned '{1}'", Path.GetFileName(FilePath), _loadStatus);
            return _loadStatus;
        }

        public void Ultracal()
        {
            Console.WriteLine("BeamGage Automation: Ultracal Started");
            IACalibration _IACalibration = (IACalibration)_clt.GetInterfaceX("AUTOMATION_CALIBRATION");
            _IACalibration.Ultracal();
            
        }
        
        public void Shutdown()
        {
            IAutomationInstance _IAInstance = (IAutomationInstance)_clt.GetInterfaceX("AUTOMATION_INSTANCE");
            _IAInstance.Shutdown();
            Console.WriteLine("BeamGage Automation: CLOSED");
        }
        
        public ADataSourceStatus CameraStatus
        {
            get
            {
                IADataSource _IDataSource = (IADataSource)_clt.GetInterfaceX("AUTOMATION_DATA_SOURCE");
                Console.WriteLine("Camera Status: {0}", _IDataSource.Status);
                return _IDataSource.Status;
            }
        }

        public CalibrationStatus CalibrationStatus
        {
            get
            {
                IACalibration _ICalibration = (IACalibration)_clt.GetInterfaceX("AUTOMATION_CALIBRATION");
                Console.WriteLine("Camera calibration: {0}", _ICalibration.Status);
                return _ICalibration.Status;
            }
        }

        public double GetSpatialResult(SpatialResult resultType)
        {
            /* NOTE: This method dynamically retrieves the result property based on the enumeration defined below
               The SpatialResult enumeration names must match the IAResultsSpatial property names */
            IAResultsSpatial _IASpatial = (IAResultsSpatial)_clt.GetInterfaceX("AUTOMATION_RESULTS_SPATIAL");
            string resultName = System.Enum.GetName(typeof(SpatialResult), (int)resultType);
            double resultValue;


            resultValue = (double)_IASpatial.GetType().GetProperty(resultName).GetValue(_IASpatial, null);

            Console.WriteLine("{0}: {1}", resultName, resultValue.ToString());
            return resultValue;
        }

        public static void CleanupInstance()
        {
            BGInterface bg = new BGInterface();
            bg.Connect(true);
            bg.Shutdown();
        }

        public enum SpatialResult
        {
            //Must match the enumeration to the property names in the IAResultsSpatial interface
            CentroidX,
            CentroidY,
            PeakLocationX,
            PeakLocationY,
            D4SigmaMajor,
            D4SigmaMinor,
            /* ... */
            Orientation
        }
    }
}
