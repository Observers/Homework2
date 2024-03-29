﻿using System.Web.Mvc;
using Homework2.Models;
using System.Linq;
using System.Collections.Generic;

namespace Homework2.Controllers
{
    public class SystemSetupController : Controller
    {
        // Menu Maintenance landing page.
        public ActionResult MenuMaintenance(ExtendedMenu menus)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                menus.menuList = db.Menus.ToList();
                menus.menuTableList = new List<Menu>();
                return View("../MenuMaintenance/MenuMaintenance", menus);
            }
        }

        // Role Maintenance landing page.
        public ActionResult RoleMaintenance(ExtendedRole roles)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                roles.rolesList = db.Roles.ToList();
                List<Role> list = new List<Role>();
                roles.rolesTableList = list;
                return View("../RoleMaintenance/RoleMaintenance", roles);
            }
        }

        // User Maintenance landing page.
        public ActionResult UserMaintenance(ExtendedUser users)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                users.userList = db.Users.ToList();
                users.roleList = db.Roles.ToList();
                users.userTableList = new List<User>();
                return View("../UserMaintenance/UserMaintenance", users);
            }
        }
    }
}
