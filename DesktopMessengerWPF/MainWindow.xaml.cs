using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopMessengerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User User { get; set; }
        public MainWindow()
        {
            this.DataContext = this;
            User = new User()
            {
                ProfilePhoto = "Images/elon.png",
                Name ="Elon Musk",
                About = "Ideas about the weekend party!",
            };
            InitializeComponent();
        }

        private void GridWindowTop_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void GridDmItem_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (!(sender is Grid grid))
                return;

            grid.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#F5F5F5"));
        }

        private void GridDmItem_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (!(sender is Grid grid))
                return;

            grid.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void BorderTop_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void TextBoxMessage_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (TextBoxMessage.Foreground == Brushes.DarkGray)
            {
                TextBoxMessage.Foreground = Brushes.Black;
                TextBoxMessage.Text = String.Empty;
            }
        }

        private void TextBoxMessage_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (TextBoxMessage.Foreground == Brushes.Black && 
                String.IsNullOrWhiteSpace(TextBoxMessage.Text))
            {
                TextBoxMessage.Foreground = Brushes.DarkGray;
                TextBoxMessage.Text = "Write a message";
                ListBoxChat.Focus();
            }
        }

        private void ImageSend_OnMouseUp(object sender, MouseButtonEventArgs e)
        {

        }
        private void ImageClose_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }

    public class User  
    {
        public string ProfilePhoto { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
    }
}
