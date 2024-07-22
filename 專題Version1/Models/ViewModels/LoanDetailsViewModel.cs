using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 專題Version1.Models.LoanModels;

namespace 專題Version1.Models.ViewModels
{
    public class LoanDetailsViewModel
    {
        public int CustomerID { get; set; }
        public int LoanApplicationID { get; set; }
        public string ProductName { get; set; }
        public decimal LoanAmount { get; set; }
        public string EconomicProof { get; set; }
        public string DisbursementAccount { get; set; }
        public int LoanTermMonths { get; set; }
        public decimal InteresRate { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
        public List<RepaymentSchedule> RepaymentSchedules { get; set; }
    }
}