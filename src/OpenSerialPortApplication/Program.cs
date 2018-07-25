using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace OpenSerialPortApplication
{
    class Program
    {
        private static string GetMainBoardSerialNumber()
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

        private static string GetNetworkSerialNumber()
        {
            var dataForSerial = string.Empty;
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");
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

        static void Main(string[] args)
        {
            Console.WriteLine("当前机器主版编号:" + GetMainBoardSerialNumber());
            Console.WriteLine("当前机器网卡编号:" + GetNetworkSerialNumber());
            Console.Read();
        }
    }
}
