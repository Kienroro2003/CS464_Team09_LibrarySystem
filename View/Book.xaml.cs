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
                View.AddBook addBookView = new View.AddBook();
                addBookView.Closed += (s, args) => bvm.LoadSach(dgBooks); 
                addBookView.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi mở cửa sổ: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
