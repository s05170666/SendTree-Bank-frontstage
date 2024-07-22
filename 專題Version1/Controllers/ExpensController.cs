using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 專題Version1.Controllers
{
    public class ExpensController : Controller
    {
        // GET: Expens
        public ActionResult Index()
        {
            return View();
        }
    }
}