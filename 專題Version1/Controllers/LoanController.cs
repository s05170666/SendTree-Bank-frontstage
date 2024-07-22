using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 專題Version1.Models;
using 專題Version1.Models.LoanModels;
using 專題Version1.Models.ViewModels;
using System.Data.Entity;
using System.IO;
using 專題Version1.Services;
using System.Threading.Tasks;
using System.Configuration;

namespace 專題Version1.Controllers
{
    [SessionCheckFilter]
    public class LoanController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        private AzureBlobService _azureBlobService;

        private Version3_CustomerEntities _db = new Version3_CustomerEntities();
        private Version3_LoanEntities1 _db2 = new Version3_LoanEntities1();

        // 貸款首頁
        public ActionResult Index()
        {
            return View();
        }

        // 顯示貸款產品列表
        public ActionResult LoanProductsList()
        {
            var loanProducts = _db2.LoanProducts.ToList();
            return View(loanProducts);
        }

        // 顯示貸款產品詳細資訊
        public ActionResult LoanProductDetail(int id)
        {
            var loanProduct = _db2.LoanProducts.Find(id);
            return View(loanProduct);
        }

        // 顯示一分鐘試算頁面
        public ActionResult OneMinTrial()
        {
            return View();
        }

        // 計算每月付款金額
        [HttpPost]
        public ActionResult CalculateMonthlyPayment(double amount, int year, double rate, string amortizationType)
        {
            double amountInYuan = amount * 10000;
            double monthlyInterestRate = rate / 12 / 100;
            int numberOfPayments = year * 12;
            double monthlyPayment = 0;

            if (amortizationType == "interestAndPrincipal")
            {
                monthlyPayment = (amountInYuan * monthlyInterestRate) / (1 - Math.Pow(1 + monthlyInterestRate, -numberOfPayments));
            }
            else if (amortizationType == "principalOnly")
            {
                monthlyPayment = amountInYuan / numberOfPayments;
            }

            double roundedMonthlyPayment = Math.Round(monthlyPayment, 2);
            return Content(roundedMonthlyPayment.ToString("N0"));
        }

        // 顯示貸款申請頁面
        [HttpGet]
        public ActionResult LoanCreate(int id)
        {
            int userId = ((SessionModel)(Session["User"])).UserId;
            var user = _db.Customers.Find(userId);
            var loanProduct = _db2.LoanProducts.Find(id);
            var loanApplcation = new LoanApplicationViewModel
            {
                CustomerID = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                InteresRate = loanProduct.InterestRate,
                LoanProductID = id,
                ProductName = loanProduct.ProductName,
                BaseRate = loanProduct.InterestRate
            };
            ViewBag.IncomeRangeList = new SelectList(_db2.IncomeRangeRates.ToList(), "AdjustmentRate", "IncomeRange");
            ViewBag.OccupationGroup = new SelectList(_db2.OccupationGroupRates.ToList(), "AdjustmentRate", "SubGroup");

            return View(loanApplcation);
        }

        // 預覽貸款申請
        [HttpPost]
        public async Task<ActionResult> Preview(LoanApplicationViewModel model)
        {
            var file = model.EconomicProof;
            var fileName = model.EconomicProof.FileName;
            if (file != null && file.ContentLength > 0)
            {
                using (var stream = file.InputStream)
                {
                    string containerName = "ecommer";
                    AzureBlobService azureBlobService = new AzureBlobService(connectionString, containerName);
                    string uniqueFileName = await azureBlobService.UploadFileAsync(fileName, stream);

                    if (uniqueFileName != "Upload failed")
                    {
                        model.EconomicProofPath = uniqueFileName; // 儲存文件名稱於模型
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("", uniqueFileName);
                    }
                }
            }
            return View(model);
        }

