using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace LogParser
{
    public class LineHolder
    {
        public List<Dictionary<string, string>> getAllLines()
        {
            List<Dictionary<string, string>> ld = MainWindow.sortedLines;
            return ld;
        }
        public Dictionary<string, string> outputLine(Dictionary<string, string> line)
        {
            try
            {
                if (line is not null) return line;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorted lines has no elements", ex.Message);
            }
            return null;
        }
        string LineCleaner(string line) => string.Join(" ", line.Split(' ').Where(p => !p.StartsWith("[")));
        string CleanEndLines(string line)
        {
            var l = line.Replace("[0m", "");
            return (l.StartsWith("\u001b\r\n")) ? l.Replace("\u001b\r\n", null) : l;
        }
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
        public Dictionary<string, string> LineSegregator(string lineInput)
        {
            if (String.IsNullOrEmpty(lineInput)) return null;
            string line = CleanEndLines(lineInput);
            if (line == null) return null;
            string hasDate = LineDateTimeFounder(line);
            if (hasDate.Contains("[0;32mI"))
            {
                string l = LineCleaner(hasDate);
                var info = new Dictionary<string, string>();
                info.Add("info", l);
                MainWindow.sortedLines.Add(info);
                return outputLine(info);
            } //info
            else if (hasDate.Contains("[0;31mE"))
            {
                string l = LineCleaner(hasDate);
                var error = new Dictionary<string, string>();
                error.Add("error", l);
                MainWindow.sortedLines.Add(error);
                return outputLine(error);
            } // error
            else if (hasDate.Contains("[0;33mW"))
            {
                string l = LineCleaner(hasDate);
                var warning = new Dictionary<string, string>();
                warning.Add("warning", l);
                MainWindow.sortedLines.Add(warning);
                return outputLine(warning);
            } //warning
            else if (hasDate.Contains("[0;34m"))
            {
                string l = LineCleaner(hasDate);
                var echo = new Dictionary<string, string>();
                echo.Add("echo", l);
                MainWindow.sortedLines.Add(echo);
                return outputLine(echo);
            } // echo
            else if (hasDate.Contains("[1;34m"))
            {
                string l = LineCleaner(hasDate);
                var bold = new Dictionary<string, string>();
                bold.Add("bold", l);
                MainWindow.sortedLines.Add(bold);
                return outputLine(bold);
            } // bold
            else if ((!hasDate.Contains("[")))
            {
                string l = LineCleaner(hasDate);
                var simple = new Dictionary<string, string>();
                simple.Add("simple", l);
                MainWindow.sortedLines.Add(simple);
                return outputLine(simple);
            } // simple
            return null;
        }
    }
}
