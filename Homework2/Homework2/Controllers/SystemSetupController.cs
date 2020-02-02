﻿// TODO: Sort tables.
// TODO: Add materialize breadcrumb to pages.

// TODO: Check all forms have required field where neccessary.
// TODO: Add comments.
// TODO: Change buttons to follow prototype.
// TODO: Split SystemSetup Controller
// TODO: Grey out GSYSADM

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
        public ActionResult RoleMaintenance(ExtendedRole viewModel)
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
                System.Diagnostics.Debug.WriteLine("-------> RoleID: "+roleID);
                string[] checkboxes = form["checkbox"].Split(',');
                var menus = db.Menus.ToList();
                var role = db.Roles.SingleOrDefault(x => x.roleID == roleID);
                foreach (var menu in menus)
                {
                    if(checkboxes.Contains(menu.menuID.ToString()))
                    {
                        if(!menu.Roles.Contains(role))
                        {
                            db.Menus.SingleOrDefault(x => x.menuID == menu.menuID).Roles.Add(role);
                            db.SaveChanges();
                        }
                    }
                    if(!checkboxes.Contains(menu.menuID.ToString()))
                    {
                        if(menu.Roles.Contains(role))
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
            using(Trainee15Entities db = new Trainee15Entities())
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
                if(selectStatus == "1")
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
                ExtendedUser users = new ExtendedUser();
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.ToList();
                users.userTableList = new List<User>();
                TempData["Message"] = "<script>alert('One or more user has been successfully deleted!')</script>";
                return View("UserMaintenance", users);
            }
        }

        public ActionResult MenuMaintenance(ExtendedMenu menu)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                menu.menuList = db.Menus.ToList();
                menu.menuTableList = new List<Menu>();
                return View(menu);
            }
        }

        [HttpPost]
        public ActionResult QueryMenu(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var menuNo = form["menuNo"];
                var title = form["title"];
                var linkType = form["linkType"];
                var status = form["status"];
                bool bStatus = false;
                if (status == "1")
                {
                    bStatus = true;
                }

                var queryMenu = db.Menus.AsQueryable();
                if(menuNo != null)
                {
                    queryMenu = queryMenu.Where(x => x.menuNo == menuNo);
                }
                if (title != null)
                {
                    queryMenu = queryMenu.Where(x => x.title == title);
                }
                if (linkType != null)
                {
                    queryMenu = queryMenu.Where(x => x.linkType == linkType);
                }
                if (status != null)
                {
                    queryMenu = queryMenu.Where(x => x.status == bStatus);
                }

                ExtendedMenu query = new ExtendedMenu();
                query.menuList = db.Menus.ToList();
                query.menuTableList = queryMenu.ToList();

                return View("MenuMaintenance", query);
            }
        }

        [HttpGet]
        public ActionResult AddMenu()
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedMenu menu = new ExtendedMenu();
                menu.menuList = db.Menus.SqlQuery("SELECT * FROM Menu m1 INNER JOIN Menu m2 ON m1.menuID = m2.menuID WHERE m1.menuNo LIKE '00%' AND LEN(m2.MenuNo) = 3").ToList();
                return View(menu);
            }
        }

        [HttpPost]
        public ActionResult AddMenu(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var level = form["selectLinkType"];
                var title = form["title"];
                var linkType = form["selectLinkType"];
                var subMenu = form["selectSubMenu"];
                var linkUrl = form["linkUrl"];
                var status = form["selectStatus"];
                ExtendedMenu menu = new ExtendedMenu();

                string menuNo = "00";
               
                int iEndNo = 0;
                if (linkType == "1")
                {
                    string temp = "SELECT * FROM Menu WHERE menuNo LIKE '" + menuNo + "[0-9]' ORDER BY menuNo DESC";
                    var endNo = db.Menus.SqlQuery(temp).ToList();
                    if (endNo != null)
                    {
                        Menu menu1 = endNo.First();
                        iEndNo = int.Parse(menu1.menuNo);
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                    else
                    {
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                    linkType = "Menu";
                }
                else
                {
                    string temp = "SELECT * FROM Menu WHERE menuNo LIKE '" + subMenu + "[0-9]' ORDER BY menuNo DESC";
                    var endNo = db.Menus.SqlQuery(temp).ToList();
                    if (endNo != null)
                    {
                        Menu menu1 = endNo.First();
                        iEndNo = int.Parse(menu1.menuNo);
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                    else
                    {
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                    linkType = "Program";
                }

                menu.menuNo = menuNo;
                menu.level = int.Parse(level);
                menu.title = title;
                menu.linkType = linkType;
                menu.linkUrl = linkUrl;
                if(status == "1")
                {
                    menu.status = true;
                }
                else
                {
                    menu.status = false;
                }
                db.Menus.Add(menu);
                db.SaveChanges();

                menu.menuList = db.Menus.ToList();
                menu.menuTableList = new List<Menu>();
                return View("MenuMaintenance", menu);
            }
        }

        [HttpPost]
        public ActionResult ModifyMenu(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                string[] checkboxes = form["checkbox"].Split(',');
                ExtendedMenu modify = new ExtendedMenu();

                if(checkboxes.Length != 1)
                {
                    modify.menuList = db.Menus.ToList();
                    modify.menuTableList = new List<Menu>();
                    TempData["Message"] = "Can only select one from table to modify.";
                    return View("MenuMaintenance", modify);
                }
                else
                {
                    int iMenuID = int.Parse(checkboxes[0]);
                    modify.menu = db.Menus.SingleOrDefault(x => x.menuID == iMenuID);
                    modify.menuList = db.Menus.SqlQuery("SELECT * FROM Menu m1 INNER JOIN Menu m2 ON m1.menuID = m2.menuID WHERE m1.menuNo LIKE '00%' AND LEN(m2.MenuNo) = 3").ToList();
                    return View("ModifyMenu", modify);
                }
            }
        }

        public ActionResult ModifyMenuDatabase(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedMenu menu = new ExtendedMenu();
                System.Diagnostics.Debug.WriteLine("ID:"+form["menuID"]);

                int menuID = int.Parse(form["menuID"]);
                menu.menu = db.Menus.SingleOrDefault(x => x.menuID == menuID);
                if (form["selectLinkType"] == "1")
                {
                    if(menu.linkType != "Menu")
                    {
                        menu.linkType = "Menu";
                        menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                    }
                }
                else
                {
                    if (form["selectLinkType"] == "2")
                    {
                        if(menu.linkType != "Program")
                        {
                            menu.linkType = "Program";
                            menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                        }
                        else
                        {
                            if(string.CompareOrdinal(menu.menuNo, 0, form["selectSubMenu"], 0, 3) != 0)
                            {
                                menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                            }
                        }
                    }
                }
                menu.level = int.Parse(form["selectLinkType"]);
                menu.title = form["title"];
                menu.linkUrl = form["linkUrl"];
                menu.status = bool.Parse(form["status"]);
                db.SaveChanges();

                menu.menuList = db.Menus.ToList();
                menu.menuTableList = new List<Menu>();
                return View("MenuMaitenance", menu);
            }
        }

        public string MenuNo(string linkType, string subMenu)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                string menuNo = "00";

                int iEndNo = 0;
                if (linkType == "1")
                {
                    string temp = "SELECT * FROM Menu WHERE menuNo LIKE '" + menuNo + "[0-9]' ORDER BY menuNo DESC";
                    var endNo = db.Menus.SqlQuery(temp).ToList();
                    if (endNo != null)
                    {
                        Menu menu1 = endNo.First();
                        iEndNo = int.Parse(menu1.menuNo);
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                    else
                    {
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                }
                else
                {
                    string temp = "SELECT * FROM Menu WHERE menuNo LIKE '" + subMenu + "[0-9]' ORDER BY menuNo DESC";
                    var endNo = db.Menus.SqlQuery(temp).ToList();
                    if (endNo != null)
                    {
                        Menu menu1 = endNo.First();
                        iEndNo = int.Parse(menu1.menuNo);
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                    else
                    {
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                }
                return menuNo;
            }
        }

        [HttpPost]
        public ActionResult DeleteMenu(FormCollection form)
        {
            using(Trainee15Entities db = new Trainee15Entities())
            {
                string[] checkboxes = form["checkbox"].Split(',');
                foreach (var menuID in checkboxes)
                {
                    int iMenuID = Int32.Parse(menuID);
                    Menu deleteMenu = db.Menus.SingleOrDefault(x => x.menuID == iMenuID);
                    db.Menus.Remove(deleteMenu);
                    db.SaveChanges();
                }
                ExtendedMenu menu = new ExtendedMenu();
                menu.menuList = db.Menus.ToList();
                menu.menuTableList = new List<Menu>();
                TempData["Message"] = "<script>alert('One or more user has been successfully deleted!')</script>";
                return View("MenuMaintenance", menu);
            }
        }
    }
}
