using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using 專題Version1.Models;
using 專題Version1.Models.ViewModels;
using 專題Version1.Services;
using System.Data.Entity;
using 專題Version1.Models.LoanModels;
using System.Data.SqlClient;


namespace 專題Version1.Controllers
{
    [SessionCheckFilter]
    public class TransactionController : Controller
    {

        private Version3_CustomerEntities _db = new Version3_CustomerEntities();
        private Version3_LoanEntities1 _dbLoan = new Version3_LoanEntities1();
        private AccountServices _accountServices = new AccountServices();
        private Version3_LoanEntities3 _dbLoan2 = new Version3_LoanEntities3();
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Transfer()
        {
            var user = (SessionModel)Session["User"];
            var accounts = user.Accounts.Select(a => new
            {
                a.AccountNumber,
            }).ToList();

            ViewBag.Accounts = new SelectList(accounts, "AccountNumber", "AccountNumber");
            ViewBag.AccountTypes = new SelectList(new List<SelectListItem>
             {
                 new SelectListItem { Text = "貸款銀行", Value = "LoanBank" },
            new SelectListItem { Text = "本行", Value = "LocalBank" }
                 }, "Value", "Text");

            return View();
        }

        //轉帳
        [HttpPost]
        public ActionResult Transfer(TransferViewModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var fromAccount = _db.Accounts.FirstOrDefault(x => x.AccountNumber == model.FromAccount);

            if (model.AccountType == "LoanBank")
            {
                var toAccount = _dbLoan2.RepaymentAccounts.FirstOrDefault(x => x.AccountNumber == model.ToAccount);
                var toAccountId = _dbLoan2.RepaymentAccounts.FirstOrDefault(x => x.AccountNumber == model.ToAccount).RepaymentAccountID;
                if (fromAccount == null || toAccount == null)
                {
                    return RedirectToAction("Index", "Error", new { message = "查無此收款帳戶" });
                }

                if (fromAccount.Balance < model.Amount)
                {
                    ViewBag.Error = "餘額不足";
                    return View("Error");
                }
                
                using (var transactionScope = _db.Database.BeginTransaction())
                {
                    try
                    {
                        PerformDebitTransaction(fromAccount, model);
                        toAccount.AmountPaid += model.Amount;
                        var transactionLog = new TransactionLog
                        {
                            RepaymentAccountID = toAccountId,
                            TransactionDate = DateTime.Now,
                            TransactionType = "Debit",
                            Amount = model.Amount,
                            CreatedDate = DateTime.Now,

                        };
                        _dbLoan2.TransactionLogs.Add(transactionLog);
                        _dbLoan2.SaveChanges();
                        _db.SaveChanges();
                        transactionScope.Commit();

                        var transactionDetails = new TransactionDetailsViewModel
                        {
                            FromAccount = fromAccount.AccountNumber,
                            ToAccount = toAccount.AccountNumber,
                            Amount = model.Amount,
                            TransactionDate = DateTime.Now,
                            Description = model.Description,
                            FromAccountBalanceBeforeTransaction = fromAccount.Balance + model.Amount,
                            FromAccountBalanceAfterTransaction = fromAccount.Balance,
                        };

                        UpdateSession(fromAccount.CustomerID);
                        return RedirectToAction("TransferDetails", transactionDetails);
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Rollback();
                        ViewBag.Message = ex.Message;
                    }
                }
            }
            else if (model.AccountType == "LocalBank")
            {
                var toAccount = _db.Accounts.FirstOrDefault(x => x.AccountNumber == model.ToAccount);
                var toAccountId = _db.Accounts.FirstOrDefault(x => x.AccountNumber == model.ToAccount).AccountID;
                var fromAccountId = _db.Accounts.FirstOrDefault(x => x.AccountNumber == model.FromAccount).AccountID;

                if (fromAccount == null || toAccount == null)
                {
                    return RedirectToAction("Index", "Error", new { message = "查無此收款帳戶" });
                }

                if (fromAccount.Balance < model.Amount)
                {
                    ViewBag.Error = "Insufficient Balance";
                    return View("Error");
                }
                try
                {
                    //執行轉帳預存程序
                    var result = _db.Database.ExecuteSqlCommand(
                     "EXEC TransferFunds @FromAccountID, @ToAccountID, @Amount, @Description",
                        new SqlParameter("@FromAccountID", fromAccountId),
                        new SqlParameter("@ToAccountID", toAccountId),
                        new SqlParameter("@Amount", model.Amount),
                        new SqlParameter("@Description", model.Description ?? (object)DBNull.Value)
                        );


                    var transactionDetails = CreateTransactionDetails(fromAccount, toAccount, model);

                    UpdateSession(fromAccount.CustomerID);
                    return RedirectToAction("TransferDetails", transactionDetails);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }

            }

            return View(model);
        }

        
        private void PerformDebitTransaction(Account fromAccount, TransferViewModel model)
        {
            fromAccount.Balance -= model.Amount;

            var fromTransaction = new Transaction
            {
                AccountID = fromAccount.AccountID,
                TransactionDate = DateTime.Now,
                TransactionType = "Debit",
                Amount = model.Amount,
                BalanceAfterTransaction = fromAccount.Balance,
                Description = model.Description,
                InteractiveAccountNumber = model.ToAccount
            };

            _db.Transactions.Add(fromTransaction);
        }

