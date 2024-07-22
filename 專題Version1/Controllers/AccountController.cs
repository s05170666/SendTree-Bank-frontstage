using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using 專題Version1.Models;
using 專題Version1.Services;

namespace 專題Version1.Controllers
{
    public class AccountController : Controller
    {
        private Version3_CustomerEntities _db = new Version3_CustomerEntities();
        private AccountServices _accountServices = new AccountServices();

        // GET: Account

        // 查詢帳戶詳情
        public async Task<ActionResult> AccountDetails(int accountNumber, DateTime? startDate = null, DateTime? endDate = null)
        {
            // 若未指定開始日期，則設為當月第一天
            startDate = startDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // 若未指定結束日期，則設為今天
            endDate = endDate ?? DateTime.Now;

            // 透過 accountNumber 獲取帳戶資訊
            var account = await _accountServices.GetAccountInfoByAccountNumber(accountNumber);
            if (account != null)
            {
                // 按日期分組交易並按交易類型再次分組
                var groupedTransactions = account.Transactions
                    .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                    .GroupBy(t => t.TransactionDate.Date)
                    .Select(group => new
                    {
                        Date = group.Key,
                        Transactions = group.GroupBy(t => t.TransactionType) // 按交易類型再次分組
                             .Select(g => new
                             {
                                 TransactionType = g.Key, // 交易類型
                                 TotalAmount = g.Sum(t => t.Amount) // 該交易類型總金額
                             })
                             .ToDictionary(t => t.TransactionType, t => t.TotalAmount)
                    });

                // 計算指定日期範圍內的總交易數量
                var totalTransactionsCount = account.Transactions.Count(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate);

                // 計算指定日期範圍內的轉出交易數量
                var outgoingTransactionsCount = account.Transactions
                    .Count(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate && t.TransactionType == "Debit");

                // 計算指定日期範圍內的轉入交易數量
                var incomingTransactionsCount = account.Transactions
                    .Count(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate && t.TransactionType == "Credit");

                // 計算指定日期範圍內的總交易金額
                var totalAmount = account.Transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).Sum(t => t.Amount);

                // 計算指定日期範圍內的轉出總金額
                var outgoingAmount = account.Transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate && t.TransactionType == "Debit").Sum(t => t.Amount);

                // 計算指定日期範圍內的轉入總金額
                var incomingAmount = account.Transactions.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate && t.TransactionType == "Credit").Sum(t => t.Amount);

                //  JSON 返回
                var data = new
                {
                    GroupedTransactions = groupedTransactions,
                    TotalTransactionsCount = totalTransactionsCount,
                    OutgoingTransactionsCount = outgoingTransactionsCount,
                    IncomingTransactionsCount = incomingTransactionsCount,
                    TotalTransactionsAmount = totalAmount.ToString("N0"),
                    OutgoingTransactionsAmount = outgoingAmount.ToString("N0"),
                    IncomingTransactionsAmount = incomingAmount.ToString("N0")
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }

            // 若未找到帳戶資訊，返回 View
            return View();
        }

        // 查詢所有帳戶的總金額
        public async Task<ActionResult> TotalAmount(string email)
        {
            // 透過 email 獲取所有帳戶資訊
            var accounts = await _accountServices.GetAccountInfoByEmail(email);
            if (accounts != null)
            {
                // 計算所有帳戶的總餘額
                var totalAmount = accounts.Sum(a => a.Balance);

                //  JSON 返回
                var result = new
                {
                    Accounts = accounts.Select(a => new
                    {
                        AccountNumber = a.AccountNumber,
                        AccountType = a.AccountType.AccountTypeName,
                        Balance = a.Balance
                    }),
                    TotalAmount = totalAmount
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            // 若未找到帳戶資訊，返回 View
            return View();
        }
    }
}
