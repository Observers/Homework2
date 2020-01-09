using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Homework2.Models;

namespace Homework2.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["Account"];
            if (cookie != null)
            {
                ViewBag.username = cookie["username"].ToString();

                string EncryptedPassword = cookie["password"].ToString();
                byte[] b = Convert.FromBase64String(EncryptedPassword);
                string DecryptedPassword = ASCIIEncoding.ASCII.GetString(b);
                ViewBag.username = DecryptedPassword.ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account acc, FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                HttpCookie cookie = new HttpCookie("Account");
                string f = form["RememberMe"];
                if (f != null)
                {
                    cookie["username"] = acc.username;

                    byte[] b = ASCIIEncoding.ASCII.GetBytes(acc.username);
                    string EncryptedPassword = Convert.ToBase64String(b);
                    cookie["password"] = EncryptedPassword;

                    cookie.Expires = DateTime.Now.AddDays(2);
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    HttpContext.Response.Cookies.Add(cookie);
                }

                var userDetails = db.Accounts.Where(x => x.username == acc.username && x.password == acc.password).FirstOrDefault();
                if (userDetails == null)
                {
                    TempData["data"] = "Wrong Username or Password.";
                    return View("Index");
                }
                else
                {
                    Session["userID"] = acc.userID;
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Account");
        }
    }
}