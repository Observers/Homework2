using Homework2.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework2.Controllers
{
    public class SystemSetupController : Controller
    {
        // GET: SystemSetup
        public ActionResult RoleMaintenance()
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var roles = db.Roles.ToList();
                return View(roles);
            }
        }
    }
}