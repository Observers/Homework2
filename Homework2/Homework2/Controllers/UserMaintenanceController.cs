using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework2.Models;

namespace Homework2.Controllers
{
    public class UserMaintenanceController : Controller
    {
        public ActionResult UserMaintenance(ExtendedUser users)
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
                ExtendedUser users = new ExtendedUser();
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.ToList();
                users.userTableList = new List<User>();

                var selectUser = form["selectUser"];
                var selectRole = form["selectRole"];

                if (selectUser != null && selectRole != null)
                {
                    int iSelectUser = int.Parse(selectUser);
                    int iSelectRole = int.Parse(selectRole);
                    users.userTableList = db.Users.Where(x => x.userID == iSelectUser && x.roleID == iSelectRole).ToList();
                    return View("UserMaintenance", users);
                }

                if (selectUser != null && selectRole == null)
                {
                    int iSelectUser = int.Parse(selectUser);
                    users.userTableList = db.Users.Where(x => x.userID == iSelectUser).ToList();
                    return View("UserMaintenance", users);
                }

                if (selectUser == null && selectRole != null)
                {
                    int iSelectRole = int.Parse(selectRole);
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
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedRole roles = new ExtendedRole();
                roles.rolesList = db.Roles.ToList();
                return View(roles);
            }
        }

        [HttpPost]
        public ActionResult AddUser(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                int sessionUserID = (int)Session["userID"];
                var sessionUser = db.Accounts.SingleOrDefault(x => x.userID == sessionUserID);
                var username = form["username"];
                var roleId = form["selectRole"];
                int iRoleID = int.Parse(roleId);
                var selectStatus = form["selectStatus"];
                DateTime curretDateTime = DateTime.Now;

                Account newAccount = new Account();
                newAccount.username = username;
                newAccount.password = username + "12345";
                db.Accounts.Add(newAccount);

                User addUser = new User();
                addUser.username = username;
                addUser.roleID = iRoleID;
                if (selectStatus == "1")
                {
                    addUser.status = true;
                }
                else
                {
                    addUser.status = false;
                }
                addUser.createDate = curretDateTime;
                addUser.createUser = sessionUser.username;
                addUser.modifyDate = curretDateTime;
                addUser.modifyUser = sessionUser.username;
                db.Users.Add(addUser);
                db.SaveChanges();

                ExtendedUser users = new ExtendedUser();
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.ToList();
                users.userTableList = new List<User>();
                TempData["Message"] = "<script>alert('A new user has been successfully added!')</script>";
                return View("UserMaintenance", users);
            }
        }

        [HttpPost]
        public ActionResult ModifyUser(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                string[] checkboxes = form["checkbox"].Split(',');
                if (checkboxes.Length != 1)
                {
                    ExtendedUser viewModel = new ExtendedUser();
                    viewModel.userList = db.Users.ToList();
                    viewModel.roleList = db.Roles.ToList();
                    viewModel.userTableList = new List<User>();
                    TempData["Message"] = "Can only select one from table to modify";
                    return View("UserMaintenance", viewModel);
                }
                else
                {
                    int iSelectUser = int.Parse(checkboxes[0]);
                    ExtendedUser user = new ExtendedUser();
                    user.user = db.Users.SingleOrDefault(x => x.userID == iSelectUser);
                    user.roleList = db.Roles.ToList();
                    return View("ModifyUser", user);
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

                ExtendedUser users = new ExtendedUser();
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
            using (Trainee15Entities db = new Trainee15Entities())
            {
                string[] checkboxes = form["checkbox"].Split(',');
                foreach (var user in checkboxes)
                {
                    int iUserID = Int32.Parse(user);
                    User deletUser = db.Users.SingleOrDefault(x => x.userID == iUserID);
                    db.Users.Remove(deletUser);
                    db.SaveChanges();
                }
                ExtendedUser users = new ExtendedUser();
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.ToList();
                users.userTableList = new List<User>();
                TempData["Message"] = "<script>alert('One or more user has been successfully deleted!')</script>";
                return View("UserMaintenance", users);
            }
        }
    }
}