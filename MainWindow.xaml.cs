using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace LogParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        ReadDataFromCom readData;
        public MainWindow()
        {
            InitializeComponent();
            COM_Port_list.ItemsSource = SerialPort.GetPortNames();
            BaudRateBox.ItemsSource = baudRateList;
            BaudRateBox.Text = "115200";
        }

        List<int> baudRateList = new List<int>() { 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 56000, 57600, 115200, 128000, 256000 };
        private void AppendText(string text)
        {
            try
            {
                if (Application.Current is not null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        string line = LineSeparator(text);
                        if (line != null)
                        {
                            LogText.Text += line;
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The problem with text output into the TextBox", ex.Message);
            }
        }
        private void onEnterPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter || e.Key == System.Windows.Input.Key.Return)
            {
                baudRateList.Add(Convert.ToInt32(BaudRateBox.Text));
                BaudRateBox.ItemsSource = null;
                BaudRateBox.ItemsSource = baudRateList;
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            LogText.Text = String.Empty;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cleanup();
        }

        private string LineSeparator(string text)
        {
            string delimiter = "[0m";
            StringBuilder sb = new StringBuilder();
            int startIndex = 0;

            int delimiterIndex = text.IndexOf(delimiter);
            while (delimiterIndex != -1)
            {
                sb.Append(text.Substring(startIndex, delimiterIndex - startIndex));
                startIndex = delimiterIndex + delimiter.Length;
                delimiterIndex = text.IndexOf(delimiter, startIndex);
            }

            sb.Append(text.Substring(startIndex));
            return sb.ToString();
        }
    }
}
