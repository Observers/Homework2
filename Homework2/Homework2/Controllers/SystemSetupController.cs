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
                viewModel.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.rolesTableList = list;
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

                var viewModel = new Role();
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
                    viewModel.rolesList = db.Roles.ToList();
                    viewModel.rolesTableList = db.Roles.ToList();
                    TempData["Message"] = "Please select user from dropdown list.";
                    return View("RoleMaintenance", viewModel);
                }
            }
        }

        [HttpPost]
        public ActionResult ModifyRoleDatabase(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                Database(form, false);

                var viewModel = new Role();
                viewModel.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                viewModel.rolesTableList = list;
                TempData["Message"] = "<script>alert('Role has been successfully modified!')</script>";
                return View("RoleMaintenance", viewModel);
            }
        }

        [HttpGet]
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

        public ActionResult UserMaintenance(User users)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.ToList();
                users.userTableList = new List<User>();
                return View(users);
            }
        }

        [HttpPost]
        public ActionResult QueryUser(FormCollection form)
        {

            using (Trainee15Entities db = new Trainee15Entities())
            {
                User users = new User();
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.ToList();
                users.userTableList = new List<User>();

                var selectUser = form["selectUser"];
                var selectRole = form["selectRole"];

                if (selectUser != null && selectRole != null)
                {
                    int iSelectUser = int.Parse(selectUser);
                    int iSelectRole = int.Parse(selectRole);
                    System.Diagnostics.Debug.WriteLine(iSelectUser);
                    System.Diagnostics.Debug.WriteLine(iSelectRole);
                    users.userTableList = db.Users.Where(x => x.userID == iSelectUser && x.roleID == iSelectRole).ToList();
                    return View("UserMaintenance", users);
                }

                if (selectUser != null && selectRole == null)
                {
                    int iSelectUser = int.Parse(selectUser);
                    System.Diagnostics.Debug.WriteLine(iSelectUser);
                    users.userTableList = db.Users.Where(x => x.userID == iSelectUser).ToList();
                    return View("UserMaintenance", users);
                }

                if (selectUser == null && selectRole != null)
                {
                    int iSelectRole = int.Parse(selectRole);
                    System.Diagnostics.Debug.WriteLine(selectRole);
                    users.userTableList = db.Users.Where(x => x.roleID == iSelectRole).ToList();
                    return View("UserMaintenance", users);
                }

                users.userTableList = db.Users.ToList();
                return View("UserMaintenance", users);
            }
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(FormCollection form)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyUser(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var selectUser = form["selectUser"];
                if (selectUser != null)
                {
                    int iSelectUser = int.Parse(selectUser);
                    User user = db.Users.SingleOrDefault(x => x.userID == iSelectUser);
                    user.roleList = db.Roles.ToList();
                    return View("ModifyUser", user);
                }
                else
                {
                    var viewModel = new User();
                    viewModel.userList = db.Users.ToList();
                    viewModel.roleList = db.Roles.ToList();
                    viewModel.userTableList = new List<User>();
                    TempData["Message"] = "Please select user from dropdown list.";
                    return View("UserMaintenance", viewModel);
                }
            }
        }

        [HttpPost]
        public ActionResult ModifyUserDatabase(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                int sessionUserID = (int)Session["userID"];
                var sessionUser = db.Accounts.SingleOrDefault(x => x.userID == sessionUserID);

                var userID = form["selectUser"];
                var roleID = form["selectRole"];
                var selectStatus = form["selectStatus"];
                int iUserID = int.Parse(userID);
                int iRoleID = int.Parse(roleID);
                User user = db.Users.SingleOrDefault(x => x.userID == iUserID);
                user.roleID = iRoleID;
                if (selectStatus == "1")
                {
                    user.status = true;
                }
                else
                {
                    user.status = false;
                }
                DateTime curretDateTime = DateTime.Now;
                user.modifyDate = curretDateTime;
                user.modifyUser = sessionUser.username;
                db.SaveChanges();

                User users = new User();
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.ToList();
                users.userTableList = new List<User>();
                TempData["Message"] = "<script>alert('Role has been successfully modified!')</script>";
                return View("UserMaintenance", users);
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(FormCollection form)
        {
            using(Trainee15Entities db = new Trainee15Entities())
            {
                string[] checkboxes = form["checkbox"].Split(',');
                foreach (var user in checkboxes)
                {
                    int iUserID = Int32.Parse(user);
                    User deletUser = db.Users.SingleOrDefault(x => x.userID == iUserID);
                    db.Users.Remove(deletUser);
                    db.SaveChanges();
                }
                User users = new User();
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.ToList();
                users.userTableList = new List<User>();
                TempData["Message"] = "<script>alert('One or more user has been successfully deleted!')</script>";
                return View("UserMaintenance", users);
            }
        }
    }
}