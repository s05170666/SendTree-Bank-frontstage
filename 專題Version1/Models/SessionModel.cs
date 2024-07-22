using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 專題Version1.Models
{
    public class SessionModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        
        public List<Account> Accounts { get; set; }

    }
}