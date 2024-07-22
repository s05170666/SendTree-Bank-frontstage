using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using 專題Version1.Models;
using 專題Version1.Models.ViewModels;
using System.Data.Entity;
using 專題Version1.Services;
using System.Threading.Tasks;


namespace 專題Version1.Controllers
{
    public class UserController : Controller
    {
        private Version3_CustomerEntities _db = new Version3_CustomerEntities();
        private ExchangeRateService _exchangeRateService = new ExchangeRateService();
        // GET: User

        [SessionCheckFilter]
        public ActionResult Index()
        {
            //更新Session
            int userId = ((SessionModel)(Session["User"])).UserId;
            var user = _db.Customers.Include(c => c.Accounts)
                .Include(c => c.Accounts.Select(a => a.AccountType))
                .FirstOrDefault(x => x.CustomerID == userId);

            var session =  new SessionModel
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
            Session["User"] = session;

           

            return View();
            
        }

        public async Task<ActionResult> GetRates()
        {
            var toCurrencies = new List<string> { "USD", "EUR", "JPY", "GBP" }; // 替換為你需要的貨幣
            var exchangeRates = await _exchangeRateService.GetExchangeRates("TWD", toCurrencies);

            return Json(exchangeRates, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Login2()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                var user = _db.Customers
                           .Include(c => c.Accounts)
                           .Include(c => c.Accounts.Select(a => a.AccountType))
                           .FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (user != null)
                {

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

                    return Json(new { success = true });
                };
            }
            ViewBag.Error = "Invalid Email or Password";
        return Json(new { success = false, error = "Invalid Email or Password" });
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Resgister(RegisterViewModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (_db.Customers.Any(c => c.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (ModelState.IsValid)
            {
                Customer customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    Password = EncryptPassword(model.Password),
                    CreatedAt = DateTime.Now,
                };
                _db.Customers.Add(customer);

                Account account = new Account
                {
                    CustomerID = customer.CustomerID,
                    AccountNumber = "1" + (new Random().Next(10000000, 99999999).ToString()),
                    AccountTypeID = 1,
                    Balance = 0,
                    CreatedAt = DateTime.Now,
                };
                _db.Accounts.Add(account);
                _db.SaveChanges();
                return RedirectToAction(nameof(Login));
            }
            return View(model);
        }

        public ActionResult CheckEmail(string email)
        {
            if (_db.Customers.Any(c => c.Email == email))
            {
                return Json("Email already exists", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        //加密密碼
        public static string EncryptPassword(string password)
        {
            // 使用 SHA256 演算法
            using (SHA256 sha256 = SHA256.Create())
            {
                // 將輸入的密碼轉換為字節陣列，並計算其哈希值
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // 建立一個 StringBuilder 來儲存哈希值的 16 進制表示
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    // 將每個位元組轉換為 16 進制字串並附加到 StringBuilder 中
                    builder.Append(bytes[i].ToString("x2"));
                }

                // 返回最終的 16 進制哈希值字串
                return builder.ToString();
            }
        }

    }
}