using System;
using System.Windows;

namespace LogParser
{
    public partial class MainWindow
    {
        private void Cleanup()
        {
            try
            {
                if (readData is not null) readData.ClosePort();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something wrong closing the port" + ex.ToString());
            }
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
                            readData = new ReadDataFromCom(AppendBytes);

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

        private void btnCreate_Display_Click(object sender, RoutedEventArgs e)
        {
            LogDisplay ddCustom = new LogDisplay(CloseDisplay, "Custom display", new DisplaySettings(displayConfiguration));
            displays.Add(ddCustom);
            gridForTextBlocks.Children.Add(ddCustom);
        }

        /// <summary>
        /// Clearing all windows.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var d in displays)
                {
                    d.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The problem with closing log window", ex.Message);
            }
        }
    }
}
