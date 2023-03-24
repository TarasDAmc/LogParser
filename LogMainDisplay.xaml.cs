using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogParser
{
    /// <summary>
    /// Interaction logic for LogMainDisplay.xaml
    /// </summary>
    public partial class LogMainDisplay : UserControl
    {
        string? logTitle = "Main window";

        DisplaySettings displaySettings;
        public LogMainDisplay()
        {
            displaySettings = new DisplaySettings();
            InitializeComponent();
            lbLogTitle.Content = logTitle;
        }
        public LogMainDisplay(DisplaySettings ds)
        {
            displaySettings = ds;
            InitializeComponent();
            lbLogTitle.Content = logTitle;
        }
        public void Clear() => spMainWindowLines.Children.Clear();
        private void btnClear_MD_Click(object sender, RoutedEventArgs e) => Clear();
        /// <summary>
        /// Filling the line property depends on the key value.
        /// </summary>
        /// <param name="valuePairs"></param>
        public void LineAppender(Dictionary<string, string> valuePairs)
        {
            if (valuePairs == null) return;
            var line = "";
            if (valuePairs.TryGetValue("info", out line) && (displaySettings.showInfo))
            {
                spMainWindowLines.Children.Add(new LineDetasils(line, Brushes.Green, FontWeight = FontWeights.Thin));
            } //info
            else if (valuePairs.TryGetValue("error", out line) && (displaySettings.showErrors))
            {
                spMainWindowLines.Children.Add(new LineDetasils(line, Brushes.Red, FontWeight = FontWeights.Thin));
            } // error
            else if (valuePairs.TryGetValue("warning", out line) && (displaySettings.showWarning))
            {
                spMainWindowLines.Children.Add(new LineDetasils(line, Brushes.Yellow, FontWeight = FontWeights.Thin));
            } //warning
            else if (valuePairs.TryGetValue("echo", out line) && (displaySettings.showEcho))
            {
                spMainWindowLines.Children.Add(new LineDetasils(line, Brushes.LightBlue, FontWeight = FontWeights.Thin));
            } // echo
            else if (valuePairs.TryGetValue("bold", out line) && (displaySettings.showBold))
            {
                spMainWindowLines.Children.Add(new LineDetasils(line, Brushes.LightSkyBlue, FontWeight = FontWeights.Bold));
            } // bold
            else if (valuePairs.TryGetValue("simple", out line) && displaySettings.showSimple)
            {
                spMainWindowLines.Children.Add(new LineDetasils(line, Brushes.White, FontWeight = FontWeights.Thin));
            } // simple
            mainSv.ScrollToEnd();
        }
        /// <summary>
        /// Sending all the lines saved in memory to the LineAppender
        /// </summary>
        /// <param name="valuePairsList"></param>
        public void LineListAppender(List<Dictionary<string, string>> valuePairsList)
        {
            if (valuePairsList == null) return;
            foreach (var pair in valuePairsList)
            {
                LineAppender(pair);
            }
        }
    }
}
