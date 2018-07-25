using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Whitestone.OpenSerialPortMonitor.SerialCommunication;

namespace OpenSerialPortWindowsService
{
    public partial class OpenSerialPortService : ServiceBase
    {
        private SerialReader _serialReader;
        private int _rawDataCounter = 0;
        public OpenSerialPortService()
        {
            InitializeComponent();

            _serialReader = new SerialReader();
        }

        void SerialDataReceived(object sender, Whitestone.OpenSerialPortMonitor.SerialCommunication.SerialDataReceivedEventArgs e)
        {
            Logger.Instance.PrintLine(System.Text.Encoding.ASCII.GetString(e.Data));

            //foreach (byte data in e.Data)
            //{
            //    _rawDataCounter = _rawDataCounter + 1;

            //    char character = (char)data;
            //    if (data <= 31 ||
            //        data == 127)
            //    {
            //        character = '.';
            //    }

            //    Logger.Instance.PrintLine(string.Format("{0:x2} ", data));
            //    Logger.Instance.PrintLine(character.ToString());

            //    if (_rawDataCounter > 0 && _rawDataCounter % 16 == 15)
            //    {
            //        Logger.Instance.PrintLine("\r\n");
            //        Logger.Instance.PrintLine("\r\n");
            //        _rawDataCounter = 0;
            //    }
            //}
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
                        dataForSerial += lan["MACAddress"] + "-";
                    }
                }
            }
            return dataForSerial;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Logger.Instance.PrintLine("启动Open Serial Port Service!");

                var comPort = ConfigurationManager.AppSettings["ComPort"];
                var baudRate = ConfigurationManager.AppSettings["BaudRate"];
                var baudRateValue = baudRate == null ? 9600 : int.Parse(baudRate);
                var dataBits = ConfigurationManager.AppSettings["DataBits"];
                var dataBitsValue = dataBits == null ? 8 : int.Parse(dataBits);

                _serialReader.Start(comPort, baudRateValue, Parity.None, dataBitsValue, StopBits.One);
                _serialReader.SerialDataReceived += SerialDataReceived;
            }
            catch (Exception err)
            {
                Logger.Instance.PrintLine("启动Open Serial Port Service失败,错误信息:" + err.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                _serialReader.Stop();
            }
            catch (Exception err)
            {
                Logger.Instance.PrintLine("关闭Open Serial Port Service失败,错误信息:" + err.Message);
            }
        }
    }
}
