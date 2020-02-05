using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework2.Models;

namespace Homework2.Controllers
{
    public class RoleMaintenanceController : Controller
    {
        public ActionResult RoleMaintenance(ExtendedRole roles)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                roles.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                roles.rolesTableList = list;
                return View(roles);
            }
        }

        [HttpPost]
        public ActionResult QueryRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var selectRole = form["selectRole"];
                var viewModel = new ExtendedRole();
                if (selectRole != null)
                {
                    int iSelectRole = int.Parse(selectRole);
                    viewModel.rolesList = db.Roles.ToList();
                    viewModel.rolesTableList = db.Roles.Where(x => x.roleID == iSelectRole).ToList();
                }
                else
                {
                    viewModel.rolesList = db.Roles.ToList();
                    viewModel.rolesTableList = db.Roles.ToList();
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

                ExtendedRole viewModel = new ExtendedRole();
                viewModel.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.rolesTableList = list;
                TempData["Message"] = "<script>alert('Role has been successfully added!')</script>";
                return View("RoleMaintenance", viewModel);
            }
        }

        [HttpPost]
        public ActionResult ModifyRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                string[] checkboxes = form["checkbox"].Split(',');
                ExtendedRole viewModel = new ExtendedRole();
                if (checkboxes.Length != 1)
                {
                    viewModel.rolesList = db.Roles.ToList();
                    viewModel.rolesTableList = db.Roles.ToList();
                    TempData["Message"] = "Can only select one to modify from table";
                    return View("RoleMaintenance", viewModel);
                }
                else
                {
                    int iSelectRole = int.Parse(checkboxes[0]);
                    Role role = db.Roles.SingleOrDefault(x => x.roleID == iSelectRole);
                    return View("ModifyRole", role);
                }
            }
        }

        [HttpPost]
        public ActionResult ModifyRoleDatabase(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                Database(form, false);

                ExtendedRole viewModel = new ExtendedRole();
                viewModel.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.rolesTableList = list;
                TempData["Message"] = "<script>alert('Role has been successfully modified!')</script>";
                return View("RoleMaintenance", viewModel);
            }
        }

        [HttpPost]
        public ActionResult MenuRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                string[] checkboxes = form["checkbox"].Split(',');
                ExtendedRole viewModel = new ExtendedRole();
                if (checkboxes.Length != 1)
                {
                    viewModel.rolesList = db.Roles.ToList();
                    List<Role> list = new List<Role>();
                    viewModel.rolesTableList = list;
                    TempData["Message"] = "Can only select one to modify from table";
                    return View("RoleMaintenance", viewModel);
                }
                else
                {
                    int iSelectRole = int.Parse(checkboxes[0]);
                    viewModel.role = db.Roles.SingleOrDefault(x => x.roleID == iSelectRole);
                    viewModel.menu = db.Menus.ToList();
                    viewModel.menuRole = viewModel.role.Menus.ToList();
                    return View("MenuRole", viewModel);
                }
            }
        }

        [HttpPost]
        public ActionResult MenuRoleDatabase(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var roleID = int.Parse(form["selectRole"]);
                System.Diagnostics.Debug.WriteLine("-------> RoleID: " + roleID);
                string[] checkboxes = form["checkbox"].Split(',');
                var menus = db.Menus.ToList();
                var role = db.Roles.SingleOrDefault(x => x.roleID == roleID);
                foreach (var menu in menus)
                {
                    if (checkboxes.Contains(menu.menuID.ToString()))
                    {
                        if (!menu.Roles.Contains(role))
                        {
                            db.Menus.SingleOrDefault(x => x.menuID == menu.menuID).Roles.Add(role);
                            db.SaveChanges();
                        }
                    }
                    if (!checkboxes.Contains(menu.menuID.ToString()))
                    {
                        if (menu.Roles.Contains(role))
                        {
                            db.Roles.SingleOrDefault(x => x.roleID == role.roleID).Menus.Remove(menu);
                            db.SaveChanges();
                        }
                    }
                }
                ExtendedRole viewModel = new ExtendedRole();
                viewModel.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.rolesTableList = list;
                TempData["Message"] = " Role - Menu updated.";
                return View("RoleMaintenance", viewModel);
            }
        }

        [HttpPost]
        public ActionResult DeleteRole(FormCollection form)
        {
            var viewModel = new ExtendedRole();
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

                viewModel.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.rolesTableList = list;
            }
            TempData["Message"] = "<script>alert('One or more role has been successfully deleted!')</script>";
            return View("RoleMaintenance", viewModel);
        }

        public void Database(FormCollection form, bool action)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                int sessionUserID = (int)Session["userID"];
                var sessionUser = db.Accounts.SingleOrDefault(x => x.userID == sessionUserID);

                Role role = new Role();

                if (action)
                {
                    role.roleName = form["roleName"];
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
                    role.createUser = sessionUser.username;
                }
                role.modifyDate = curretDateTime;
                role.modifyUser = sessionUser.username;

                if (action)
                {
                    db.Roles.Add(role);
                }

                db.SaveChanges();
            }
        }
    }
}