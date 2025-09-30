using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LS.ViewModel
{
    class BookViewModel
    {
        Model.Database1Entities1 DB = new Model.Database1Entities1();

        public void LoadSach(DataGrid dg)
        {
            try
            {
                dg.ItemsSource = null;
                dg.ItemsSource = DB.Books.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ThemSach(Model.Book book)
        {
            try
            {
                DB.Books.Add(book);
                DB.SaveChanges();
                MessageBox.Show("Thêm sách mới thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void CapNhatSach(Model.Book book)
        {
            try
            {
                Model.Book b = DB.Books.Find(book.Id); 
                if (b != null)
                {
                    b.name = book.name;
                    b.author = book.author;
                    b.publisher = book.publisher;
                    b.category = book.category;
                    b.page = book.page;
                    b.quanlity = book.quanlity; 
                    b.url_image = book.url_image;
                    DB.SaveChanges();
                    MessageBox.Show("Cập nhật sách thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Không tồn tại sách cần cập nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void XoaSach(Model.Book book)
        {
            try
            {
                Model.Book b = DB.Books.Find(book.Id); 
                if (b != null)
                {
                    var borrowRecords = DB.Order_Detail.Where(x => x.book_id == book.Id).ToList();
                    DB.Order_Detail.RemoveRange(borrowRecords);
                    DB.Books.Remove(b);
                    DB.SaveChanges();
                    MessageBox.Show("Xóa sách thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Không tồn tại sách cần xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa sách: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void TimKiem(DataGrid dg, string tuKhoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tuKhoa))
                {
                    LoadSach(dg);
                    return;
                }

                string keyword = tuKhoa.ToLower().Trim();
                var ketQua = DB.Books.Where(x =>
                    (x.name != null && x.name.ToLower().Contains(keyword)) ||
                    (x.author != null && x.author.ToLower().Contains(keyword))
                ).ToList();

                dg.ItemsSource = null;
                dg.ItemsSource = ketQua;

                if (ketQua.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sách phù hợp", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
