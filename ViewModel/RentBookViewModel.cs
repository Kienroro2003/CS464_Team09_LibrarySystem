using LS.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LS.ViewModel
{
    public class RentBookViewModel
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
                MessageBox.Show("Lỗi tải danh sách độc giả: " + ex.Message);
            }
        }
        
        public void SearchBooks(TextBox searchBox, ListView resultListView)
        {
            string searchText = searchBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                resultListView.ItemsSource = null;
                return;
            }
            try
            {
                var results = db.Books
                                .Where(b => b.name.ToLower().Contains(searchText) && b.quanlity > 0)
                                .ToList();
                resultListView.ItemsSource = results;
                resultListView.DisplayMemberPath = "name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm sách: " + ex.Message);
            }
        }
        
        public void AddBookToCart(Book selectedBook, ObservableCollection<Order_Detail> cartItems)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Vui lòng chọn một cuốn sách để thêm.");
                return;
            }

            var existingItem = cartItems.FirstOrDefault(item => item.book_id == selectedBook.Id);

            if (existingItem != null)
            {
                int currentQuantity = int.Parse(existingItem.quanlity);
                existingItem.quanlity = (currentQuantity + 1).ToString();
            }
            else
            {
                var newItem = new Order_Detail
                {
                    book_id = selectedBook.Id,
                    Book = selectedBook,
                    quanlity = "1"
                };
                cartItems.Add(newItem);
            }
        }
        
        public void ConfirmRental(ComboBox userComboBox, DatePicker startDatePicker, DatePicker endDatePicker, ObservableCollection<Order_Detail> cartItems, Window rentBookWindow)
        {
            if (userComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!startDatePicker.SelectedDate.HasValue || !endDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và kết thúc.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (endDatePicker.SelectedDate.Value < startDatePicker.SelectedDate.Value)
            {
                MessageBox.Show("Ngày kết thúc không thể trước ngày bắt đầu.", "Lỗi ngày", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!cartItems.Any())
            {
                MessageBox.Show("Giỏ hàng trống. Vui lòng thêm sách để thuê.", "Giỏ hàng trống", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newOrder = new Order
                {
                    user_id = (int)userComboBox.SelectedValue,
                    start_date = startDatePicker.SelectedDate.Value,
                    end_date = endDatePicker.SelectedDate.Value,
                    total_book = cartItems.Sum(item => int.Parse(item.quanlity))
                };

                foreach (var cartItem in cartItems)
                {
                    newOrder.Order_Detail.Add(new Order_Detail
                    {
                        book_id = cartItem.book_id,
                        quanlity = cartItem.quanlity
                    });
                }

                db.Orders.Add(newOrder);
                db.SaveChanges();

                MessageBox.Show("Thuê sách thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                rentBookWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi lưu đơn hàng: " + ex.Message, "Lỗi CSDL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
