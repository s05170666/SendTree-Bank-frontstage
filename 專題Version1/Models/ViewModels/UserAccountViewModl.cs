using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 專題Version1.Models.ViewModels
{
    public class UserAccountViewModl
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public List<string> AccountNumber { get; set; }
        public List<string> AccountType { get; set; }
        public List<decimal> Balance { get; set; }

    }
}