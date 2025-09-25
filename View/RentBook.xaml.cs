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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LS
{
    /// <summary>
    /// Interaction logic for RentBook.xaml
    /// </summary>
    public partial class RentBook : Window
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
            viewModel.ConfirmRental(UserComboBox, StartDatePicker, EndDatePicker, cartItems, this);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
