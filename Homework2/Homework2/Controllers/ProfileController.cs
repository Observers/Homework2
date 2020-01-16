using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Homework2.Models;

namespace Homework2.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["Profile"];
            if (cookie != null)
            {
                ViewBag.username = cookie["username"].ToString();

                string EncryptedPassword = cookie["password"].ToString();
                byte[] b = Convert.FromBase64String(EncryptedPassword);
                string DecryptedPassword = ASCIIEncoding.ASCII.GetString(b);
                ViewBag.password = DecryptedPassword.ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account acc, FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                HttpCookie cookie = new HttpCookie("Profile");
                string f = form["RememberMe"];
                if (f != null)
                {
                    cookie["username"] = acc.username;

                    byte[] b = ASCIIEncoding.ASCII.GetBytes(acc.password);
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
                //Where(x => x.username == acc.username && x.password == acc.password).FirstOrDefault();
                var userDetails = db.Accounts.SingleOrDefault(x => x.username == acc.username && x.password == acc.password); 
                if (userDetails == null)
                {
                    TempData["Message"] = "Wrong Username or Password.";
                    return View("Index");
                }
                else
                {
                    Session["userID"] = userDetails.userID;
                    Session["username"] = userDetails.username;
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Profile");
        }

        public ActionResult ChangePassword()
        {
            HttpCookie cookie = Request.Cookies["Profile"];
            if (cookie != null)
            {
                ViewBag.username = cookie["username"].ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(Account acc)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                Account userDetails = db.Accounts.SingleOrDefault(x => x.username == acc.username && x.password == acc.password);
                if (userDetails == null)
                {
                    HttpCookie cookie = Request.Cookies["Profile"];
                    if (cookie != null)
                    {
                        ViewBag.username = cookie["username"].ToString();
                    }
                    TempData["Message"] = "Wrong Username or Password.";
                    return View("ChangePassword");
                }
                else
                {
                    userDetails.password = acc.newPassword;

                    db.SaveChanges();

                    TempData["Message"] = "<script>alert('Password changed successfully!')</script>";

                    return View("ChangePassword");
                }
            }
        }
    }
}