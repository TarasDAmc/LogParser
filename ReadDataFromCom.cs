using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;

namespace LogParser
{
    class ReadDataFromCom
    {
        SerialPort _port;
        List<byte> buffer;
        public List<Frame> framesToExectute;
        bool readedData;   // if readBytesToread>0=false
        MainWindow m;
        public ReadDataFromCom()
        {
            buffer = new List<byte>();
            framesToExectute = new List<Frame>();
        }
        public bool OpenPort(string comPort, int baudrate)
        {
            try
            {
                _port = new SerialPort(comPort, baudrate);

                // _port.DataReceived += _port_DataReceived;
                _port.ReceivedBytesThreshold = 14;

                _port.Open();

                //VMS_COMM.SetSerialPort(_port);

                if (_port.IsOpen)
                {
                    readedData = true;
                    //tim1.Start();
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Something wrong with the port" + e.ToString());
                return false;
            }
            return false;
        }

        public bool ClosePort()
        {
            _port.Close();
            if (!_port.IsOpen) return true;
            else return false;
        }
    }
}
