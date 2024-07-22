using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 專題Version1.Services
{
    public class SessionCheckFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] == null)
            {
                filterContext.Result = new RedirectResult("/User/Login2");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}