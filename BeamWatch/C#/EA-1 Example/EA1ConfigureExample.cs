using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using Ophir.PowerMeter.EA1;

namespace EA1_Standalone
{

    /// <summary>
    /// Example program for discovery and configuration of Ophir's EA-1 Network Adapters.
    /// 
    /// This program depends on the following BeamWatch libraries:
    ///     Ophir.PowerMeter.EA1.dll
    ///     Spiricon.Interfaces.dll
    ///     Spiricon.TreePattern.dll
    ///     Spiricon.TreePattern.Interfaxces.dll
    ///     
    ///     Notes:
    /// 
    ///     The discovery of EA-1 devices uses UDP broadcast and should
    ///     find devices on the network even when not configured for the network.
    ///     
    ///     The enumeration call by default looks on all ethernet and 802.11 wireless
    ///     network adapters. To search on specific adapters, pass those constraints
    ///     to the enumeration call. For example:
    ///             EnumeratePowerMeters("Local Area Connection"); // enumerates devices on the adapter named "Local Area Connection"
    /// 
    ///     In order to reconfigure an EA-1 device, the device must be physically attached
    ///     to a local network.
    ///     
    ///     If the device is not configured for the local network, use the
    ///     EA1Enumerator.ConfigureNetwork() command to configure via UDP Broadcast
    ///     using the device's MAC address as a target.
    ///     
    ///     Another option is to temporarily alter the network adapter settings to match
    ///     the current EA-1 address so we can connect to it via TCP/Telnet.
    ///     
    ///     The EA-1 can only connect to one program/device at a time using TCP/Telnet
    ///     so make sure it is not connected to any BeamWatch instance.
    ///    
    /// 
    ///     Configuration steps:
    ///     
    ///         A.  If you know the device MAC address, you can configure via UDP broadcast.
    ///             (See the example BasicNetworkConfiguration() below.)
    ///         
    ///         B.  If you don't know the device MAC Address, you will need to enumerate
    ///             the devices on the network to find the device.
    ///             
    ///             1.  Enumerate the EA-1 devices on the network.
    ///     
    ///             2.  Select the EA-1 device that needs to be reconfigured.
    ///         
    ///             3.  Call EA1Enumerator.ConfigureNetwork() command with the desired network settings.
    ///                 (See the example DetailedNetworkConfiguration() below.)
    ///                 
    ///     *** PLEASE NOTE: When EA-1 network settings are altered, the device will reset
    ///     and will be unresponsive for several seconds.
    ///             
    ///     BeamWatch should now be able to connect to the EA-1
    ///         
    ///         
    /// </summary>
    class EA1ConfigureExample
    {
        private static EA1Enumerator Enumerator = new EA1Enumerator(null);

        static void Main(string[] args)
        {
            // BasicEnumeration();
            // DetailedEnumeration();

            // NOTE: The ConfigureNetwork command reconfigures the EA-1 network settings and causes
            // a device reset which will make the device unresponsive for a several seconds.

            BasicNetworkConfiguration();
            // DetailedNetworkConfiguration();

            Console.Write("\n\nPress any ket to exit. ");
            Console.ReadKey();
        }

        /// <summary>
        /// Network Configuration Example.
        /// </summary>
        public static void BasicNetworkConfiguration()
        {
            // This will allow network configuration of a device as long as it is physically connected to a local network.
            // It uses UDP broadcast to configure the device even if it is not configured for the subnet.

            PhysicalAddress mac = PhysicalAddress.Parse("00-1E-AF-00-03-48"); // this value should be labeled on the EA-1 case.
            IPAddress desiredAddress = IPAddress.Parse("192.168.1.91"); // here's the ip address we want the EA-1 to have
            IPAddress netMask = IPAddress.Parse("255.255.255.0"); // this is a standard network mask for a clsss C network
            
            var status = Enumerator.ConfigureNetwork(mac, false, desiredAddress, netMask);

            switch (status)
            {
                case EA1Enumerator.Status.SUCCESS:
                    Console.WriteLine($"EA-1 address successfully changed to {desiredAddress}\n");
                    break;
                case EA1Enumerator.Status.TIMEOUT:
                    Console.WriteLine($"EA-1 address change response timed out.\n");
                    break;
                case EA1Enumerator.Status.FAILURE:
                    Console.WriteLine($"EA-1 address change failed.\n");
                    break;
            }
        }

