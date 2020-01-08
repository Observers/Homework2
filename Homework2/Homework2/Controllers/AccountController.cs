using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Homework2.Models;
using System.Data.SqlClient;

namespace Homework2.Controllers
{
    public class AccountController : Controller
    {
        readonly SqlConnection con = new SqlConnection();
        readonly SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        void connectionString()
        {
            con.ConnectionString = "data source=SILVER; database=Trainee15; integrated security=SSPI;";
        }

        [HttpPost]
        public IActionResult Verify(AccountModel acc)
        {
            System.Diagnostics.Debug.WriteLine(acc.Username);
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from account where username='"+acc.Username+"' and password='"+acc.Password+"'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                System.Diagnostics.Debug.WriteLine("HI");
                con.Close();
                return RedirectToAction("Index","Home");
            }
            else
            {
                con.Close();
                return View("../Home/Index.cshtml");
            }
        }
    }
}