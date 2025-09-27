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
            Model.User user = db.Users.Find(userDele.Id);
            if(user != null)
            {
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
