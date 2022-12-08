using System;
using System.IO;
using System.Threading;

namespace BGClient
{
    public static class TestUtilities
    {
        public static string GetTestResourceString(string inputResource)
        {
            string myDocDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if(inputResource.Contains(".bgSetup"))
            {
                string fileDir = Path.GetFullPath(myDocDir + @"\BeamGage\Setups");
                string filePath = Path.Combine(fileDir, inputResource);
                return filePath;
            }

            return "Resource Not Found";
        }

        public static void PauseTest()
        {
            PauseTest(null);
        }

        public static void PauseTest(string message)
        {
            if(message == null)
                Console.WriteLine("Press the ANY key to continue...");
            else
                Console.WriteLine("{0}\r\n{1}", message, "Press the ANY key to continue...");
            
            while (Console.KeyAvailable == false)
                Thread.Sleep(500);
            
            Console.ReadKey();
            Console.WriteLine("\r\n");
        }
    }
}