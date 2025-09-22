using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace LS
{
    public partial class UserWindow : Window, INotifyPropertyChanged
    {
        Database1Entities1 db = new Database1Entities1();

        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(); // báo cho UI biết thuộc tính đã đổi
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
            Users = new ObservableCollection<User>(db.Users.ToList());
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
                form.ShowDialog();
                // reload danh sách sau khi update

            }
            

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
                    Users.Remove(selectedUser);
                }
            }
        }

        // Triển khai INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Form_Search_user form = new Form_Search_user();
            form.ShowDialog();
            LoadUsers();
        }
    }
}
