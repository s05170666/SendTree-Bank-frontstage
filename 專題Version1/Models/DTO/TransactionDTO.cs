using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 專題Version1.Models.DTO
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public int AccountID { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public string Description { get; set; }
        public string InteractiveAccountNumber { get; set; }
    }
}