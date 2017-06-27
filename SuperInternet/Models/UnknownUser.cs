using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace SuperInternet.Models
{
    public class UnknownUser
    {
        public string Nickname { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        public User ToUser()
        {
            User user = new User();
            user.Nickname = Nickname;
            user.Role = Role;
            user.Email = Email;
            user.Image = "Avatar.jpg";

            byte[] pas = Encoding.Unicode.GetBytes(Password);
            SHA1 sha = new SHA1CryptoServiceProvider();
            user.Password = sha.ComputeHash(pas);

            return user;
        }
    }
}