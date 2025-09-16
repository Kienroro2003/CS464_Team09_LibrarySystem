using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LS
{
    public partial class UserWindow : Window
    {
        Database1Entities db = new Database1Entities();

        // Dùng ObservableCollection để UI tự động cập nhật khi thay đổi
        public ObservableCollection<User> Users { get; set; }

        public UserWindow()
        {
            InitializeComponent();

            // Gắn DataContext cho Window để Binding trong XAML hoạt động
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        public void LoadUsers()
        {
            Users = new ObservableCollection<User>(db.Users.ToList());
            DataContext = this; // refresh lại Binding
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            Form_User form = new Form_User();
            form.ShowDialog();
            LoadUsers();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is User selectedUser)
            {
                var user = db.Users.Find(selectedUser.Id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    Users.Remove(selectedUser); // cập nhật ngay UI
                }
            }
        }
    }
}
