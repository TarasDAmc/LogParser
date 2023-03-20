using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogParser
{
    /// <summary>
    /// Interaction logic for SingleLineDisplay.xaml
    /// </summary>
    public partial class SingleLineDisplay : UserControl
    {
        DateTime dt = DateTime.Now;
        public SingleLineDisplay()
        {
            InitializeComponent();
        }
        public SingleLineDisplay(string content, SolidColorBrush c)
        {
            InitializeComponent();
            tbMainText.Foreground = c;
            tbMainText.Text = content;
            // TODO: if found time, than add it as a line parameter.
        }

        private void Open_Details(object sendeer, RoutedEventArgs e)
        {

        }
    }
}
