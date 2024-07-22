using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 專題Version1.Models.ViewModels
{
    public class TransactionDetailsViewModel
    {
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal FromAccountBalanceBeforeTransaction { get; set; }
        public decimal FromAccountBalanceAfterTransaction { get; set; }


    }
}