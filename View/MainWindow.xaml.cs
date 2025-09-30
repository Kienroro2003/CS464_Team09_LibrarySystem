using System;
using System.Windows;
using System.Windows.Controls;

namespace LS
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Tải trang mặc định khi khởi động
            MainFrame.Navigate(new Uri("View/UserWindow.xaml", UriKind.Relative));
        }

        private void MenuItem_UserManagement_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("View/UserWindow.xaml", UriKind.Relative));
        }

        private void MenuItem_BookManagement_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("View/Book.xaml", UriKind.Relative));
        }

        private void MenuItem_RentBook_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("View/RentBook.xaml", UriKind.Relative));
        }

        private void MenuItem_RentalHistory_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("View/RentalHistory.xaml", UriKind.Relative));
        }

        //private void MenuItem_Logout_Click(object sender, RoutedEventArgs e)
        //{
        //    // Mở cửa sổ Login và đóng cửa sổ hiện tại
        //    Login loginWindow = new Login();
        //    loginWindow.Show();
        //    this.Close();
        //}
    }
}