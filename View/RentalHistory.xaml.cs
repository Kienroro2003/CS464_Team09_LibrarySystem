using LS.Model;
using LS.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace LS
{
    public partial class RentalHistory : Window
    {
        private RentalHistoryViewModel viewModel = new RentalHistoryViewModel();

        public RentalHistory()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.LoadUsers(UserComboBox);
        }

        private void UserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrdersGrid.ItemsSource = null;
            OrderDetailsGrid.ItemsSource = null;

            if (UserComboBox.SelectedItem is User selectedUser)
            {
                viewModel.LoadOrdersForUser(selectedUser.Id, OrdersGrid);
            }
        }

        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrderDetailsGrid.ItemsSource = null;

            if (OrdersGrid.SelectedItem is Order selectedOrder)
            {
                viewModel.LoadDetailsForOrder(selectedOrder.Id, OrderDetailsGrid);
            }
        }
    }
}