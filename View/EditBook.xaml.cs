using Microsoft.Win32;
using System;
using System.IO;
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

namespace LS.View
{
    /// <summary>
    /// Interaction logic for EditBook.xaml
    /// </summary>
    public partial class EditBook : Window
    {
        private Model.Book _book; // Lưu dữ liệu sách
        private string selectedImagePath;
        ViewModel.BookViewModel bvm = new ViewModel.BookViewModel();
        public EditBook(Model.Book book)
        {
            InitializeComponent();
            _book = book;

            // Set dữ liệu vào TextBox
            txtTenSach.Text = book.name;
            txtTacGia.Text = book.author;
            txtNhaXuatBan.Text = book.publisher;
            txtTheLoai.Text = book.category;
            txtSoTrang.Text = book.page.ToString();
            txtSoLuong.Text = book.quanlity.ToString();
            txtHinhAnh.Text = book.url_image;
            selectedImagePath = book.url_image; 
        }

        private void BtnCapNhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txtTenSach.Text) ||
                    string.IsNullOrWhiteSpace(txtTacGia.Text) ||
                    string.IsNullOrWhiteSpace(txtNhaXuatBan.Text) ||
                    string.IsNullOrWhiteSpace(txtTheLoai.Text))
                {
                    MessageBox.Show("Vui lòng điền đầy đủ các trường bắt buộc!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtSoTrang.Text, out int page) || page <= 0)
                {
                    MessageBox.Show("Số trang phải là số nguyên dương!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtSoLuong.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Số lượng phải là số không âm!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Cập nhật dữ liệu cho _book
                _book.name = txtTenSach.Text;
                _book.author = txtTacGia.Text;
                _book.publisher = txtNhaXuatBan.Text;
                _book.category = txtTheLoai.Text;
                _book.page = page;
                _book.quanlity = quantity;
                _book.url_image = selectedImagePath ?? _book.url_image; 

                // Gọi hàm cập nhật từ ViewModel
                bvm.CapNhatSach(_book);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnChonHinh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*"
                };
                if (OpenFileDialog.ShowDialog() == true)
                {
                    selectedImagePath = OpenFileDialog.FileName;
                    txtHinhAnh.Text = System.IO.Path.GetFileName(selectedImagePath);

                    string imagesDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);

                    string destFile = System.IO.Path.Combine(imagesDir, System.IO.Path.GetFileName(selectedImagePath));
                    File.Copy(selectedImagePath, destFile, true);
                    selectedImagePath = $"Images/{System.IO.Path.GetFileName(selectedImagePath)}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi chọn hình: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnHuy_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Bạn chắc chắn muốn thoát?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                Close(); 
            }
        }
    }
}
