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

namespace LS.View
{
    /// <summary>
    /// Interaction logic for Book.xaml
    /// </summary>
    public partial class Book : Window
    {
        ViewModel.BookViewModel bvm = new ViewModel.BookViewModel();
        public Book()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bvm.LoadSach(dgBooks);
        }

        private void BtnAddNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddBook addBookView = new AddBook();
                addBookView.Closed += (s, args) => bvm.LoadSach(dgBooks); 
                addBookView.ShowDialog();
                bvm.LoadSach(dgBooks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở cửa sổ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            Model.Book selectedBook = btn.DataContext as Model.Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Không tìm thấy dữ liệu sách!");
                return;
            }

            // Mở window UpdateBook và truyền dữ liệu sách
            EditBook editBook = new EditBook(selectedBook);
            editBook.ShowDialog();
            bvm.LoadSach(dgBooks);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Model.Book selectedBook = btn.DataContext as Model.Book;
            if (selectedBook != null)
            {
                MessageBoxResult rs = MessageBox.Show($"Xóa sách : {selectedBook.name}", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (rs == MessageBoxResult.Yes)
                {
                    bvm.XoaSach(selectedBook);
                }
                    bvm.LoadSach(dgBooks);
            }
        }

        private void DgBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
