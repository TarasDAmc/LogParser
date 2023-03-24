using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogParser
{
    /// <summary>
    /// Interaction logic for LogDisplay.xaml
    /// </summary>
    public partial class LogDisplay : UserControl
    {
        string? logTitle = "Main window"; // showing the title when check box is on;

        DisplaySettings displaySettings;

        Action<LogDisplay> onRemove = (LogDisplay d) => { };
        #region CTOR
        public LogDisplay()
        {
            displaySettings = new DisplaySettings();
            InitializeComponent();
            lbLogTitle.Content = logTitle;
        }
        public LogDisplay(DisplaySettings ds)
        {
            this.displaySettings = ds;
            InitializeComponent();
            lbLogTitle.Content = logTitle;
        }
        public LogDisplay(Action<LogDisplay> onClose, string title, DisplaySettings ds)
        {
            this.displaySettings = ds;
            onRemove = onClose;
            this.logTitle = title;
            InitializeComponent();
            lbLogTitle.Content = logTitle;
        }

        #endregion
        public void Clear() => spMainLines.Children.Clear();
        private void btnClose_Click(object sender, RoutedEventArgs e) => onRemove(this);
        private void btnClear_Click(object sender, RoutedEventArgs e) => Clear();

        /// <summary>
        /// Parsing the line, add it to the memory and send it to the output window.
        /// </summary>
        /// <param name="line"></param>
        public void LineAppender(Dictionary<string, string> valuePairs)
        {
            if (valuePairs == null) return;
            var line = "";
            if (valuePairs.TryGetValue("info", out line) && displaySettings.showInfo)
            {
                spMainLines.Children.Add(new LineDetasils(line, Brushes.Green, FontWeight = FontWeights.Thin));
            } //info
            else if (valuePairs.TryGetValue("error", out line) && displaySettings.showErrors)
            {
                spMainLines.Children.Add(new LineDetasils(line, Brushes.Red, FontWeight = FontWeights.Thin));
            } // error
            else if (valuePairs.TryGetValue("warning", out line) && displaySettings.showWarning)
            {
                spMainLines.Children.Add(new LineDetasils(line, Brushes.Yellow, FontWeight = FontWeights.Thin));
            } //warning
            else if (valuePairs.TryGetValue("echo", out line) && displaySettings.showEcho)
            {
                spMainLines.Children.Add(new LineDetasils(line, Brushes.LightBlue, FontWeight = FontWeights.Thin));
            } // echo
            else if (valuePairs.TryGetValue("bold", out line) && displaySettings.showBold)
            {
                spMainLines.Children.Add(new LineDetasils(line, Brushes.LightSkyBlue, FontWeight = FontWeights.Bold));
            } // bold
            else if (valuePairs.TryGetValue("simple", out line) && displaySettings.showSimple)
            {
                spMainLines.Children.Add(new LineDetasils(line, Brushes.White, FontWeight = FontWeights.Thin));
            } // simple
            mainSv.ScrollToEnd();
        }
    }
}
