using LS.Model;
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

namespace LS
{
    public partial class Form_Search_user : Window
    {
        Database1Entities1 db = new Database1Entities1();

        public List<User> SearchResult { get; private set; } // ✅ trả về danh sách cho UserWindow

        public Form_Search_user()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            // Bắt đầu với toàn bộ danh sách Users
            var query = db.Users.AsQueryable();

            // Lọc từng điều kiện nếu có nhập
            if (!string.IsNullOrWhiteSpace(txt_userid.Text))
            {
                if (int.TryParse(txt_userid.Text, out int id))
                    query = query.Where(u => u.Id == id);
            }

            if (!string.IsNullOrWhiteSpace(txt_username.Text))
            {
                string keyword = txt_username.Text.Trim();
                query = query.Where(u => u.username.Contains(keyword));
            }

            if (!string.IsNullOrWhiteSpace(txt_pass.Password))
            {
                string pass = txt_pass.Password.Trim();
                query = query.Where(u => u.password.Contains(pass));
            }

            if (!string.IsNullOrWhiteSpace(txt_fullname.Text))
            {
                string name = txt_fullname.Text.Trim();
                query = query.Where(u => u.fullname.Contains(name));
            }

            if (rb_male.IsChecked == true)
            {
                query = query.Where(u => u.gender == "Male");
            }
            else if (rb_female.IsChecked == true)
            {
                query = query.Where(u => u.gender == "Female");
            }

            if (txt_birth.SelectedDate.HasValue)
            {
                DateTime selectedDate = txt_birth.SelectedDate.Value.Date;
                query = query.Where(u => u.birthday == selectedDate);
            }

            if (!string.IsNullOrWhiteSpace(txt_addess.Text))
            {
                string addr = txt_addess.Text.Trim();
                query = query.Where(u => u.address.Contains(addr));
            }

            if (!string.IsNullOrWhiteSpace(txt_phone.Text))
            {
                string phone = txt_phone.Text.Trim();
                query = query.Where(u => u.phone.Contains(phone));
            }

            // Lọc theo Role nếu có chọn
            if (cb_role.SelectedItem is ComboBoxItem selectedRole)
            {
                string roleName = selectedRole.Content.ToString();
                if (roleName == "Admin")
                    query = query.Where(u => u.role_id == 1);
                else if (roleName == "User")
                    query = query.Where(u => u.role_id == 2);
            }

            // Lấy kết quả
            SearchResult = query.ToList();

            // Nếu không tìm thấy, báo cho người dùng
            if (SearchResult.Count == 0)
            {
                MessageBox.Show("Không tìm thấy người dùng phù hợp!");
                return;
            }

            this.DialogResult = true; // để UserWindow biết đã tìm xong
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
