using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        public AddBook()
        {
            InitializeComponent();
        }
        private string selectedImagePath;
        ViewModel.BookViewModel bvm = new ViewModel.BookViewModel();

        private void BtnThemSach_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTenSach.Text) || string.IsNullOrEmpty(txtTacGia.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Model.Book newBook = new Model.Book
                {
                    name = txtTenSach.Text,
                    author = txtTacGia.Text,
                    publisher = txtNhaXuatBan.Text,
                    category = txtTheLoai.Text,
                    page = string.IsNullOrEmpty(txtSoTrang.Text) ? 0 : int.Parse(txtSoTrang.Text),
                    quanlity = string.IsNullOrEmpty(txtSoLuong.Text) ? 0 : int.Parse(txtSoLuong.Text),
                    url_image = selectedImagePath 
                };

                bvm.ThemSach(newBook);
                Close(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void BtnHuyThemMoi_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult rs = MessageBox.Show("Bạn chắc chắn muốn thoát!", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rs == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}
