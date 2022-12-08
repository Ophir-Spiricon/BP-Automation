/* NanoScan v2 CSharp Automation Example
 * 2012 Ophir-Spiricon, LLC
 * Modification of C# sample is permitted
 * See included readme.txt for further instructions
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace NS2_CSharp_Example
{
    partial class Program
    {
        static void Main(string[] args)
        {
            /* InitNsInterop instructs the interop to initialize a connection to the
             * ActiveX server (the interop, not the c# application manages the 
             * ActiveX reference to NanoScan v2).
             */
            if (1 == InitNsInterop())
            {
                //Display GUI.
                NsInteropSetShowWindow(true);

                //Retrieve number of connected scan heads.
                Int16 NumberOfDevices = -1;
                Int32 Scode = NsInteropGetNumDevices(ref NumberOfDevices);
                Console.WriteLine(@"There are {0} devices", NumberOfDevices);

                //Start DAQ so that we have meaningful profile data before reading the profile.
                NsInteropSetDataAcquisition(true);

                // retrieve device list
                object deviceList = new ulong[10];
                NsInteropGetDeviceList(ref deviceList);

                //Wait until the first data is processed before reading the profile.
                WaitForFirstData();

                ReadProfile();

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(false);

                ShutdownNsInterop();
            }
        }

        private static void ReadProfile()
        {
            //currently we are only reading data from Aperture 1 (or X axis) to simplify the code.

            short aperture = 0;
            float leftBound = 0;
            float rightBound = 0;
            float samplingRes = 0;
            short decimation = 1;
            int numOfPoints = 10;

            /* Initialize new arrays that will be marshaled through the COM interop.  The interop
             * will handle conversion from VARIANT to .NET Array.  In this case, they will come
             * back as an array of floats.
             */
            object amplitude = new float[numOfPoints];
            object position = new float[numOfPoints];

            //Calculate the decimation factor to only retrieve numOfPoints
            NsInteropGetApertureLimits(aperture, ref leftBound, ref rightBound);
            NsInteropGetSamplingResolution(aperture, ref samplingRes);
            decimation = (short)((rightBound - leftBound) / samplingRes / numOfPoints);

            //Read the profile data
            NsInteropReadProfile(aperture, leftBound, rightBound, decimation, ref amplitude, ref position);

            //Cast the returned objects to an IEnumerable of type float and then to an array of floats to access the data.
            float[] fAmplitude = ((float[])((IEnumerable<float>)amplitude));
            float[] fPosition = ((float[])((IEnumerable<float>)position));

            //Output data values to the console
            Console.WriteLine(@"Values: (Amplitude, Position)");
            for (var i = 0; i < numOfPoints; i++)
                Console.WriteLine(@"({0}, {1})", fAmplitude[i], fPosition[i]);
        }

        private static void WaitForFirstData()
        {
            /* A valid method of determining whether data has been processed yet is
             * to evaluate whether any Results (Parameters per NS1) have yet been computed.
             * In this example the Centroid position result is used due to its benign
             * nature, i.e. usually enabled and not affected by other settings or results.
             */

            //Enable 13.5% Percent of Peak, D4σ, and Centroid Position results
            NsInteropSelectParameters(0x1 | 0x10 | 0x20);

            bool daqState = false;
            int count = 0;
            float centroidValue = 0;
            do
            {
                //Sleep for a moment while daq initializes
                Thread.Sleep(50);

                //Get the Centroid position result
                NsInteropGetCentroidPosition(0, 0, ref centroidValue);

                //Is the value greater than the default of 0.0?
                if (centroidValue > 0)
                    daqState = true;

                //How many times did we iterate before we had data?
                count++;
            }
            while (daqState == false);

            Console.WriteLine(@"DAQ state: {0} : {1}", daqState, count);
        }
    }
}
