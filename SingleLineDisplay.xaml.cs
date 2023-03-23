using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LogParser
{
    /// <summary>
    /// Interaction logic for SingleLineDisplay.xaml
    /// </summary>
    public partial class Open_Details : UserControl
    {
        public Open_Details()
        {
            InitializeComponent();
        }
        public Open_Details(string content, SolidColorBrush c)
        {
            InitializeComponent();
            tbMainText.Foreground = c;
            tbMainText.Text = content;
            tbMainText.TextAlignment = TextAlignment.Left;
        }

        public Open_Details(string content, SolidColorBrush c, FontWeight weight)
        {
            InitializeComponent();
            tbMainText.Foreground = c;
            tbMainText.Text = content;
            tbMainText.FontWeight = weight;
            tbMainText.TextAlignment = TextAlignment.Left;
        }

        public void Open_Details_Window(object sender, RoutedEventArgs e)
        {
            var message = new LogDetailsWindow();
            message.tbDetail.Text = tbMainText.Text;
            message.Foreground = tbMainText.Foreground;
            message.FontWeight = tbMainText.FontWeight;
            message.Show();
        }
    }
}