        /// <summary>
        /// Enumerates EA-1 devices using UDP broadcast discovery.
        /// </summary>
        public static void BasicEnumeration()
        {
            DisplayDevices(Enumerator.DiscoveredDevices);
        }

        /// <summary>
        /// Enumerat EA-1 devices using DHCP and then instantiate each device.
        /// This involves connecting to the device and requesting additional information.
        /// If a device is not configured for the network, it will be skipped since 
        /// Telnet communication with the device will not be possible.
        /// </summary>
        public static void DetailedEnumeration()
        {
            if (Enumerator.DiscoveredDevices.Count == 0)
            {
                Console.WriteLine("No EA-1 devices found.");
                return;
            }

            foreach (EA1Info info in Enumerator.DiscoveredDevices)
            {
                if (info.IPAddress.IsInNetwork())
                    DisplayDevice(new EA1PowerMeter(Enumerator.DiscoveredDevices[0]));
                else
                    Console.WriteLine($"{info.Name} at {info.IPAddress} is not configured for the local network.");
            }
        }

        public static void DetailedNetworkConfiguration()
        {
            // Enumerate all EA-1 Devices
            Console.WriteLine("Finding EA-1 devices...");
            Enumerator.EnumeratePowerMeters();

            // List all devices found.
            List<EA1Info> devices = Enumerator.DiscoveredDevices;
            DisplayDevices(devices);

            if (devices.Count > 0)
            {
                // As an example, we'll just use the first EA-1 that was found.
                // Alternatively, you could select by the serial number, MAC address or device name.
                EA1Info info = devices[0];

                IPAddress originalAddress = info.IPAddress;

                // Here are the new network settings we want for the EA-1
                IPAddress desiredAddress = IPAddress.Parse("192.168.1.91"); // here's the ip address we want the EA-1 to have
                IPAddress netMask = IPAddress.Parse("255.255.255.0"); // this is a standard network mask for a clsss C network
                IPAddress gateway = IPAddress.Parse("192.168.1.1");

                if (info.IPAddress.Equals(desiredAddress))
                {
                    Console.WriteLine($"EA-1 Address for {info.Name} @{info.IPAddress} is already configured with the expected IP address.");
                }
                else
                {
                    // Note that if the device isn't configured for the network, we must use UDP broadcast to configure it.
                    // We use the device MAC address to target it.
                    // *NOTE: This is the preferred way to set the address since it works whether the device is on the subnet or not.

                    PhysicalAddress mac = info.MACAddress; // this value should also be labeled on the EA-1 case.
                    Enumerator.ConfigureNetwork(mac, false, desiredAddress, netMask, gateway);

                    // The ConfigureNetwork command reconfigures the EA-1 network settings and causes
                    // a device reset which will make the device unresponsive for a few seconds.

                    Console.WriteLine($"EA-1 Address for {info.Name} Changed from {originalAddress} to {desiredAddress}\n");
                }
            }
        }

        public static void DisplayDevices(List<EA1Info> devices)
        {
            if (devices.Count == 0)
            {
                Console.WriteLine("No EA-1 devices found.");
                return;
            }

            Console.WriteLine($"found {devices.Count} EA-1 devices.");
            foreach (EA1Info info in devices)
                Console.WriteLine($"\t{info.Name} MAC:{info.MACAddress} @{info.IPAddress} ({(info.IPAddress.IsInNetwork() ? "OK" : "NOT IN NETWORK") })");
            Console.WriteLine();
        }

        public static void DisplayDevice(EA1PowerMeter ea1)
        {
            Console.WriteLine($"EA-1 Ethernet Adapter: {ea1.Info.Name}");
            Console.WriteLine($"    Serial Number: {ea1.SerialNumber})");
            Console.WriteLine($"    MAC Address: {ea1.MACAddress})");
            Console.WriteLine($"    Firmware Version: {ea1.FirmwareVersion})");
            Console.WriteLine($"    DHCP: {(ea1.UsingDHCP ? "ON" : "OFF")}");
            Console.WriteLine($"    IP Address: {ea1.Address}");
            Console.WriteLine($"    Network Mask: {ea1.NetMask}");
            Console.WriteLine($"    Gateway IP: {ea1.Gateway}");
            Console.WriteLine($"    Sensor Information: {ea1.SensorInformation}");
            Console.WriteLine();
        }
    }
}
