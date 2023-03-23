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
        LogDisplay mainDisplay = new LogDisplay();
        List<LogDisplay> displays = new List<LogDisplay>();
        public static List<Open_Details> logs = new List<Open_Details>();

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
                        if (displays.Count > 0)
                        {
                            foreach (var d in displays)
                            {
                                d.LineAppender(lineToAdd);
                            }

                        }
                        mainDisplay.LineAppender(lineToAdd);
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
            catch (Exception ex)
            {
                MessageBox.Show("Can't close display", ex.Message);
            }
        }
        private void CloseMainDisplay(LogMainDisplay mainDisplay)
        {
            try
            {
                gridForTextBlocksMainWindow.Children.Remove(mainDisplay);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't close main display", ex.Message);
            }
        }

        static List<CheckBox> displayConfiguration = new List<CheckBox>();
        private void Window_Configuration(object sender, RoutedEventArgs e)
        {
            cbInfo.IsChecked = true;
            cbErrors.IsChecked = true;
            cbWarnings.IsChecked = true;
            cbEcho.IsChecked = true;
            cbBold.IsChecked = true;
            cbSimple.IsChecked = true;

            displayConfiguration.Add(cbInfo);
            displayConfiguration.Add(cbErrors);
            displayConfiguration.Add(cbWarnings);
            displayConfiguration.Add(cbEcho);
            displayConfiguration.Add(cbBold);
            displayConfiguration.Add(cbSimple);
        }

        private void cb_Display_configuration_change(object checkBox, RoutedEventArgs e)
        {
            foreach (CheckBox c in displayConfiguration)
            {
                if (c.IsChecked == false) c.IsChecked = false;
                else c.IsChecked = true;
            }

            if (LogDisplay.sortedLines is not null)
            {
                foreach (var d in LogDisplay.sortedLines)
                {
                    var details = new Open_Details();
                    if (d.TryGetValue("info", out details) && cbInfo.IsChecked == true) logs.Add(details);
                    else if (d.TryGetValue("error", out details) && cbErrors.IsChecked == true) logs.Add(details);
                    else if (d.TryGetValue("warning", out details) && cbWarnings.IsChecked == true) logs.Add(details);
                    else if (d.TryGetValue("echo", out details) && cbEcho.IsChecked == true) logs.Add(details);
                    else if (d.TryGetValue("bold", out details) && cbBold.IsChecked == true) logs.Add(details);
                    else if (d.TryGetValue("simple", out details) && cbSimple.IsChecked == true) logs.Add(details);
                }
            }
            gridForTextBlocksMainWindow.Children.Clear();
            mainDisplay = new LogDisplay(CloseDisplay, "Main Display", new DisplaySettings(displayConfiguration));
            gridForTextBlocksMainWindow.Children.Add(mainDisplay);
        }
    }
}
