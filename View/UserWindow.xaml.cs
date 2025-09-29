using LS.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls; // Thêm using này

namespace LS.View
{
    public partial class UserWindow : Page, INotifyPropertyChanged // Thay đổi ở đây
    {
        Database1Entities1 db = new Database1Entities1();

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public UserWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        public void LoadUsers()
        {
            using (var newDb = new Database1Entities1())
            {
                Users = new ObservableCollection<User>(newDb.Users.ToList());
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Form_Search_user form = new Form_Search_user();
            if (form.ShowDialog() == true)
            {
                Users = new ObservableCollection<User>(form.SearchResult);
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            Form_User form = new Form_User();
            form.ShowDialog();
            LoadUsers();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is User selectedUser)
            {
                Form_User_Update form = new Form_User_Update();
                form.LoadUser(selectedUser);

                if (form.ShowDialog() == true)
                {
                    LoadUsers();
                }
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is User selectedUser)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa người dùng '{selectedUser.username}' không?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var userVM = new ViewModel.UserViewModel();
                        userVM.DeleteUser(selectedUser);
                        Users.Remove(selectedUser);
                        MessageBox.Show("Đã xóa người dùng thành công!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa người dùng: " + ex.Message);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}