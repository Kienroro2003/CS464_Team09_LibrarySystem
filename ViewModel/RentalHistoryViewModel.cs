using LS.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LS.ViewModel
{
    public class RentalHistoryViewModel
    {
        private Database1Entities1 db = new Database1Entities1();

        public void LoadUsers(ComboBox userComboBox)
        {
            try
            {
                userComboBox.ItemsSource = db.Users.ToList();
                userComboBox.DisplayMemberPath = "fullname";
                userComboBox.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách độc giả: " + ex.Message);
            }
        }

        public void LoadOrdersForUser(int userId, DataGrid ordersGrid)
        {
            try
            {
                var orders = db.Orders.Where(o => o.user_id == userId).ToList();
                ordersGrid.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách đơn hàng: " + ex.Message);
            }
        }

        public void LoadDetailsForOrder(int orderId, DataGrid orderDetailsGrid)
        {
            try
            {
                var details = db.Order_Detail
                                .Include("Book")
                                .Where(d => d.order_id == orderId)
                                .ToList();
                orderDetailsGrid.ItemsSource = details;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết đơn hàng: " + ex.Message);
            }
        }
    }
}