using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework2.Models;

namespace Homework2.Controllers
{
    public class HomeController : Controller
    {
        // Home landing page function
        public ActionResult Index()
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var userID = (int)Session["userID"];
                Home encodedEditor = db.Homes.SingleOrDefault(x => x.userID == userID);
                return View(encodedEditor);
            }
        }

        public ActionResult CKEditor()
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var userID = (int)Session["userID"];
                Home editor = db.Homes.SingleOrDefault(x => x.userID == userID);
                return View(editor);
            }
        }
    }
}