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
        // Khởi tạo DB context và giỏ hàng
        private Database1Entities1 db = new Database1Entities1();
        private ObservableCollection<Order_Detail> cartItems;

        public RentBook()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Khởi tạo giỏ hàng và gán cho DataGrid
            cartItems = new ObservableCollection<Order_Detail>();
            OrderDetailGrid.ItemsSource = cartItems;

            // 2. Tải danh sách người dùng
            LoadUsers();

            // 3. Thiết lập ngày mặc định
            StartDatePicker.SelectedDate = DateTime.Now;
            EndDatePicker.SelectedDate = DateTime.Now.AddDays(14); // Mặc định cho mượn 2 tuần

            // 4. Gán các sự kiện
            SearchBookTextBox.TextChanged += SearchBookTextBox_TextChanged;
            AddBookButton.Click += AddBookButton_Click;
            ConfirmButton.Click += ConfirmButton_Click;
            CancelButton.Click += CancelButton_Click;
            // Gán sự kiện cho các nút Xóa trong DataGrid
            OrderDetailGrid.LoadingRow += (s, args) =>
            {
                var deleteButton = FindVisualChild<Button>(args.Row);
                if (deleteButton != null)
                {
                    deleteButton.Click += DeleteBookButton_Click;
                }
            };
        }

        // Tải danh sách độc giả vào ComboBox
        private void LoadUsers()
        {
            try
            {
                UserComboBox.ItemsSource = db.Users.ToList();
                UserComboBox.DisplayMemberPath = "fullname";
                UserComboBox.SelectedValuePath = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách độc giả: " + ex.Message);
            }
        }

        // Tìm kiếm sách khi người dùng gõ chữ
        private void SearchBookTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBookTextBox.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                BookSearchResultsListView.ItemsSource = null;
                return;
            }
            try
            {
                var results = db.Books
                                .Where(b => b.name.ToLower().Contains(searchText) && b.quanlity > 0)
                                .ToList();
                BookSearchResultsListView.ItemsSource = results;
                BookSearchResultsListView.DisplayMemberPath = "name"; // Hiển thị tên sách
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm sách: " + ex.Message);
            }
        }

        // Thêm sách được chọn vào giỏ hàng (OrderDetailGrid)
        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookSearchResultsListView.SelectedItem is Book selectedBook)
            {
                // Kiểm tra sách đã có trong giỏ chưa
                var existingItem = cartItems.FirstOrDefault(item => item.book_id == selectedBook.Id);

                if (existingItem != null)
                {
                    // Nếu có, tăng số lượng (chuyển đổi từ string sang int và ngược lại)
                    int currentQuantity = int.Parse(existingItem.quanlity);
                    existingItem.quanlity = (currentQuantity + 1).ToString();
                }
                else
                {
                    // Nếu chưa, thêm mới vào giỏ
                    var newItem = new Order_Detail
                    {
                        book_id = selectedBook.Id,
                        Book = selectedBook, // Gán đối tượng sách để binding hoạt động
                        quanlity = "1"
                    };
                    cartItems.Add(newItem);
                }
                // Cập nhật lại giao diện DataGrid
                OrderDetailGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một cuốn sách để thêm.");
            }
        }

        // Xóa sách khỏi giỏ hàng
        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.CommandParameter != null)
            {
                int bookIdToRemove = (int)deleteButton.CommandParameter;
                var itemToRemove = cartItems.FirstOrDefault(item => item.book_id == bookIdToRemove);
                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                }
            }
        }

        // Xác nhận thuê và lưu vào CSDL
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // --- Validation ---
            if (UserComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và kết thúc.", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (EndDatePicker.SelectedDate.Value < StartDatePicker.SelectedDate.Value)
            {
                MessageBox.Show("Ngày kết thúc không thể trước ngày bắt đầu.", "Lỗi ngày", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!cartItems.Any())
            {
                MessageBox.Show("Giỏ hàng trống. Vui lòng thêm sách để thuê.", "Giỏ hàng trống", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- Lưu vào CSDL ---
            try
            {
                // 1. Tạo Order mới
                var newOrder = new Order
                {
                    user_id = (int)UserComboBox.SelectedValue,
                    start_date = StartDatePicker.SelectedDate.Value,
                    end_date = EndDatePicker.SelectedDate.Value,
                    total_book = cartItems.Sum(item => int.Parse(item.quanlity))
                };

                // 2. Thêm các Order_Detail từ giỏ hàng vào Order
                foreach (var cartItem in cartItems)
                {
                    // Quan trọng: không gán đối tượng Book vào đây vì nó đã tồn tại
                    // Chỉ cần gán book_id là đủ.
                    newOrder.Order_Detail.Add(new Order_Detail
                    {
                        book_id = cartItem.book_id,
                        quanlity = cartItem.quanlity
                    });
                }

                // 3. Thêm Order vào context và lưu
                db.Orders.Add(newOrder);
                db.SaveChanges(); // Lệnh này sẽ lưu Order và tất cả Order_Detail liên quan

                MessageBox.Show("Thuê sách thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi lưu đơn hàng: " + ex.Message + (ex.InnerException != null ? "\nInner: " + ex.InnerException.Message : ""), "Lỗi CSDL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Nút Hủy
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Hàm tiện ích để tìm control con trong một hàng của DataGrid
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
    }
}
