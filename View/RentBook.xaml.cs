using LS.Model;
using LS.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LS
{
    public partial class RentBook : Page // Thay đổi ở đây
    {
        private RentBookViewModel viewModel = new RentBookViewModel();
        private ObservableCollection<Order_Detail> cartItems;

        public RentBook()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cartItems = new ObservableCollection<Order_Detail>();
            OrderDetailGrid.ItemsSource = cartItems;
            viewModel.LoadUsers(UserComboBox);
            StartDatePicker.SelectedDate = DateTime.Now;
            EndDatePicker.SelectedDate = DateTime.Now.AddDays(14);
        }

        private void SearchBookTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            viewModel.SearchBooks(SearchBookTextBox, BookSearchResultsListView);
        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = BookSearchResultsListView.SelectedItem as Book;
            viewModel.AddBookToCart(selectedBook, cartItems);
            OrderDetailGrid.Items.Refresh();
        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.CommandParameter is int bookIdToRemove)
            {
                var itemToRemove = cartItems.FirstOrDefault(item => item.book_id == bookIdToRemove);
                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                }
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Vì đây là Page, không có 'this' để close, ta sẽ tìm Window cha
            Window parentWindow = Window.GetWindow(this);
            viewModel.ConfirmRental(UserComboBox, StartDatePicker, EndDatePicker, cartItems, parentWindow);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Có thể làm trống các trường thay vì đóng
            // Hoặc điều hướng về trang trước đó
        }
    }
}