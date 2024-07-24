using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EntityFramwork.Auth
{
    public class Authorization : FilterAttribute, IAuthorizationFilter
    {
        private string[] _roles;
        public Authorization(params string[] role){

            _roles = role;
        }

        private Demo1Entities db = new Demo1Entities();
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var UserName = Convert.ToString(filterContext.HttpContext.Session["UserName"]);
            var Password = Convert.ToString(filterContext.HttpContext.Session["Password"]);
            var checkUser = db.Users.Where(x => x.UserName == UserName && x.Password == Password).FirstOrDefault();

            if(checkUser == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Logout");
            }
            else
            {
                var userRole = db.UserRoles.Include("Role").Where(u => u.UserID == checkUser.UserID).ToList();

                if(_roles.Count() > 0)
                {
                    var roles = _roles.FirstOrDefault().Split(',');

                    string[] userRoleName = userRole.Select(x => x.Role.RoleName).ToArray();

                    var CheckRightRole = roles.Intersect(userRoleName);

                    //var CheckRightRole = roles.Contains(userRole?.Role.RoleName);

                    if (CheckRightRole.Count() <0)
                    {
                        filterContext.Result = new RedirectResult("~/Error/AccessDenied");
                    }
                }

            }
        }
    }
}