        // 提交貸款申請
        [HttpPost]
        public ActionResult LoanApply(LoanApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = ((SessionModel)(Session["User"])).UserId;
                var userEmail = _db.Customers.Find(userId).Email;
                var user = _db.Customers.Find(userId);
                var userLoan = _db2.CustomersInLoans.Include(c => c.LoanApplications)
                                .FirstOrDefault(c => c.Email == userEmail);

                if (userLoan == null)
                {
                    var newCustomerInLoan = new CustomersInLoan
                    {
                        Email = userEmail,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth
                    };

                    _db2.CustomersInLoans.Add(newCustomerInLoan);
                    _db2.SaveChanges();
                }

                var interRate = model.InteresRate + model.IncomeRange + model.OccupationGroup;
                var loanApplication = new LoanApplication
                {
                    CustomerID = model.CustomerID,
                    LoanProductID = model.LoanProductID,
                    LoanAmount = model.LoanAmount,
                    EconomicProof = model.EconomicProofPath,
                    DisbursementAccount = model.DisbursementAccount,
                    ApplicationDate = DateTime.Now,
                    LoanStatus = "Pending",
                    InterestRate = interRate,
                    RepaymentMonths = model.LoanTermMonths
                };

                _db2.LoanApplications.Add(loanApplication);
                _db2.SaveChanges();
                ViewBag.Message = "申請成功!!";
                return View(nameof(ApplyDone));
            }
            return View(model);
        }

        // 顯示申請完成頁面
        public ActionResult ApplyDone()
        {
            return View();
        }

        // 顯示我的貸款頁面
        public ActionResult MyLoan()
        {
            int userId = ((SessionModel)(Session["User"])).UserId;
            var userEmail = _db.Customers.Find(userId).Email;
            var userLoan = _db2.CustomersInLoans.Include(c => c.LoanApplications)
                            .FirstOrDefault(c => c.Email == userEmail);

            if (userLoan != null)
            {
                var loans = userLoan.LoanApplications.ToList();
                return View(loans);
            }
            else
            {
                ViewBag.Message = "尚未申請貸款";
                return View();
            }
        }

        // 顯示貸款詳細資訊
        public ActionResult LoanDetails(int id)
        {
            var loan = _db2.LoanApplications.Find(id);

            if (loan.LoanStatus == "Confirmed")
            {
                var repaymentschedule = _db2.RepaymentSchedules.Where(r => r.LoanApplicationID == id).ToList();
                int repaymentMonths = (int)loan.RepaymentMonths;
                var loanViewModle = new LoanDetailsViewModel
                {
                    CustomerID = loan.CustomerID,
                    LoanApplicationID = loan.LoanApplicationID,
                    ProductName = loan.LoanProduct.ProductName,
                    LoanAmount = loan.LoanAmount,
                    EconomicProof = loan.EconomicProof,
                    DisbursementAccount = loan.DisbursementAccount,
                    InteresRate = (decimal)loan.InterestRate,
                    ApplicationDate = loan.ApplicationDate,
                    Status = loan.LoanStatus,
                    RepaymentSchedules = repaymentschedule,
                    LoanTermMonths = repaymentMonths
                };

                return View(loanViewModle);
            }

            var loanViewModle2 = new LoanDetailsViewModel
            {
                CustomerID = loan.CustomerID,
                LoanApplicationID = loan.LoanApplicationID,
                ProductName = loan.LoanProduct.ProductName,
                LoanAmount = loan.LoanAmount,
                EconomicProof = loan.EconomicProof,
                DisbursementAccount = loan.DisbursementAccount,
                InteresRate = (decimal)loan.InterestRate,
                ApplicationDate = loan.ApplicationDate,
                Status = loan.LoanStatus,
                LoanTermMonths = (int)loan.RepaymentMonths,
                RepaymentSchedules = null
            };

            return View(loanViewModle2);
        }

        // 下載經濟證明
        public async Task<ActionResult> DownloadEconomicProof(string fileName)
        {
            string containerName = "ecommer";
            AzureBlobService azureBlobService = new AzureBlobService(connectionString, containerName);
            Stream stream = await azureBlobService.DownloadFileAsync(fileName);

            byte[] fileBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return File(fileBytes, "application/octet-stream", fileName);
        }

        // 取消貸款申請
        public ActionResult CancelApplication()
        {
            return View();
        }

        // 取消貸款申請 (POST)
        [HttpPost]
        public ActionResult CancelApplication(int loanApplicationID)
        {
            var loan = _db2.LoanApplications.Find(loanApplicationID);
            if (loan != null && loan.LoanStatus != "Confirmed")
            {
                _db2.LoanApplications.Remove(loan);
                _db2.SaveChanges();
                return RedirectToAction("MyLoan");
            }
            ViewBag.Message = "無法取消已確認的貸款申請";
            return View();
        }
    }
}
