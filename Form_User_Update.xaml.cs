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
    public partial class Form_User_Update : Window
    {
        Database1Entities1 db = new Database1Entities1();
        private User _editingUser;
        private ObservableCollection<User> _users;


        public Form_User_Update()
        {
            InitializeComponent();
        }

        public void LoadUser(User user)
        {
            _editingUser = db.Users.Find(user.Id); // lấy lại từ db (đảm bảo tracking)
            if (_editingUser != null)
            {
                txt_username.Text = _editingUser.username;
                txt_pass.Password = _editingUser.password;
                txt_fullname.Text = _editingUser.fullname;
                txt_addess.Text = _editingUser.address;
                txt_phone.Text = _editingUser.phone;

                if (_editingUser.gender == "Male")
                    rb_male.IsChecked = true;
                else
                    rb_female.IsChecked = true;

                txt_birth.SelectedDate = _editingUser.birthday;
                cb_role.SelectedIndex = _editingUser.role_id == 1 ? 1 : 0;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_editingUser == null)
            {
                MessageBox.Show("Không tìm thấy người dùng để cập nhật!");
                return;
            }

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

            // ✅ Cập nhật thông tin cho user đang được sửa
            _editingUser.username = txt_username.Text.Trim();
            _editingUser.password = txt_pass.Password.Trim();
            _editingUser.fullname = txt_fullname.Text.Trim();
            _editingUser.gender = gender;
            _editingUser.birthday = selectedDate.Value;
            _editingUser.address = txt_addess.Text.Trim();
            _editingUser.phone = txt_phone.Text.Trim();
            _editingUser.role_id = roleId;

            try
            {
                db.SaveChanges();
                MessageBox.Show("Cập nhật người dùng thành công!");
                this.DialogResult = true; // để UserWindow biết có thay đổi
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

    }
}
