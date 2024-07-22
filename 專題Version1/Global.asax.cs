using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using 專題Version1.Controllers;

namespace 專題Version1
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 應用程式啟動時執行的程式碼
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);            
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        routeData.Values.Add("action", "NotFound");
                        break;
                    case 500:
                        routeData.Values.Add("action", "ServerError");
                        break;
                    default:
                        routeData.Values.Add("action", "Index");
                        break;
                }
            }
            else
            {
                routeData.Values.Add("action", "Index");
            }

            Server.ClearError();

            Response.TrySkipIisCustomErrors = true;

            IController errorController = new ErrorController();
            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}