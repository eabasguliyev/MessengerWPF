using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using TextBlock = System.Windows.Controls.TextBlock;

namespace DesktopMessengerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public User ReceiverUser { get; set; }
        public User SenderUser { get; set; }

        private List<Grid> _chatGrids;

        private List<string> _loremIpsums;
        private Random _random;
        public MainWindow()
        {
            this.DataContext = this;

            var currentDirectoryPath = Directory.GetCurrentDirectory();
            ReceiverUser = new User()
            {
                ProfilePhoto = $@"{currentDirectoryPath}\Images\elon-musk.png",
                Name ="Elon Musk",
                About = "Ideas about the weekend party!",
            };

            SenderUser = new User()
            {
                ProfilePhoto = $@"{currentDirectoryPath}\Images\amber-heard.jpg",
                Name = "Amber Heard",
                About = "Ideas about the weekend party!",
            };
            InitializeComponent();

            _chatGrids = new List<Grid>();

            _loremIpsums = new List<string>()
            {
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                "auctor neque vitae tempus quam pellentesque nec nam aliquam sem",
                "amet risus nullam eget felis eget nunc lobortis mattis aliquam",
                "elit scelerisque mauris pellentesque pulvinar",
                "laoreet non curabitur gravida arcu",
                "aliquet nibh praesent tristique magna",
                "egestas maecenas pharetra convallis posuere morbi leo urna",
                "egestas maecenas pharetra convallis posuere morbi leo urna"
            };

            _random = new Random();
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

        private void ImageClose_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private Grid CreateMessagePanel(bool sender)
        {
            var grid = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition()
                    {
                        Width = new GridLength(70),
                    },
                    new ColumnDefinition()
                    {
                        Width = new GridLength(),
                    },
                },
            };
            
            var stackPanel = new StackPanel();

            var ellipse = new Ellipse()
            {
                StrokeThickness = 1,
                Height = 50,
                Width = 50,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(5),
            };

            stackPanel.Children.Add(ellipse);

            stackPanel.Children.Add(new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = "12:47",
            });

            grid.Children.Add(stackPanel);

            Grid.SetColumn(stackPanel, 0);
            
            var border = new Border()
            {
                BorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(15),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
            };

            border.Background = sender
                ? new SolidColorBrush((Color) ColorConverter.ConvertFromString("#859FFE"))
                : Brushes.White;


            var stackPanel2 = new StackPanel() { Orientation = Orientation.Vertical};

            var textBlock = new TextBlock()
            {
                Foreground = sender ? Brushes.White : Brushes.Black,
                MaxWidth = 300,
                TextWrapping = TextWrapping.Wrap
            };


            textBlock.Text = "Data";

            stackPanel2.Children.Add(textBlock);

            border.Child = stackPanel2;

            grid.Children.Add(border);

            Grid.SetColumn(border, 1);
            return grid;
        }

        private void ImageSend_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var newGrid = CreateMessagePanel(true);

            LoadMessageData(ReceiverUser, TextBoxMessage.Text.ToString(), newGrid);

            _chatGrids.Add(newGrid);

            TextBoxMessage.Text = string.Empty;
            RefreshChat();

            RecevieMessage();
        }

        private void RefreshChat()
        {
            ListBoxChat.ItemsSource = null;

            ListBoxChat.ItemsSource = _chatGrids;
        }
        private void LoadMessageData(User user, string message, Grid grid)
        {
            var panel = grid.Children[0] as StackPanel;


            var ellipse = panel.Children[0] as Ellipse;

            ellipse.Fill = new ImageBrush(new BitmapImage(new Uri(user.ProfilePhoto)));

            var textBlockDate = panel.Children[1] as TextBlock;

            textBlockDate.Text = DateTime.Now.ToShortTimeString();
            
            var border = grid.Children[1] as Border;

            var stackPanel = border.Child as StackPanel;

            var textBlock = stackPanel.Children[0] as TextBlock;

            textBlock.Text = message;
        }

        
        private string GetRandomMessage()
        {
            return _loremIpsums[_random.Next(_loremIpsums.Count)];
        }

        private void RecevieMessage()
        {
            Task.Run(() =>
            {
                Thread.Sleep(500);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    var newGrid = CreateMessagePanel(false);

                    LoadMessageData(SenderUser, GetRandomMessage(), newGrid);

                    _chatGrids.Add(newGrid);

                    RefreshChat();
                });
            });
        }

        private void GetLastMessage()
        {
            
        }
    }

    public class User  
    {
        public string ProfilePhoto { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
    }
}