        private void PerformCreditTransaction(Account toAccount, TransferViewModel model)
        {
            toAccount.Balance += model.Amount;

            var toTransaction = new Transaction
            {
                AccountID = toAccount.AccountID,
                TransactionDate = DateTime.Now,
                TransactionType = "Credit",
                Amount = model.Amount,
                BalanceAfterTransaction = toAccount.Balance,
                Description = model.Description,
                InteractiveAccountNumber = model.FromAccount
            };

            _db.Transactions.Add(toTransaction);
        }

        private TransactionDetailsViewModel CreateTransactionDetails(Account fromAccount, Account toAccount, TransferViewModel model)
        {
            return new TransactionDetailsViewModel
            {
                FromAccount = fromAccount.AccountNumber,
                ToAccount = toAccount.AccountNumber,
                Amount = model.Amount,
                TransactionDate = DateTime.Now,
                Description = model.Description,
                FromAccountBalanceBeforeTransaction = fromAccount.Balance,
                FromAccountBalanceAfterTransaction = fromAccount.Balance-model.Amount,
            };
        }

        private void UpdateSession(int customerId)
        {
            var user = _db.Customers.Include(c => c.Accounts)
                                    .Include(c => c.Accounts.Select(a => a.AccountType))
                                    .FirstOrDefault(x => x.CustomerID == customerId);

            var sessionModel = new SessionModel
            {
                UserId = user.CustomerID,
                FirstName = user.FirstName,
                Accounts = user.Accounts.Select(a => new Account
                {
                    AccountNumber = a.AccountNumber,
                    AccountType = new AccountType { AccountTypeName = a.AccountType.AccountTypeName },
                    Balance = a.Balance
                }).ToList()
            };

            Session["User"] = sessionModel;
        }


        public ActionResult TransferDetails(TransactionDetailsViewModel model)
        {
            // update session

            return View(model);
        }

        public ActionResult GetBalance(int accountnumber)
        {

            string accountnumberstring = accountnumber.ToString();

            var account = _db.Accounts.FirstOrDefault(x => x.AccountNumber == accountnumberstring);
            if (account == null)
            {
                return HttpNotFound();
            }
            return Json(account.Balance, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerifyPassword(string password)
        {
            var user = (SessionModel)Session["User"];
            if (user == null)
            {
                return Json(new { success = false });
            }
            var dbUser = _db.Customers.FirstOrDefault(x => x.CustomerID == user.UserId);
            string hashedPassword = LoginServices.EncryptPassword(password);

            if (hashedPassword == dbUser.Password)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        
        public ActionResult GetTransactions()
        {
            var user = (SessionModel)Session["User"];
            var accounts = user.Accounts.Select(a => new
            {
                a.AccountNumber,
            }).ToList();

            ViewBag.Accounts = new SelectList(accounts, "AccountNumber", "AccountNumber");

            return View();
        }

        //取得貸款還款紀錄
        public ActionResult TransForLoan()
        {
            int userId = ((SessionModel)(Session["User"])).UserId;
            var userEmail = _db.Customers.Find(userId).Email;

            var userInLoanId = _dbLoan.CustomersInLoans.FirstOrDefault(x => x.Email == userEmail).CustomerID;
            var myLoan = _dbLoan.LoanApplications.Where(x => x.CustomerID == userInLoanId).ToList();

            return View(myLoan);
        }

    }


}