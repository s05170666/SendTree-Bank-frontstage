using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using 專題Version1.Models;
using 專題Version1.Models.ApiViewModels;
using 專題Version1.Models.DTO;
using 專題Version1.Services;

namespace 專題Version1.Controllers.APIControllers
{
    [RoutePrefix("TransactionAPI")]
    public class TransactionAPIController : ApiController
    {
        private Version3_CustomerEntities _db = new Version3_CustomerEntities();
        private AccountServices _accountServices = new AccountServices();//負責處理帳戶相關的服務


        [Route("GetAccountInfoByUserId")]
        public async Task<IHttpActionResult> GetAccountInfoByUserId(int userId)
        {
            var account = await _db.Customers
                .Where(c => c.CustomerID == userId)
                .SelectMany(c => c.Accounts)
                .Include(a => a.AccountType).ToListAsync();

            if (account == null)
            {
                return NotFound();
            }

            var result = account.Select(a => new AccountDetailsDTO
            {
                AccountNumber = a.AccountNumber,
                AccountType = a.AccountType.AccountTypeName,
                Balance = a.Balance
            }).ToList();

            return Ok(result);
        }


        [Route("GetAccountDetails")]
        public async Task<IHttpActionResult> GetAccountDetails(int accountNumber)
        {

            var account = await _accountServices.GetAccountInfoByAccountNumber(accountNumber);


            if (account == null)
            {
                return NotFound();
            }
            var accountDetails = new AccountDetailsDTO
            {
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType.AccountTypeName,
                Balance = account.Balance,
                Transactions = account.Transactions.
                                OrderByDescending(t => t.TransactionDate).
                                Take(5).
                                Select(t => new TransactionDTO
                                {
                                    TransactionID = t.TransactionID,
                                    AccountID = t.AccountID,
                                    TransactionDate = t.TransactionDate,
                                    TransactionType = t.TransactionType,
                                    Amount = t.Amount,
                                    BalanceAfterTransaction = t.BalanceAfterTransaction,
                                    Description = t.Description,
                                    InteractiveAccountNumber = t.InteractiveAccountNumber
                                }).ToList()

            };

            return Ok(accountDetails);
        }

        [Route("GetTransactions")]

        public async Task<IHttpActionResult> GetTransactions(int accountNumber, DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate = startDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            endDate = endDate ?? DateTime.Now; // 今天

            var account = await _accountServices.GetAccountInfoByAccountNumber(accountNumber);
            if (account == null)
            {
                return NotFound();
            }
            var accountDetails = new AccountDetailsDTO
            {
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType.AccountTypeName,
                Balance = account.Balance,
                Transactions = account.Transactions
                                .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                                .OrderBy(t => t.TransactionDate)
                                .Select(t => new TransactionDTO
                                {
                                    TransactionID = t.TransactionID,
                                    AccountID = t.AccountID,
                                    TransactionDate = t.TransactionDate,
                                    TransactionType = t.TransactionType,
                                    Amount = t.Amount,
                                    BalanceAfterTransaction = t.BalanceAfterTransaction,
                                    Description = t.Description,
                                    InteractiveAccountNumber = t.InteractiveAccountNumber
                                }).ToList()
            };
            return Ok(accountDetails);

        }
    }
}
