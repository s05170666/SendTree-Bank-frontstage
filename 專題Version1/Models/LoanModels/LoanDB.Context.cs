﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace 專題Version1.Models.LoanModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Version3_LoanEntities1 : DbContext
    {
        public Version3_LoanEntities1()
            : base("name=Version3_LoanEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CustomersInLoan> CustomersInLoans { get; set; }
        public virtual DbSet<IncomeRangeRate> IncomeRangeRates { get; set; }
        public virtual DbSet<LoanApplication> LoanApplications { get; set; }
        public virtual DbSet<LoanProduct> LoanProducts { get; set; }
        public virtual DbSet<OccupationGroupRate> OccupationGroupRates { get; set; }
        public virtual DbSet<RepaymentAccount> RepaymentAccounts { get; set; }
        public virtual DbSet<RepaymentSchedule> RepaymentSchedules { get; set; }
    }
}
