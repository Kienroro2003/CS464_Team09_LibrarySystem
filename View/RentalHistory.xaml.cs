using LS.Model;
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

namespace LS
{
    /// <summary>
    /// Interaction logic for RentalHistory.xaml
    /// </summary>
    public partial class RentalHistory : Window
    {
        private Database1Entities1 db = new Database1Entities1();

        public RentalHistory()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
            UserComboBox.SelectionChanged += UserComboBox_SelectionChanged;
            OrdersGrid.SelectionChanged += OrdersGrid_SelectionChanged;
        }

        /// <summary>
        /// Tải danh sách người dùng vào ComboBox.
        /// </summary>
        private void LoadUsers()
        {
            try
            {
                UserComboBox.ItemsSource = db.Users.ToList();
                UserComboBox.DisplayMemberPath = "fullname"; 
                UserComboBox.SelectedValuePath = "Id";   
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách độc giả: " + ex.Message);
            }
        }

        /// <summary>
        /// Sự kiện xảy ra khi người dùng thay đổi lựa chọn trong ComboBox.
        /// </summary>
        private void UserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrdersGrid.ItemsSource = null;
            OrderDetailsGrid.ItemsSource = null;

            if (UserComboBox.SelectedItem is User selectedUser)
            {
                LoadOrdersForUser(selectedUser.Id);
            }
        }

        /// <summary>
        /// Tải danh sách đơn hàng của một người dùng cụ thể.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        private void LoadOrdersForUser(int userId)
        {
            try
            {
                var orders = db.Orders.Where(o => o.user_id == userId).ToList();
                OrdersGrid.ItemsSource = orders;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách đơn hàng: " + ex.Message);
            }
        }

        /// <summary>
        /// Sự kiện xảy ra khi người dùng chọn một dòng trong DataGrid đơn hàng.
        /// </summary>
        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrderDetailsGrid.ItemsSource = null;

            if (OrdersGrid.SelectedItem is Order selectedOrder)
            {
                LoadDetailsForOrder(selectedOrder.Id);
            }
        }

        /// <summary>
        /// Tải chi tiết các cuốn sách của một đơn hàng cụ thể.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng.</param>
        private void LoadDetailsForOrder(int orderId)
        {
            try
            {
                var details = db.Order_Detail
                                .Include("Book")
                                .Where(d => d.order_id == orderId)
                                .ToList();
                OrderDetailsGrid.ItemsSource = details;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết đơn hàng: " + ex.Message);
            }
        }
    }
}
