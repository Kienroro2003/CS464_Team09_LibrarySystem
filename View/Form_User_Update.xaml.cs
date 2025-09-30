using LS.Model;
using LS.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace LS
{
    public partial class Form_User_Update : Window
    {
        private User _editingUser;
        private UserViewModel _userVM = new UserViewModel();

        public Form_User_Update()
        {
            InitializeComponent();
        }

        public void LoadUser(User user)
        {
            _editingUser = user; // giữ object gốc để truyền cho ViewModel khi cập nhật
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
                roleId = roleName == "Admin" ? 1 : 2;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn vai trò!");
                return;
            }

            // Gán dữ liệu mới vào object _editingUser
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
                _userVM.UpdateUser(_editingUser); // Gọi ViewModel xử lý
                MessageBox.Show("Cập nhật người dùng thành công!");
                this.DialogResult = true; // thông báo cho UserWindow reload
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
