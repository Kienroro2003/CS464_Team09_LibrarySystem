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

namespace LS
{
    /// <summary>
    /// Interaction logic for Form_User.xaml
    /// </summary>
    public partial class Form_User : Window
    {
        Database1Entities db = new Database1Entities();

        private ObservableCollection<User> _users;


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

            if (string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Vui lòng chọn giới tính!");
                return;
            }
            DateTime? selectedDate = txt_birth.SelectedDate;
            if (selectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh!");
                return;
            }
            int roleId = 0;

            if (cb_role.SelectedItem is ComboBoxItem selectedRole)
            {
                string roleName = selectedRole.Content.ToString();

                if (roleName == "Admin")
                    roleId = 1;
                else if (roleName == "User")
                    roleId = 2;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vai trò!");
                return;
            }
            User user = new User
            {
                username = txt_username.Text.Trim(),
                password = txt_pass.Password.Trim(),
                fullname = txt_fullname.Text.Trim(),
                gender = gender,
                birthday = selectedDate.Value,
                address = txt_addess.Text.Trim(),
                phone = txt_phone.Text.Trim(),
                role_id = roleId,
            };
            db.Users.Add(user);
            db.SaveChanges();
            MessageBox.Show("Thêm người dùng thành công thành công");
            Form_User form = new Form_User();

        }
    }
}
