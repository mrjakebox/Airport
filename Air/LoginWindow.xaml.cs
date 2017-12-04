using System.Windows;
using System.Windows.Input;

namespace Air
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DradWindow(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
