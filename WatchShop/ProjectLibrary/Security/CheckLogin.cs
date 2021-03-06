using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibrary.Security
{
    public class CheckLogin
    {
        public static int CheckUserLogin(string username, string password)
        {
            using (var db = new MyDbDataContext())
            {
                CurrentSession.ClearAll();
                string pashPassWord = Password.HashPassword(password);
                User checkUser = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == pashPassWord);
                if (checkUser != null)
                {
                    if (checkUser.Status)
                    {
                        //Đăng nhập thành công
                        return 1;
                    }
                    return 2;
                }
            }
            return 3;
        }
    }
}
