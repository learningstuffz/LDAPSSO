using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using User_Authentication;
using System.Web.Mvc;
using System.Web;
using System.IO;
using Moq;
using System.Web.Routing;

namespace User_Authentication.Tests
{
    [TestClass]
    public class LoginControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {

        }
        // Testing Index
        [TestMethod]
        public void IndexData()
        {
            /*Testing when the user is loging in first time*/
            Controllers.LoginController login = new Controllers.LoginController();

            var request = new Mock<HttpRequestBase>();
            // Not working - IsAjaxRequest() is static extension method and cannot be mocked
            // request.Setup(x => x.IsAjaxRequest()).Returns(true /* or false */);
            // use this
            request.SetupGet(x => x.Headers).Returns(
                new System.Net.WebHeaderCollection {
        {"X-Requested-With", "XMLHttpRequest"}
    });

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            login.ControllerContext = new ControllerContext(context.Object, new RouteData(), login);
            var result = login.Index() as RedirectToRouteResult;

            Assert.AreEqual("Login", result.RouteValues["action"]);
        }

        //Testing Login Details -- with correct details
        [TestMethod]
        public void LoginData()
        {
            Controllers.LoginController login = new Controllers.LoginController();
            Models.LoginUser usr= new Models.LoginUser();
            usr.Username = "read-only-admin";
            usr.Password = "password";
            usr.Domain = "example.com";

            var result = login.Login(usr) as RedirectToRouteResult;
            Assert.AreEqual("Index", result.RouteValues["action"]);
            
        }


        //Testing Login Details -- with in-correct details
        [TestMethod]
        public void LoginInvalidData()
        {
            Controllers.LoginController login = new Controllers.LoginController();
            Models.LoginUser usr = new Models.LoginUser();
            usr.Username = "read-only-admin";
            usr.Password = "wrongdetails";
            usr.Domain = "example.com";

            var result = login.Login(usr) as ViewResult;
            Assert.AreEqual("AUTH FAILED", result.ViewData["Error"]);

        }
    }
}
