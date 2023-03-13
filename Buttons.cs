using System;
using System.IO.Ports;
using System.Windows;

namespace LogParser
{
    public partial class MainWindow
    {
        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            COM_Port_list.ItemsSource = SerialPort.GetPortNames();
        }
        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            int baudRate = 0;
            try
            {
                if (Convert.ToString(Connect_btn.Content) == "Connect")
                {
                    if (COM_Port_list.SelectedItem != null)
                    {
                        bool isNumeric = int.TryParse(BaudRateBox.Text, out baudRate);
                        if (isNumeric == true)
                        {
                            dispatcherTimer.Start();

                            readData = new ReadDataFromCom();
                            if (readData.OpenPort(COM_Port_list.Text, Convert.ToInt32(BaudRateBox.Text)))
                                Connect_btn.Content = "Disconnect";


                        }
                    }
                }
                else
                {
                    if (readData.ClosePort()) Connect_btn.Content = "Connect";

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
