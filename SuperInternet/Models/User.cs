using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SuperInternet.Models
{
    public enum UserRole
    {
        CLIENT = 0,
        ADMIN = 1
    };

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sername { get; set; }
        public string Patronymic { get; set; }
        public UserRole Role { get; set; }
        public string Nickname { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Tarif { get; set; }
        public byte[] Password { get; set; }
    }
}