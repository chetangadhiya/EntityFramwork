using EntityFramwork.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EntityFramwork.Controllers
{
    [LogActionFilter]

    public class AccountController : Controller
    {
        private Demo1Entities db = new Demo1Entities();

        [LogResultFilter]
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]

        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var UserData = db.Users.ToList().Where(us => us.UserName.Equals(user.UserName) && us.Password.Equals(user.Password)).FirstOrDefault();

                if(UserData != null)
                {
                    Session["UserName"] = user.UserName;
                    Session["Password"] = user.Password;

                    return RedirectToAction("Index", "Employee");
                }
                ViewBag.Validation = "Invalid username And Password";
            }
            return View(user);

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}