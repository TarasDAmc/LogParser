using System;
using System.Collections.Generic;
using System.Linq;
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
        string? logTitle = null; // showing the title when check box is on;

        DisplaySettings displaySettings;
        List<Open_Details> lines;

        Action<LogDisplay> onRemove = (LogDisplay d) => { };
        #region CTOR
        public LogDisplay()
        {
            lines = new List<Open_Details>();
            displaySettings = new DisplaySettings();
            InitializeComponent();
            lbLogTitle.Content = logTitle;
        }
        public LogDisplay(Action<LogDisplay> onClose, string title, DisplaySettings ds)
        {
            lines = new List<Open_Details>();
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

        string LineCleaner(string line) => string.Join(" ", line.Split(' ').Where(p => !p.StartsWith("[")));
        string CleanEndLines(string line) => line.Replace("[0m", "");

        string LineDateTimeFounder(string line)
        {
            string newLine = line;
            if (line.Contains(")"))
            {
                int endIndex = line.IndexOf(")");
                int startIndex = line.IndexOf(")") - 7;
                if (endIndex > 0)
                {
                    string substring = line.Substring(startIndex, endIndex - startIndex);
                    if (int.TryParse(substring, out int numeric))
                    {
                        DateTime dateTime = new DateTime(numeric);
                        string dt = dateTime.ToString();
                        newLine = line.Replace(substring, dt);
                    }
                }
                return newLine;
            }
            else
            {
                return newLine;
            }
        }
        /// <summary>
        /// Changing the colour of the line depend on key sequence.
        /// </summary>
        /// <param name="line"></param>
        public void LineAppender(string lineInput)
        {
            if (String.IsNullOrEmpty(lineInput)) return;
            string line = CleanEndLines(lineInput);
            string hasDate = LineDateTimeFounder(line);
            if (line == null) return;

            if (lineInput.Contains("[0;32mI") && (displaySettings.showInfo)) // info
            {
                spMainLines.Children.Add(new Open_Details(LineCleaner(hasDate), Brushes.Green, FontWeight = FontWeights.Thin));
                lines.Add(new Open_Details(LineCleaner(hasDate), Brushes.Green, FontWeight = FontWeights.Thin));
            }
            else if (lineInput.Contains("[0;31mE") && (displaySettings.showErrors)) // error
            {
                spMainLines.Children.Add(new Open_Details(LineCleaner(hasDate), Brushes.Red, FontWeight = FontWeights.Thin));
                lines.Add(new Open_Details(LineCleaner(hasDate), Brushes.Red, FontWeight = FontWeights.Thin));
            }
            else if (lineInput.Contains("[0;33mW") && (displaySettings.showWarning)) //warning
            {
                spMainLines.Children.Add(new Open_Details(LineCleaner(hasDate), Brushes.Yellow, FontWeight = FontWeights.Thin));
                lines.Add(new Open_Details(LineCleaner(hasDate), Brushes.Yellow, FontWeight = FontWeights.Thin));
            }
            else if (lineInput.Contains("[0;34m") && (displaySettings.showEcho)) // echo
            {
                spMainLines.Children.Add(new Open_Details(LineCleaner(hasDate), Brushes.LightBlue, FontWeight = FontWeights.Thin));
                lines.Add(new Open_Details(LineCleaner(hasDate), Brushes.LightBlue, FontWeight = FontWeights.Thin));
            }
            else if (lineInput.Contains("[1;34m") && (displaySettings.showBold))// bold
            {
                spMainLines.Children.Add(new Open_Details(LineCleaner(hasDate), Brushes.LightBlue, FontWeight = FontWeights.Bold));
                lines.Add(new Open_Details(LineCleaner(hasDate), Brushes.LightSkyBlue, FontWeight = FontWeights.Bold));
            }
            else if ((!lineInput.Contains("[")) && displaySettings.showSimple)// simple
            {
                spMainLines.Children.Add(new Open_Details(LineCleaner(hasDate), Brushes.White, FontWeight = FontWeights.Thin));
                lines.Add(new Open_Details(LineCleaner(hasDate), Brushes.White, FontWeight = FontWeights.Thin));
            }
            mainSv.ScrollToEnd();
        }
    }
}
