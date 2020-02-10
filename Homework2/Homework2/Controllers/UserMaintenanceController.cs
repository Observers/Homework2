using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework2.Models;
using System.Security.Cryptography;

namespace Homework2.Controllers
{
    public class UserMaintenanceController : Controller
    {
        // User Maintenance landing page.
        public ActionResult UserMaintenance(ExtendedUser users)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.Where(x => x.status == true).ToList();
                users.userTableList = new List<User>();
                return View(users);
            }
        }

        // Query user table from database function.
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

                var queryUser = db.Users.AsQueryable();
                if (selectUser != "")
                {
                    int iSelectUser = int.Parse(selectUser);
                    queryUser = queryUser.Where(x => x.userID == iSelectUser);
                }
                if (selectRole != "")
                {
                    int iSelectRole = int.Parse(selectRole);
                    queryUser = queryUser.Where(x => x.roleID == iSelectRole);
                }

                users.userTableList = queryUser.ToList();
                return View("UserMaintenance", users);
            }
        }

        // Add user landing page.
        [HttpGet]
        public ActionResult AddUser()
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedRole roles = new ExtendedRole();
                roles.rolesList = db.Roles.Where(x => x.status == true).ToList();
                return View(roles);
            }
        }

        // Add user to database function. Hard code setting the default password as 'username12345'
        // where username is the username entered.
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
                string newPassword = username + "12345";
                using (MD5 md5Hash = MD5.Create())
                {
                    string hash = Encryption.GetMd5Hash(md5Hash, newPassword);
                    newAccount.password = hash;
                    db.Accounts.Add(newAccount); 
                }

                User addUser = new User();
                addUser.username = username;
                addUser.roleID = iRoleID;
                addUser.status = false;
                if (selectStatus == "1")
                {
                    addUser.status = true;
                }
                addUser.createDate = curretDateTime;
                addUser.createUser = sessionUser.username;
                addUser.modifyDate = curretDateTime;
                addUser.modifyUser = sessionUser.username;
                db.Users.Add(addUser);
                db.SaveChanges();

                ExtendedUser users = new ExtendedUser();
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.Where(x => x.status == true).ToList();
                users.userTableList = new List<User>();
                TempData["Message"] = "A new user has been successfully added!";
                return View("UserMaintenance", users);
            }
        }

        // Modify user landing page.
        [HttpPost]
        public ActionResult ModifyUser(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedUser viewModel = new ExtendedUser();
                viewModel.userList = db.Users.ToList();
                viewModel.roleList = db.Roles.Where(x => x.status == true).ToList();
                viewModel.userTableList = new List<User>();

                try
                {
                    // Checkbox is the attribute name. 
                    // Returns values of the boxes that have been checked as csv.
                    string[] checkboxes = form["checkbox"].Split(',');

                    if (checkboxes.Length != 1) // More than 1 checkbox selected.
                    {
                        TempData["Message"] = "Error: Can only select one from table to modify.";
                        return View("UserMaintenance", viewModel);
                    }
                    else
                    {
                        int iSelectUser = int.Parse(checkboxes[0]);
                        ExtendedUser user = new ExtendedUser();
                        user.user = db.Users.SingleOrDefault(x => x.userID == iSelectUser);
                        user.roleList = db.Roles.Where(x => x.status == true).ToList();
                        return View("ModifyUser", user);
                    }
                }
                catch (Exception e) // Catch exception when no item from table was selected.
                {
                    TempData["Message"] = "Error: No item was selected from table.";
                    return View("UserMaintenance", viewModel);
                }
            }
        }

        // Modify user in the database function.
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
                user.status = false;
                if (selectStatus == "1")
                {
                    user.status = true;
                }
                DateTime curretDateTime = DateTime.Now;
                user.modifyDate = curretDateTime;
                user.modifyUser = sessionUser.username;
                db.SaveChanges();

                ExtendedUser users = new ExtendedUser();
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.Where(x => x.status == true).ToList();
                users.userTableList = new List<User>();
                TempData["Message"] = "Role has been successfully modified!";
                return View("UserMaintenance", users);
            }
        }

        // Delete user(s) from database function.
        [HttpPost]
        public ActionResult DeleteUser(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedUser users = new ExtendedUser();
                try
                {
                    // Checkbox is the attribute name. 
                    // Returns values of the boxes that have been checked as csv.
                    string[] checkboxes = form["checkbox"].Split(',');

                    foreach (var user in checkboxes)
                    {
                        int iUserID = Int32.Parse(user);
                        User deletUser = db.Users.SingleOrDefault(x => x.userID == iUserID);
                        db.Users.Remove(deletUser);
                        db.SaveChanges();
                    }
                    users.userList = db.Users.ToList();
                    users.roleList = db.Roles.Where(x => x.status == true).ToList();
                    users.userTableList = new List<User>();
                    TempData["Message"] = "One or more user has been successfully deleted!";
                    return View("UserMaintenance", users);
                }
                catch (Exception e) // Catch exception when no item from table was selected.
                {
                    users.userList = db.Users.ToList();
                    users.roleList = db.Roles.Where(x => x.status == true).ToList();
                    users.userTableList = new List<User>();
                    TempData["Message"] = "Error: No item was selected from table.";
                    return View("UserMaintenance", users);
                }
            }
        }
    }
}