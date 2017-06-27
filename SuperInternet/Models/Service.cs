using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperInternet.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Tarif { get; set; }
        public string ConnectionType { get; set; }
        public string Payment { get; set; }
        public string Speed { get; set; }
        public string Term { get; set; }
        public string Traffic { get; set; }
        public string SubscrCash { get; set; }
        public string Agreement { get; set; }
    }
}