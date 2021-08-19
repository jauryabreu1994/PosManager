using PosLibrary.Model.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PosManager.Controller
{
    public class GenericController
    {
        public static User userLogin = null;

        protected internal void SetCurrentUser(User user) 
        {
            userLogin = user;
        }
        protected internal User GetCurrentUser()
        {
            return userLogin;
        }
        protected internal bool ValidateAccess(string permission)
        {
            if (userLogin.UserGroup.GroupPermissions.
                        Any(a => a.PermissionCode == permission &&
                                 a.Condition_Status && 
                                 !a.Deleted))
            {
                return true;
            }
            else
                MessageBox.Show("No posee permiso para esta opcion.");

            return false;
        }

        protected internal bool MessageError(string msg)
        {
            MessageBox.Show(msg, "error", MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
            return false;
        }
    }
}
