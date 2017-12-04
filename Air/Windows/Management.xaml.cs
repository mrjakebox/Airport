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
using System.Windows.Shapes;

namespace Air.Windows
{
    /// <summary>
    /// Логика взаимодействия для Management.xaml
    /// </summary>
    public partial class Management : Window
    {
        public Management()
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

        private void LogOutClick(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            ClearCache();
            loginWindow.Show();
            Close();
        }

        private void ClearCache() { }
    }
}
