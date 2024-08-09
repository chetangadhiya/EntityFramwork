using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EntityFramwork.Auth
{
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var cName = filterContext.RouteData.Values["Controller"];
            var aName = filterContext.RouteData.Values["action"];
            var errorMsg = filterContext.Exception.Message;
            var errorStackTrace = filterContext.Exception.StackTrace;

            Log("Exception occureed in controller: " + cName + "and Action Name is" + aName + "and error message" + errorMsg + "and Stack trace" + errorStackTrace);

            filterContext.ExceptionHandled = true;

            filterContext.Result = new RedirectResult("~/Error/ErrorIndex");
        }

        private void Log(string msg)
        {
            string m_exepath = @"F:\ShowIT\.Net\EntityFramwork\EntityFramwork\";
            File.AppendAllText(m_exepath + "log.txt", "-----------------------------------------------------------------");


            File.AppendAllText(m_exepath + "log.txt", msg);

        }
    }
}