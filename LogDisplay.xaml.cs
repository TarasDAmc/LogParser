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
        List<SingleLineDisplay> lines;
        public LogDisplay()
        {
            lines = new List<SingleLineDisplay>();
            displaySettings = new DisplaySettings();
            InitializeComponent();

            if (displaySettings.showVerbose)
            {
                // ShowInfo.IsChecked = true;
            }

            lbLogTitle.Content = logTitle;
        }
        Action<LogDisplay> onRemove = (LogDisplay d) => { };
        public LogDisplay(Action<LogDisplay> onClose, string title, DisplaySettings ds)
        {
            lines = new List<SingleLineDisplay>();
            this.displaySettings = ds;
            onRemove = onClose;
            this.logTitle = title;
            InitializeComponent();

            if (displaySettings.showVerbose)
            {
                // ShowInfo.IsChecked = true;
            }
            lbLogTitle.Content = logTitle;
        }

        //private void ShowEcho_Checked(object sender, RoutedEventArgs e)
        //{
        //    // if (ShowEcho.IsChecked == true)
        //    {
        //        displaySettings.showVerbose = true;
        //    }
        //    // else
        //    {
        //        displaySettings.showVerbose = false;
        //    }
        //}
        public void Clear() => destination.Text = String.Empty;
        string LineCleaner(string line) => string.Join(" ", line.Split(' ').Where(p => !p.StartsWith("[")));
        string CleanEndLines(string line) => line.Replace("[0m", "");
        /// <summary>
        /// Changing the colour of the line depend on key sequence.
        /// </summary>
        /// <param name="line"></param>
        public void LineAppender(string lineInput)
        {
            if (String.IsNullOrEmpty(lineInput)) return;
            string line = CleanEndLines(lineInput);
            if (line == null) return;

            if (lineInput.Contains("[0;32mI")
                && (displaySettings.showInfo)) // info
            {
                spMainLines.Children.Add(new SingleLineDisplay(LineCleaner(line), Brushes.Green));
            }
            else if (lineInput.Contains("[0;31mE")
                 && (displaySettings.showErrors || displaySettings.showAll)) // error
            {
                spMainLines.Children.Add(new SingleLineDisplay(LineCleaner(line), Brushes.Red));
            }
            else if (lineInput.Contains("[0;33mW")
                 && (displaySettings.showWarning || displaySettings.showAll)) //warning
            {
                spMainLines.Children.Add(new SingleLineDisplay(LineCleaner(line), Brushes.Yellow));
            }
            else if (lineInput.Contains("[0;34m")
                 && (displaySettings.showVerbose || displaySettings.showAll)) // verbose
            {
                spMainLines.Children.Add(new SingleLineDisplay(LineCleaner(line), Brushes.LightBlue));
            }
            else if (lineInput.Contains("[1;34m")
                 && (displaySettings.showVerbose || displaySettings.showAll))// verbose
            {
                spMainLines.Children.Add(new SingleLineDisplay(LineCleaner(line), Brushes.LightSkyBlue));
            }
            else if (displaySettings.showVerbose || displaySettings.showAll)// verbose
            {
                spMainLines.Children.Add(new SingleLineDisplay(LineCleaner(line), Brushes.White));
            }
            mainSv.ScrollToEnd();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            onRemove(this);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }
    }
}
