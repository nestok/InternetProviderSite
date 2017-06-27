using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperInternet.Models
{
    public class PasswordChangeEntity
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassword { get; set; }
    }
}