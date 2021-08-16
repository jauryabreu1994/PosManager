using PosLibrary.Model.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosManager.Controller
{
    public class LoginController
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


    }
}
