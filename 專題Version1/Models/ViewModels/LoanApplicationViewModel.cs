using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 專題Version1.Models.ViewModels
{
    public class LoanApplicationViewModel
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int LoanProductID { get; set; }
        public string ProductName { get; set; }
        public decimal LoanAmount { get; set; }
        public HttpPostedFileBase EconomicProof { get; set; }
        public string EconomicProofPath { get; set; }
        public string DisbursementAccount { get; set; }
        public int LoanTermMonths { get; set; }
        public decimal InteresRate { get; set; }
        public decimal OccupationGroup { get; set; }
        public decimal IncomeRange { get; set; }
        public string SelectedIncomeRange { get; set; }
        public decimal BaseRate { get; set; }
    }
}