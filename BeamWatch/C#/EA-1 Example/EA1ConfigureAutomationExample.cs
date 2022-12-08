using System;
using System.Collections.Generic;
using BeamWatch.Automation;
using BeamWatch.Automation.Interfaces;

namespace EA1_Automation
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
            // Start BeamWatch Automation client
            Console.WriteLine("Connecting to BeamWatch...");
            _bw = new AutomatedBeamWatch(true);


            // Example of setting the Basler camera IP address

            SetBaslerIP("192.168.10.211");


            // Examples of setting the EA-1 IP address

            SetEA1IP("192.168.10.233", "255.255.255.0"); // Set first EA-1 found to given address.

            SetEA1IPbyMAC("00-1E-AF-00-03-48", "10.41.1.222", "255.255.255.0"); // Setup the EA-1 with the given MAC address.

            Console.WriteLine("Press return to exit.\n");
            Console.Read();
            _bw.Instance.Shutdown();
        }

        public void SetEA1IP(string targetIP, string subnetMask = "255.255.255.0")
        {
            Console.WriteLine($"Configuring EA-1 for {targetIP}.\n");

            List<IANetworkDevice> devices = _bw.NetworkDevices.GetDevices(NetworkDeviceType.EA1);
            if (devices.Count > 0)
            {
                IANetworkDevice ea1 = devices[0];
                Console.WriteLine($"Found EA-1 Adapter at IP: { ea1.IPAddress }");

                // Using UDP to configure the device is the preferred method.
                _bw ??= new AutomatedBeamWatch(true); // Instantiate BeamWatch Automation client if needed
                _bw.NetworkDevices.ConfigureDeviceUDP(ea1, false, targetIP, subnetMask);

                Console.WriteLine($"Address changed to { targetIP }");
            } else
            {
                Console.WriteLine($"No EA-1 Adapters were found.");
            }
        }

        public void SetEA1IPbyMAC(string macAddress, string targetIP, string subnetMask)
        {
            Console.WriteLine($"Configuring EA-1 ({macAddress}) for {targetIP}.\n");
            _bw ??= new AutomatedBeamWatch(true); // Instantiate BeamWatch Automation client if needed

            if (_bw.NetworkDevices.ConfigureEA1(macAddress, false, targetIP, subnetMask))
                Console.WriteLine($"Address changed to { targetIP }");
            else
                Console.WriteLine($"Address change failed or timed out.");
        }

        public bool SetBaslerIP(string newIPAddress, int timeoutSeconds = 20)
        {
            if (_bw.DataSource.Status == ADataSourceStatus.UNAVAILABLE)
            {
                Console.WriteLine("No camera available");
                return false;
            }

            string originalAddress = _bw.DataSource.IPAddress;
            Console.WriteLine($"Found Basler Camera at IP: { originalAddress }");

            _bw.DataSource.IPAddress = newIPAddress;

            return _bw.DataSource.Status != ADataSourceStatus.UNAVAILABLE;
        }
    }
}