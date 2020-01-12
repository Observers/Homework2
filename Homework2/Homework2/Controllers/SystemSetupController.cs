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
        public ActionResult RoleMaintenance(Role viewModel)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                viewModel.ListA = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.ListB = list;
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult QueryForm(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var selectRole = form["selectRole"];
                System.Diagnostics.Debug.WriteLine(selectRole); 
                var viewModel = new Role();
                if (selectRole != null)
                {
                    int iSelectRole = Int32.Parse(selectRole);
                    viewModel.ListA = db.Roles.ToList();
                    viewModel.ListB = db.Roles.Where(x => x.roleID == iSelectRole).ToList();
                }
                else
                {
                    viewModel.ListA = db.Roles.ToList();
                    viewModel.ListB = db.Roles.ToList();
                }
                return View("RoleMaintenance", viewModel);
            }
        }

        [HttpPost]
        public ActionResult AddForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MenuForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteForm()
        {
            return View();
        }
    }
}