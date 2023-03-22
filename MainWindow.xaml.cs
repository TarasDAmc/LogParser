using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LogParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        ReadDataFromCom readData;
        List<int> baudRateList = new List<int>() { 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 56000, 57600, 115200, 128000, 256000 };
        List<string> lines = new List<string>();
        List<byte> buffer = new List<byte>();
        string line = "";
        List<LogDisplay> displays = new List<LogDisplay>();

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            COM_Port_list.ItemsSource = SerialPort.GetPortNames();
            BaudRateBox.ItemsSource = baudRateList;
            BaudRateBox.Text = "115200";
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

        private void AppendBytes(byte[] bytes)
        {
            try
            {
                string lineToAdd = LineAllocator(bytes);
                if (Application.Current is not null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (displays.Count == 0)
                        {
                            LogDisplay ddAtLeastWarn = new LogDisplay(CloseDisplay, "At least warnings", new DisplaySettings(DisplayType.atLeastWarning));
                            displays.Add(ddAtLeastWarn);
                            gridForTextBlocks.Children.Add(ddAtLeastWarn);
                            LogDisplay ddAll = new LogDisplay(CloseDisplay, "All", new DisplaySettings(DisplayType.all));
                            displays.Add(ddAll);
                            gridForTextBlocks.Children.Add(ddAll);
                        }

                        foreach (var d in displays)
                        {
                            d.LineAppender(lineToAdd);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The problem with text output into the TextBlock", ex.Message);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => Cleanup();

        /// <summary>
        /// This object difine the end of the line.
        /// </summary>
        /// <param name="bytes"></param>
        private string LineAllocator(byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                buffer.Add(b);
                if (b == 0x0a) // b == '\n'
                {
                    lines.Add(System.Text.Encoding.UTF8.GetString(buffer.ToArray()));
                    buffer.Clear();
                }
                line = lines.FirstOrDefault() ?? String.Empty;
            }
            lines.Clear();
            return line;
        }

        /// <summary>
        /// Closing the display on btnClose Click.
        /// </summary>
        /// <param name="displayToRemove"></param>
        private void CloseDisplay(LogDisplay displayToRemove)
        {
            try
            {
                gridForTextBlocks.Children.Remove(displayToRemove);
                displays.Remove(displayToRemove);
            }
            catch
            {

            }
        }

        List<CheckBox> customWindowConfiguration = new List<CheckBox>();


        private void Window_Configuration(object sender, RoutedEventArgs e)
        {
            //cbAll.IsChecked = true;
            cbInfo.IsChecked = true;
            cbErrors.IsChecked = true;
            cbWarnings.IsChecked = true;
            cbEcho.IsChecked = true;

            //customWindowConfiguration.Add(cbAll);
            customWindowConfiguration.Add(cbInfo);
            customWindowConfiguration.Add(cbErrors);
            customWindowConfiguration.Add(cbWarnings);
            customWindowConfiguration.Add(cbEcho);


        }
    }
}
