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
        string? logTitle = "Main window"; // showing the title when check box is on;

        DisplaySettings displaySettings;
        public static List<Dictionary<string, Open_Details>> sortedLines;

        Action<LogDisplay> onRemove = (LogDisplay d) => { };
        #region CTOR
        public LogDisplay()
        {
            sortedLines = new List<Dictionary<string, Open_Details>>();
            displaySettings = new DisplaySettings();
            InitializeComponent();
            lbLogTitle.Content = logTitle;
        }
        public LogDisplay(DisplaySettings ds)
        {
            sortedLines = new List<Dictionary<string, Open_Details>>();
            displaySettings = new DisplaySettings();
            InitializeComponent();
            lbLogTitle.Content = logTitle;
        }
        public LogDisplay(Action<LogDisplay> onClose, string title, DisplaySettings ds)
        {
            sortedLines = new List<Dictionary<string, Open_Details>>();
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
        /// Parsing the line, add it to the memory and send it to the output window.
        /// </summary>
        /// <param name="line"></param>
        public void LineAppender(string lineInput)
        {
            if (String.IsNullOrEmpty(lineInput)) return;
            string line = CleanEndLines(lineInput);
            string hasDate = LineDateTimeFounder(line);
            if (line == null) return;
            if (lineInput.Contains("[0;32mI") && (displaySettings.showInfo))
            {
                var info = new Dictionary<string, Open_Details>();
                var od = new Open_Details(LineCleaner(hasDate), Brushes.Green, FontWeight = FontWeights.Thin);
                spMainLines.Children.Add(od);
                info.Add("info", od);
                sortedLines.Add(info);
            } //info
            else if (lineInput.Contains("[0;31mE") && (displaySettings.showErrors))
            {
                var error = new Dictionary<string, Open_Details>();
                var od = new Open_Details(LineCleaner(hasDate), Brushes.Red, FontWeight = FontWeights.Thin);
                spMainLines.Children.Add(od);
                error.Add("error", od);
                sortedLines.Add(error);
            } // error
            else if (lineInput.Contains("[0;33mW") && (displaySettings.showWarning))
            {
                var warning = new Dictionary<string, Open_Details>();
                var od = new Open_Details(LineCleaner(hasDate), Brushes.Yellow, FontWeight = FontWeights.Thin);
                spMainLines.Children.Add(od);
                warning.Add("warning", od);
                sortedLines.Add(warning);
            } //warning
            else if (lineInput.Contains("[0;34m") && (displaySettings.showEcho))
            {
                var echo = new Dictionary<string, Open_Details>();
                var od = new Open_Details(LineCleaner(hasDate), Brushes.LightBlue, FontWeight = FontWeights.Thin);
                spMainLines.Children.Add(od);
                echo.Add("echo", od);
                sortedLines.Add(echo);
            } // echo
            else if (lineInput.Contains("[1;34m") && (displaySettings.showBold))
            {
                var bold = new Dictionary<string, Open_Details>();
                var od = new Open_Details(LineCleaner(hasDate), Brushes.LightSkyBlue, FontWeight = FontWeights.Bold);
                spMainLines.Children.Add(od);
                bold.Add("bold", od);
                sortedLines.Add(bold);
            } // bold
            else if ((!lineInput.Contains("[")) && displaySettings.showSimple)
            {
                var simple = new Dictionary<string, Open_Details>();
                var od = new Open_Details(LineCleaner(hasDate), Brushes.White, FontWeight = FontWeights.Thin);
                spMainLines.Children.Add(od);
                simple.Add("simple", od);
                sortedLines.Add(simple);
            } // simple
            mainSv.ScrollToEnd();


        }
    }
}
