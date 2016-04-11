using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace User_Authentication.Controllers
{
    public class AppController : Controller
    {
        //
        // GET: /App/

        public ActionResult Index()
        {
            string userData = "";
            if (Request.IsAuthenticated)
            {
                Models.LoginUser user = new Models.LoginUser();
                user.Username = User.Identity.Name;
               userData= user.FetchUserDetails();
               ViewData["USERDATA"] = userData;
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

    }
}
