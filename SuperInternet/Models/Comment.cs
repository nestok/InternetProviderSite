using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperInternet.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Text { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }   
    }
}