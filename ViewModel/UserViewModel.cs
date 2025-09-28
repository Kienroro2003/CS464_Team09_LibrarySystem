using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LS.ViewModel
{
    class UserViewModel
    {
        Model.Database1Entities1 db = new Model.Database1Entities1();
        public void AddUser(Model.User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }

        public void DeleteUser(Model.User userDele)
        {
            var user = db.Users.Find(userDele.Id);
            if (user != null)
            {
                // Lấy tất cả các đơn hàng của user
                var orders = db.Orders.Where(o => o.user_id == user.Id).ToList();

                foreach (var order in orders)
                {
                    // Xóa chi tiết từng đơn hàng trước
                    var orderDetails = db.Order_Detail.Where(d => d.order_id == order.Id).ToList();
                    db.Order_Detail.RemoveRange(orderDetails);
                }

                // Sau đó xóa các đơn hàng
                db.Orders.RemoveRange(orders);

                // Cuối cùng xóa user
                db.Users.Remove(user);

                db.SaveChanges();
            }
        }



        public void UpdateUser(Model.User updateUser)
        {
            Model.User user = db.Users.Find(updateUser.Id);
            if (user != null)
            {
                user.username = updateUser.username;
                user.password = updateUser.password;
                user.fullname = updateUser.fullname;
                user.gender = updateUser.gender;
                user.birthday = updateUser.birthday;
                user.address = updateUser.address;
                user.phone = updateUser.phone;
                user.role_id = updateUser.role_id;
                db.SaveChanges();
            }
        }

        public void LoadUser(DataGrid dg)
        {
            dg.ItemsSource = db.Users.ToList();
        }

        public void LoadRole(ComboBox cb)
        {
            cb.ItemsSource = db.Roles.ToList();
            cb.DisplayMemberPath = "name";
            cb.SelectedValuePath = "Id";
        }
    }
}
