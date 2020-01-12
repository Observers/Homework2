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
        public ActionResult QueryRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var selectRole = form["selectRole"];
                var viewModel = new Role();
                if (selectRole != null)
                {
                    int iSelectRole = int.Parse(selectRole);
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

        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                int userID = (int)Session["userID"];
                var user = db.Accounts.SingleOrDefault(x => x.userID == userID);

                Role roleAdd = new Role();
                roleAdd.role = form["role"];
                roleAdd.description = form["description"];
                var selectStatus = form["selectStatus"];
                if (selectStatus == "1")
                {
                    roleAdd.status = true;
                }
                else
                {
                    roleAdd.status = false;
                }
                DateTime curretDateTime = DateTime.Now;
                roleAdd.createDate = curretDateTime;
                roleAdd.createUser = user.username;
                roleAdd.modifyDate = curretDateTime;
                roleAdd.modifyUser = user.username;

                db.Roles.Add(roleAdd);
                db.SaveChanges();

                var viewModel = new Role();
                viewModel.ListA = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.ListB = list;

                return View("RoleMaintenance", viewModel);
            }
        }

        [HttpPost]
        public ActionResult ModifyRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var selectRole = form["selectRole"];
                if (selectRole != null)
                {
                    int iSelectRole = int.Parse(selectRole);
                    Role role = db.Roles.SingleOrDefault(x => x.roleID == iSelectRole);
                    return View("ModifyRole", role);
                }
                else
                {
                    var viewModel = new Role();
                    viewModel.ListA = db.Roles.ToList();
                    viewModel.ListB = db.Roles.ToList();
                    return View("RoleMaintenance", viewModel);
                }

            }
        }

        [HttpPost]
        public ActionResult ModifyDatabase(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                int userID = (int)Session["userID"];
                var user = db.Accounts.SingleOrDefault(x => x.userID == userID);

                var selectRole = form["selectRole"];
                System.Diagnostics.Debug.WriteLine(selectRole);
                int iSelectRole = int.Parse(selectRole);
                Role roleModified = db.Roles.SingleOrDefault(x => x.roleID == iSelectRole);
                roleModified.description = form["description"];
                var selectStatus = form["selectStatus"];
                if (selectStatus == "1")
                {
                    roleModified.status = true;
                }
                else
                {
                    roleModified.status = false;
                }
                DateTime curretDateTime = DateTime.Now;
                roleModified.modifyDate = curretDateTime;
                roleModified.modifyUser = user.username;

                db.SaveChanges();

                var viewModel = new Role();
                viewModel.ListA = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.ListB = list;

                return View("RoleMaintenance", viewModel);
            }
        }

        [HttpPost]
        public ActionResult MenuRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteRole()
        {
            return View();
        }
    }
}