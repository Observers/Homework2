using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Homework2.Models;
using System.Security.Cryptography;

namespace Homework2.Controllers
{
    public class ProfileController : Controller
    {
        // Login landing page. 
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["Profile"];
            // Check if cookie has login data saved.
            if (cookie != null)
            {
                ViewBag.username = cookie["username"].ToString();
                // Decrypt stored ASCII password from cookie into MD5 hash.
                string EncryptedPassword = cookie["password"].ToString();
                byte[] b = Convert.FromBase64String(EncryptedPassword);
                string DecryptedPassword = ASCIIEncoding.ASCII.GetString(b);
                ViewBag.password = DecryptedPassword.ToString();
            }
            return View();
        }

        // Login authentication function. Password is converted into MD5 hash.
        // If remember me is checked, password is then exrypted to ASCII and stored in cookie.
        [HttpPost]
        public ActionResult Login(ExtendedAccount acc, FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    // Encode text password into MD5 hash.
                    string hash = GetMd5Hash(md5Hash, acc.password);

                    HttpCookie cookie = new HttpCookie("Profile");
                    string f = form["RememberMe"];
                    if (f != null)
                    {
                        cookie["username"] = acc.username;
                        // Encrypt MD5 password into ASCII and save to cookie.
                        byte[] b = ASCIIEncoding.ASCII.GetBytes(acc.password);
                        string EncryptedPassword = Convert.ToBase64String(b);
                        cookie["password"] = EncryptedPassword;
                        // Set expire timmer to cookie.
                        cookie.Expires = DateTime.Now.AddDays(2);
                        HttpContext.Response.Cookies.Add(cookie);
                    }
                    else
                    {
                        // Remove any cookie related to this website.
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Response.Cookies.Add(cookie);
                    }

                    var userDetails = db.Accounts.SingleOrDefault(x => x.username == acc.username && x.password == hash);
                    if (userDetails == null)
                    {
                        TempData["Message"] = "Wrong Username or Password.";
                        return View("Index");
                    }
                    else
                    {
                        // Store neccessary information in session that will be used in all websites without having to
                        // continuously call the same information from database.
                        Session["userID"] = userDetails.userID;
                        Session["username"] = userDetails.username;
                        Session["mainMenu"] = userDetails.User.Role.Menus.Where(x => x.level == 0 || x.level == 1).OrderBy(x => x.menuNo).ToList();
                        Session["subMenu"] = userDetails.User.Role.Menus.Where(x => x.level == 2).ToList();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
        }

        // Function to convert plain text password into MD5 hash.
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Logout function.
        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Index", "Profile");
        }

        // Change password landing page.
        public ActionResult ChangePassword()
        {
            HttpCookie cookie = Request.Cookies["Profile"];
            if (cookie != null)
            {
                ViewBag.username = cookie["username"].ToString();
            }
            return View();
        }

        // Change password function.
        [HttpPost]
        public ActionResult ChangePassword(ExtendedAccount acc)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = GetMd5Hash(md5Hash, acc.password);
                    var userDetails = db.Accounts.SingleOrDefault(x => x.username == acc.username && x.password == hash);

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
                        userDetails.password = hash;
                        db.SaveChanges();

                        TempData["Message"] = "<script>alert('Password changed successfully!')</script>";
                        return View("ChangePassword");
                    }
                }
            }
        }
    }
}