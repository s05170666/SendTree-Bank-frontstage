using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 專題Version1.Models.DTO;
using 專題Version1.Models.ViewModels;

namespace 專題Version1.Models.ApiViewModels
{
    public class AccountDetailsDTO
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public List<TransactionDTO> Transactions { get; set; }

    }
}