using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 專題Version1.Models;
using System.Threading.Tasks;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;


namespace 專題Version1.Services
{
    public class AccountServices
    {
        private Version3_CustomerEntities _db = new Version3_CustomerEntities();

        public async Task<Account> GetAccountInfoByAccountNumber(int accountnumber)
        {
            var account = await _db.Accounts
                .Include(a => a.AccountType)
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountnumber.ToString());

            return account;
        }

        public async Task<List<Account>> GetAccountInfoByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }
            var accounts = await _db.Customers
                .Where(c => c.Email == email)
                .SelectMany(c => c.Accounts)
                .Include(a => a.AccountType)
                .ToListAsync();

            return accounts;
        }
    }
}