using LS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Form_User.xaml
    /// </summary>
    public partial class Form_User : Window
    {
        Database1Entities1 db = new Database1Entities1();
        private User _editingUser;
        private ObservableCollection<User> _users;

        ViewModel.UserViewModel umd = new ViewModel.UserViewModel();
       
        public Form_User()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string gender = null;
            if (rb_male.IsChecked == true)
                gender = rb_male.Content.ToString();
            else if (rb_female.IsChecked == true)
                gender = rb_female.Content.ToString();

            int roleId = 0;
            if (cb_role.SelectedItem is ComboBoxItem selectedRole)
            {
                string roleName = selectedRole.Content.ToString();
                roleId = roleName == "Admin" ? 1 : 2; // 1 = Admin, 2 = User
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vai trò!");
                return;
            }

            Model.User user = new Model.User
            {
                username = txt_username.Text.Trim(),
                password = txt_pass.Password.Trim(),
                fullname = txt_fullname.Text.Trim(),
                gender = gender,
                birthday = txt_birth.SelectedDate,
                address = txt_addess.Text.Trim(),
                phone = txt_phone.Text.Trim(),
                role_id = roleId
            };
            umd.AddUser(user);
            MessageBox.Show("Thêm người dùng thành công thành công");
            this.DialogResult = true; // Quan trọng
            this.Close();

        }
    }
}
