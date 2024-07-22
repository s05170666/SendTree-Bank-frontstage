using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace 專題Version1.Models.ViewModels
{
    public class TransferViewModel
    {
        [Required]
        public string FromAccount { get; set; }
        [Required(ErrorMessage = "請輸入收款帳號")]
        public string ToAccount { get; set; }
        [Required(ErrorMessage = "請輸入金額")]
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string AccountType { get; set; } // 新增的属性

    }
}