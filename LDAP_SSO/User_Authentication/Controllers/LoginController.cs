using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace User_Authentication.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            Models.LoginUser user = new Models.LoginUser();
            if (Request.IsAuthenticated)
            {
                if (TempData["USER"] != null)
                {
                    user = (Models.LoginUser)TempData["USER"];
                }
                else
                {
                    user.Username = User.Identity.Name;
                }
                
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Models.LoginUser user)
        {
            if (ModelState.IsValid)
            {
                
                if (user.Authenticate(user.Username,user.Password,user.Domain))
                {
                    FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                    TempData["USER"] = user;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Error","");
                }
                
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
