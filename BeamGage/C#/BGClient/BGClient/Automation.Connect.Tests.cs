
namespace BGClient.Tests
{
    class ConnectionTests
    {   
        /* This class merely serves as an example of the simplicity achieved by abstracting the BGInterface class functionality from your client code.*/
        public static void ConnectTest()
        {
            /* This setup file is installed with BeamGage. */
            string file1 = TestUtilities.GetTestResourceString(@"~Advanced_BeamMaker TEMoo with Gauss fit.bgSetup");

            BGInterface _bgInterface = new BGInterface();
            _bgInterface.Connect(true);
            _bgInterface.LoadSetupFile(file1);

            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.CentroidX);
            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.CentroidY);
            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.D4SigmaMajor);
            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.D4SigmaMinor);
            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.Orientation);
            
            TestUtilities.PauseTest("Press any key to proceed with client disconnect.");
        }

        public static void ReconnectTest()
        {
            BGInterface _bgInterface = new BGInterface();
            _bgInterface.Reconnect(true);

            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.CentroidX);
            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.CentroidY);
            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.D4SigmaMajor);
            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.D4SigmaMinor);
            _bgInterface.GetSpatialResult(BGClient.BGInterface.SpatialResult.Orientation);

            TestUtilities.PauseTest("Press any key to proceed with client disconnect and server shutdown.");

            _bgInterface.Shutdown();
        }
    }
}
