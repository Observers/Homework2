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
        // Role Maintenance landing page.
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

        // Query role table from database.
        [HttpPost]
        public ActionResult QueryRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var selectRole = form["selectRole"];
                var viewModel = new ExtendedRole();
                if (selectRole != "")
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

        // Add role landing page.
        [HttpGet]
        public ActionResult AddRole()
        {
            return View();
        }

        // Add role to database function.
        [HttpPost]
        public ActionResult AddRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                // Calling database function down below.
                Database(form, true);

                ExtendedRole viewModel = new ExtendedRole();
                viewModel.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.rolesTableList = list;
                TempData["Message"] = "Role has been successfully added!";
                return View("RoleMaintenance", viewModel);
            }
        }

        // Modify role in database landing page.
        [HttpPost]
        public ActionResult ModifyRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedRole viewModel = new ExtendedRole();
                viewModel.rolesList = db.Roles.ToList();
                viewModel.rolesTableList = db.Roles.ToList();
                try
                {
                    // Checkbox is the attribute name. 
                    // Returns values of the boxes that have been checked as csv.
                    string[] checkboxes = form["checkbox"].Split(',');
                    if (checkboxes.Length != 1) // More than 1 checkbox selected.
                    {
                        TempData["Message"] = "Error: Can only select one to modify from table";
                        return View("RoleMaintenance", viewModel);
                    }
                    else
                    {
                        int iSelectRole = int.Parse(checkboxes[0]);
                        Role role = db.Roles.SingleOrDefault(x => x.roleID == iSelectRole);
                        return View("ModifyRole", role);
                    }
                }
                catch (Exception e) // Catch exception when no item from table was selected.
                {
                    TempData["Message"] = "Error: No item was selected from table.";
                    return View("RoleMaintenance", viewModel);
                }
            }
        }

        // Modify and update role in database function.
        [HttpPost]
        public ActionResult ModifyRoleDatabase(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                // Calling database function down below.
                Database(form, false);

                ExtendedRole viewModel = new ExtendedRole();
                viewModel.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.rolesTableList = list;
                TempData["Message"] = "Role has been successfully modified!";
                return View("RoleMaintenance", viewModel);
            }
        }

        // Role to Menu landing page.
        [HttpPost]
        public ActionResult MenuRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedRole viewModel = new ExtendedRole();
                viewModel.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.rolesTableList = list;

                try
                {
                    // Checkbox is the attribute name. 
                    // Returns values of the boxes that have been checked as csv.
                    string[] checkboxes = form["checkbox"].Split(',');

                    if (checkboxes.Length != 1) // More than 1 checkbox selected.
                    {
                        TempData["Message"] = "Error: Can only select one to modify from table";
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
                catch (Exception e) // Catch exception when no item from table was selected.
                {
                    TempData["Message"] = "Error: No item was selected from table.";
                    return View("RoleMaintenance", viewModel);
                }
            }
        }

        // Update Role to Menu in the database.
        [HttpPost]
        public ActionResult MenuRoleDatabase(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var roleID = int.Parse(form["selectRole"]);
                ExtendedRole viewModel = new ExtendedRole();
                try
                {
                    // Checkbox is the attribute name. 
                    // Returns values of the boxes that have been checked as csv.
                    string[] checkboxes = form["checkbox"].Split(',');

                    var menus = db.Menus.ToList();
                    var role = db.Roles.SingleOrDefault(x => x.roleID == roleID);
                    foreach (var menu in menus)
                    {
                        // Add to menu to role.
                        if (checkboxes.Contains(menu.menuID.ToString()))
                        {
                            if (!menu.Roles.Contains(role))
                            {
                                db.Menus.SingleOrDefault(x => x.menuID == menu.menuID).Roles.Add(role);
                                db.SaveChanges();
                            }
                        }
                        // Remove menu from role.
                        if (!checkboxes.Contains(menu.menuID.ToString()))
                        {
                            if (menu.Roles.Contains(role))
                            {
                                db.Roles.SingleOrDefault(x => x.roleID == role.roleID).Menus.Remove(menu);
                                db.SaveChanges();
                            }
                        }
                    }

                    viewModel.rolesList = db.Roles.ToList();
                    List<Role> list = new List<Role>();
                    viewModel.rolesTableList = list;
                    TempData["Message"] = " Role - Menu updated.";
                    return View("RoleMaintenance", viewModel);
                }
                catch (Exception e) // Catch exception when no item from table was selected.
                {
                    viewModel.role = db.Roles.SingleOrDefault(x => x.roleID == roleID);
                    viewModel.menu = db.Menus.ToList();
                    viewModel.menuRole = viewModel.role.Menus.ToList();
                    TempData["Message"] = "Error: No item was selected from table.";
                    return View("MenuRole", viewModel);
                }
            }
        }

        // Delete role(s) from database function.
        [HttpPost]
        public ActionResult DeleteRole(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedRole viewModel = new ExtendedRole();

                try
                {
                    // Checkbox is the attribute name. 
                    // Returns values of the boxes that have been checked as csv.
                    string[] checkboxes = form["checkbox"].Split(',');
                    foreach (var role in checkboxes)
                    {
                        int iRoleID = Int32.Parse(role);
                        // Delete role-menu relationship first before deleting roles.
                        List<Menu> dropMenus = db.Roles.SingleOrDefault(r => r.roleID == iRoleID).Menus.ToList();
                        foreach (var drop in dropMenus)
                        {
                            db.Roles.SingleOrDefault(r => r.roleID == iRoleID).Menus.Remove(drop);
                        }
                        Role deleteRole = db.Roles.SingleOrDefault(r => r.roleID == iRoleID);
                        db.Roles.Remove(deleteRole);
                        db.SaveChanges();
                    }
                    viewModel.rolesList = db.Roles.ToList();
                    List<Role> list = new List<Role>();
                    viewModel.rolesTableList = list;
                    TempData["Message"] = "One or more role has been successfully deleted!";
                    return View("RoleMaintenance", viewModel);
                }
                catch (Exception e) // Catch exception when no item from table was selected.
                {
                    viewModel.rolesList = db.Roles.ToList();
                    List<Role> list = new List<Role>();
                    viewModel.rolesTableList = list;
                    TempData["Message"] = "Error: No item was selected from table.";
                    return View("RoleMaintenance", viewModel);
                }


            }
        }

        // Database function to add and modify records accordingly.
        public void Database(FormCollection form, bool action)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                int sessionUserID = (int)Session["userID"];
                var sessionUser = db.Accounts.SingleOrDefault(x => x.userID == sessionUserID);

                Role role = new Role();

                if (action) // Add role.
                {
                    role.roleName = form["roleName"];
                }
                if (!action) // Modify role.
                {
                    var selectRole = form["selectRole"];
                    int iSelectRole = int.Parse(selectRole);
                    role = db.Roles.SingleOrDefault(x => x.roleID == iSelectRole);
                }

                role.description = form["description"];
                var selectStatus = form["selectStatus"];
                role.status = false;
                if (selectStatus == "1")
                {
                    role.status = true;
                }

                DateTime curretDateTime = DateTime.Now;
                if (action) // Add role
                {
                    role.createDate = curretDateTime;
                    role.createUser = sessionUser.username;
                }
                role.modifyDate = curretDateTime;
                role.modifyUser = sessionUser.username;

                if (action) // Add role
                {
                    db.Roles.Add(role);
                }

                db.SaveChanges();
            }
        }
    }
}