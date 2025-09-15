using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace CS464_Team09_LibrarySystem
{
    public partial class UserWindow : Window
    {
        public ObservableCollection<UserModel> Users { get; set; }

        public UserWindow()
        {
            InitializeComponent();

            Users = new ObservableCollection<UserModel>
            {
                new UserModel { username="nva", password="123", fullname="Nguyen Van A", gender="Male", address="Hanoi", phone="1234567890"},
                new UserModel { username="nbb", password="123", fullname="Nguyen Van B", gender="Female", address="HCM", phone="0987654321"}
            };

            DataContext = this;
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is UserModel selectedUser)
            {
                Users.Remove(selectedUser);
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            Form_User form = new Form_User(); // Mở cửa sổ thêm user
            form.ShowDialog();
        }
    }

    public class UserModel
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string gender { get; set; }
        public DateTime? birthday { get; set; }
        public string address { get; set; }
        public int? role_id { get; set; }
        public string phone { get; set; }

        public virtual Role Role { get; set; }
    }
}
