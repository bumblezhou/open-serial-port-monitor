using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OpenSerialPortUnitTest
{
    [TestFixture]
    public class GetMachineSerialNumberTest
    {
        private string GetSerialNumber()
        {
            string dataForSerial = string.Empty;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_SystemEnclosure");
            foreach (ManagementObject enclosure in searcher.Get())
            {

                if (enclosure["SMBIOSAssetTag"] != null)
                {
                    dataForSerial += enclosure["SMBIOSAssetTag"].ToString() + "\n";
                }
                if (enclosure["SerialNumber"] != null)
                {
                    dataForSerial += enclosure["SerialNumber"].ToString() + "\n";
                }
            }

            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            foreach (ManagementObject processor in searcher.Get())
            {
                if (processor["Name"] != null)
                {
                    dataForSerial += processor["Name"].ToString().Trim() + "\n";
                }
            }

            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject board in searcher.Get())
            {

                if (board["Manufacturer"] != null)
                {
                    dataForSerial += board["Manufacturer"].ToString() + "\n";
                }
                if (board["Product"] != null)
                {
                    dataForSerial += board["Product"].ToString() + "\n";
                }
                if (board["SerialNumber"] != null)
                {
                    dataForSerial += board["SerialNumber"].ToString() + "\n";
                }
            }

            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");
            foreach (ManagementObject lan in searcher.Get())
            {
                if (lan["MACAddress"] != null)
                {
                    if (lan["PNPDeviceID"].ToString().StartsWith("PCI", StringComparison.OrdinalIgnoreCase))
                    {
                        dataForSerial += lan["MACAddress"].ToString() + "\n";
                    }
                }
            }

            return dataForSerial;
        }

        private string GetMainBoardSerialNumber()
        {
            var dataForSerial = string.Empty;
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject board in searcher.Get())
            {
                if (board["SerialNumber"] != null)
                {
                    dataForSerial += board["SerialNumber"].ToString() + "\n";
                }
            }
            return dataForSerial;
        }

        private string GetNetworkSerialNumber()
        {
            var dataForSerial = string.Empty;
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");
            foreach (ManagementObject lan in searcher.Get())
            {
                if (lan["MACAddress"] != null)
                {
                    if (lan["PNPDeviceID"].ToString().StartsWith("PCI", StringComparison.OrdinalIgnoreCase))
                    {
                        dataForSerial += lan["MACAddress"].ToString() + "-";
                    }
                }
            }
            return dataForSerial;
        }

        [Test]
        public void test_get_machine_serial_number()
        {
            Console.WriteLine(GetNetworkSerialNumber());
        }
    }
}
