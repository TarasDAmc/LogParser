using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
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
        List<int> baudRateList = new List<int>() { 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 56000, 57600, 115200, 128000, 256000 };
        public MainWindow()
        {
            InitializeComponent();
            COM_Port_list.ItemsSource = SerialPort.GetPortNames();
            BaudRateBox.ItemsSource = baudRateList;
            BaudRateBox.Text = "115200";
        }

        List<string> lines = new List<string>();
        List<byte> buffer = new List<byte>();
        string line = null;
        private void AppendBytes(byte[] bytes)
        {
            try
            {
                foreach (byte b in bytes)
                {
                    buffer.Add(b);
                    if (b == 0x0a) // b == '\n'
                    {
                        lines.Add(System.Text.Encoding.UTF8.GetString(buffer.ToArray()));
                        buffer.Clear();
                    }
                    line = lines.FirstOrDefault();

                }
                if (Application.Current is not null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (line is not null)
                        {
                            LineBrusher(line);
                            //LogText.Inlines.Add(line);
                            lines.Clear();

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
        public void LineBrusher(string line)
        {
            if (line.Contains("[0;32mI"))
            {
                Run run = new Run(LineCleaner(line));
                run.Foreground = Brushes.Green;
                LogText.Inlines.Add(run);
            }
            else if (line.Contains("[0;31mE"))
            {
                Run run = new Run(LineCleaner(line));
                run.Foreground = Brushes.Red;
                LogText.Inlines.Add(run);
            }
            else if (line.Contains("[0;33mW"))
            {
                Run run = new Run(LineCleaner(line));
                run.Foreground = Brushes.Yellow;
                LogText.Inlines.Add(run);
            }
            else if (line.Contains("[0;34m"))
            {
                Run run = new Run(LineCleaner(line));
                run.Foreground = Brushes.LightBlue;
                LogText.Inlines.Add(run);
            }
            else if (line.Contains("[1;34m"))
            {
                Run run = new Run(LineCleaner(line));
                run.Foreground = Brushes.LightSkyBlue;
                LogText.Inlines.Add(run);
            }
            else
            {
                Run run = new Run(LineCleaner(line));
                run.Foreground = Brushes.White;
                LogText.Inlines.Add(run);
            }
        }

        public static string LineCleaner(string line)
        {
            return line = string.Join(" ", line.Split(' ').Where(p => !p.StartsWith("["))).Replace("[0m", "");
        }
    }
}
