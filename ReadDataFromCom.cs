﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;

namespace LogParser
{
    class ReadDataFromCom
    {
        SerialPort _port;
        bool readedData;   // if readBytesToread>0=false
        List<string> textList;

        Action<string> onTextReaded = (string s) => { };
        public ReadDataFromCom() { }
        public ReadDataFromCom(Action<string> onTextRecevied)
        {
            textList = new List<string>();
            onTextReaded = onTextRecevied;
        }
        public bool OpenPort(string comPort, int baudrate)
        {
            try
            {
                _port = new SerialPort(comPort, baudrate);
                _port.DataReceived += _port_DataReceived;
                _port.ReceivedBytesThreshold = 14; // why we use exactly "14"?

                _port.Open();

                if (_port.IsOpen)
                {
                    readedData = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong with the port" + ex.ToString());
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
                    _port.Read(bytee, 0, count); // here was throwed an "invalid operation exception" (the port was closed)
                    onTextReaded(System.Text.Encoding.UTF8.GetString(bytee));
                }
                if (_port.BytesToRead == 0) readedData = true;
            }
        }
    }
}
