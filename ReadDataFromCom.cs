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
        Action<string> onTextReaded = (string s) => { };

        public ReadDataFromCom(Action<string> onTextRecevied)
        {
            onTextReaded = onTextRecevied;
            buffer = new List<byte>();
            framesToExectute = new List<Frame>();
        }
        public bool OpenPort(string comPort, int baudrate)
        {
            try
            {
                _port = new SerialPort(comPort, baudrate);

                _port.DataReceived += _port_DataReceived;
                _port.ReceivedBytesThreshold = 14;

                _port.Open();

                if (_port.IsOpen)
                {
                    readedData = true;
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
        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_port.BytesToRead >= 14)
            {
                readedData = false;
                ReadData(_port.BytesToRead);
            }
        }
        private void ReadData(int count)
        {
            if (_port.IsOpen)
            {
                if (_port.BytesToRead > 0)
                {
                    byte[] bytee = new byte[count];
                    _port.Read(bytee, 0, count);
                    onTextReaded(System.Text.Encoding.UTF8.GetString(bytee));
                }
                if (_port.BytesToRead == 0) readedData = true;
            }
        }
    }
}
