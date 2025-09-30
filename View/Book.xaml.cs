using LS.Model; // Đảm bảo bạn đã using Model
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation; // Thêm using cho Navigation

namespace LS.View
{
    public partial class Book : Page
    {
        // Khởi tạo ViewModel để quản lý logic
        private ViewModel.BookViewModel bvm = new ViewModel.BookViewModel();

        public Book()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Tải danh sách sách khi trang được mở
            bvm.LoadSach(dgBooks);
            MessageBox.Show("Dữ liệu sách đã được tải (giả lập)."); // Dùng để kiểm tra
        }

        private void BtnAddNew_Click(object sender, RoutedEventArgs e)
        {
            // Sử dụng NavigationService để chuyển đến trang AddBook
            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new AddBook());
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Lấy sách được chọn từ DataContext của button
            if (sender is Button btn && btn.DataContext is Model.Book selectedBook)
            {
                // Chuyển đến trang EditBook và truyền đối tượng sách qua constructor
                if (this.NavigationService != null)
                {
                    this.NavigationService.Navigate(new EditBook(selectedBook));
                }
            }
            else
            {
                MessageBox.Show("Không thể lấy thông tin sách để chỉnh sửa.");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Model.Book selectedBook)
            {
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sách: '{selectedBook.name}'?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Gọi phương thức xóa từ ViewModel
                    bvm.XoaSach(selectedBook);
                    bvm.LoadSach(dgBooks);
                    MessageBox.Show("Đã xóa sách thành công (giả lập).");
                }
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Gọi phương thức tìm kiếm từ ViewModel
            bvm.TimKiem(dgBooks, txtSearch.Text);
            MessageBox.Show($"Đang tìm kiếm với từ khóa: '{txtSearch.Text}' (giả lập).");
        }
    }
}