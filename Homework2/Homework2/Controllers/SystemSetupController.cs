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
                Database(form, true);

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
                Database(form, false);

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
        public ActionResult DeleteRole(FormCollection form)
        {
            var viewModel = new Role();
            using (Trainee15Entities db = new Trainee15Entities())
            {
                string[] checkboxes = form["checkbox"].Split(',');
                foreach (var role in checkboxes)
                {
                    int iRoleID = Int32.Parse(role);
                    Role deleteRole = db.Roles.SingleOrDefault(r => r.roleID == iRoleID);
                    db.Roles.Remove(deleteRole);
                    db.SaveChanges();
                }

                viewModel.ListA = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.ListB = list;
            }

            return View("RoleMaintenance", viewModel);
        }

        public void Database(FormCollection form, bool action)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                int userID = (int)Session["userID"];
                var user = db.Accounts.SingleOrDefault(x => x.userID == userID);

                Role role = new Role();

                if (action)
                {
                    role.role = form["role"];
                }
                if (!action)
                {
                    var selectRole = form["selectRole"];
                    int iSelectRole = int.Parse(selectRole);
                    role = db.Roles.SingleOrDefault(x => x.roleID == iSelectRole);
                }

                role.description = form["description"];
                var selectStatus = form["selectStatus"];
                if (selectStatus == "1")
                {
                    role.status = true;
                }
                else
                {
                    role.status = false;
                }

                DateTime curretDateTime = DateTime.Now;
                if (action)
                {
                    role.createDate = curretDateTime;
                    role.createUser = user.username;
                }
                role.modifyDate = curretDateTime;
                role.modifyUser = user.username;

                if (action)
                {
                    db.Roles.Add(role);
                }

                db.SaveChanges();
            }
        }
    }
}