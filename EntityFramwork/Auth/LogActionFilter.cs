using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EntityFramwork.Auth
{
    public class LogActionFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log("Action method executing" + filterContext.ActionDescriptor.ActionName);
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("Action method executed" + filterContext.ActionDescriptor.ActionName);
        }


        public void Log(string msg)
        {

        }

    }
}