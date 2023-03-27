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
        LogMainDisplay mainDisplay = new LogMainDisplay(new DisplaySettings(displayConfiguration));
        List<LogDisplay> displays = new List<LogDisplay>();
        List<Dictionary<string, string>> chakedLines = new List<Dictionary<string, string>>();
        public static List<Dictionary<string, string>> sortedLines = new List<Dictionary<string, string>>();
        static List<CheckBox> displayConfiguration = new List<CheckBox>();
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
        } // Baud rate

        private void AppendBytes(byte[] bytes)
        {
            try
            {
                string lineToAdd = LineAllocator(bytes);
                var parsedLine = new LineHolder();
                var l = parsedLine.LineSegregator(lineToAdd);
                if (Application.Current is not null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (displays.Count > 0)
                        {
                            foreach (var d in displays)
                            {
                                d.LineAppender(l);
                            }
                        }

                        if (chakedLines is not null)
                        {
                            mainDisplay.LineListAppender(chakedLines);
                            chakedLines.Clear();
                        }
                        mainDisplay.LineAppender(l);
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
                gridForTextBlocksMainWindow.Children.Remove(displayToRemove);
                displays.Remove(displayToRemove);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't close the display", ex.Message);
            }
        }
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

            gridForTextBlocksMainWindow.Children.Clear();
            var dl = new LineHolder();

            if (sortedLines is not null)
            {
                var allLines = dl.getAllLines();
                foreach (var d in allLines)
                {
                    if (d.ContainsKey("info") && cbInfo.IsChecked == true) chakedLines.Add(d);
                    else if (d.ContainsKey("error") && cbErrors.IsChecked == true) chakedLines.Add(d);
                    else if (d.ContainsKey("warning") && cbWarnings.IsChecked == true) chakedLines.Add(d);
                    else if (d.ContainsKey("echo") && cbEcho.IsChecked == true) chakedLines.Add(d);
                    else if (d.ContainsKey("bold") && cbBold.IsChecked == true) chakedLines.Add(d);
                    else if (d.ContainsKey("simple") && cbSimple.IsChecked == true) chakedLines.Add(d);
                }
            }
            mainDisplay = new LogMainDisplay(new DisplaySettings(displayConfiguration));
            gridForTextBlocksMainWindow.Children.Add(mainDisplay);

            #region cbAllIsChaked
            cbAll.IsChecked = null;
            if ((cbInfo.IsChecked == true) &&
                (cbErrors.IsChecked == true) &&
                (cbWarnings.IsChecked == true) &&
                (cbEcho.IsChecked == true) &&
                (cbBold.IsChecked == true) &&
                (cbSimple.IsChecked == true))
            {
                cbAll.IsChecked = true;
            }
            if ((cbInfo.IsChecked == false) &&
                (cbErrors.IsChecked == false) &&
                (cbWarnings.IsChecked == false) &&
                (cbEcho.IsChecked == false) &&
                (cbBold.IsChecked == false) &&
                (cbSimple.IsChecked == false))
            {
                cbAll.IsChecked = false;
            }
            #endregion
        }

        private void cbAll_Display_configuration_change(object sender, RoutedEventArgs e)
        {
            bool newVal = (cbAll.IsChecked == true);
            cbInfo.IsChecked = newVal;
            cbErrors.IsChecked = newVal;
            cbWarnings.IsChecked = newVal;
            cbEcho.IsChecked = newVal;
            cbBold.IsChecked = newVal;
            cbSimple.IsChecked = newVal;
        }
    }
}
