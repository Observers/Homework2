using Homework2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Homework2.Controllers
{
    public class MenuMaintenanceController : Controller
    {
        // Menu Maintenance landing page.
        public ActionResult MenuMaintenance(ExtendedMenu menus)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                // Query database first to get data for dropdown list.
                menus.menuList = db.Menus.ToList();
                menus.menuTableList = new List<Menu>();
                return View(menus);
            }
        }

        // Querying the menu table.
        [HttpPost]
        public ActionResult QueryMenu(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                var menuNo = form["menuNo"];
                var title = form["title"];
                var linkType = form["linkType"];
                // Status is of type string from front-end.
                // bStatus is used to check database.
                var status = form["status"];

                var queryMenu = db.Menus.AsQueryable();
                if (menuNo != null)
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
                    queryMenu = queryMenu.Where(x => x.status == bool.Parse(status));
                }

                ExtendedMenu query = new ExtendedMenu();
                query.menuList = db.Menus.ToList();
                query.menuTableList = queryMenu.ToList();

                return View("MenuMaintenance", query);
            }
        }

        // Add a new menu landing page.
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

        // Add a new menu function.
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
                // Determine the new menu number.
                if (linkType == "1")
                {
                    menu.linkType = "Menu";
                    // Get new main menu number.
                    menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                }

                if (linkType == "2")
                {
                    if (subMenu != null)
                    {
                        menu.linkType = "Program";
                        // Get new sub menu number.
                        menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                    }
                    else
                    {
                        TempData["Message"] = "Error: Sub-menu category must be selected.";
                        return View("MenuMaintenance", "SystemSetup");
                    }
                }

                menu.level = int.Parse(level);
                menu.title = title;

                if (linkUrl != null) // Optional
                {
                    menu.linkUrl = linkUrl;
                }
                else
                {
                    menu.linkUrl = "";
                }

                menu.status = bool.Parse(status);
                db.Menus.Add(menu);
                db.SaveChanges();

                menu.menuList = db.Menus.ToList();
                menu.menuTableList = new List<Menu>();
                return View("MenuMaintenance", menu);
            }
        }


        // Modify menu landing page.
        [HttpPost]
        public ActionResult ModifyMenu(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedMenu modify = new ExtendedMenu();
                modify.menuList = db.Menus.ToList();
                modify.menuTableList = new List<Menu>();

                try
                {
                    // Checkbox is the attribute name. 
                    // Returns values of the boxes that have been checked as csv.
                    string[] checkboxes = form["checkbox"].Split(',');

                    if (checkboxes.Length != 1) // More than 1 checkbox selected.
                    {
                        TempData["Message"] = "Error: Can only select one from table to modify.";
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
                catch (Exception e) // Catch exception when no item from table was selected.
                {
                    TempData["Message"] = "Error: No itemm was selected from table.";
                    return View("MenuMaintenance", modify);
                }
            }
        }

        // To modify and update the menu table.
        public ActionResult ModifyMenuDatabase(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                // Menu object to store and update database.
                ExtendedMenu menu = new ExtendedMenu();

                int menuID = int.Parse(form["menuID"]);
               // Get data from database and assign it to object.
                menu.menu = db.Menus.SingleOrDefault(x => x.menuID == menuID);

                if (form["selectLinkType"] == "1") //Main Menu
                {
                    if (menu.linkType != "Menu") // Checks if menu isn't already a menu.
                    {
                        menu.linkType = "Menu";
                        // Update menu number if changed from sub menu to main menu.
                        menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                    }
                }
                if (form["selectLinkType"] == "2") // Sub Menu
                {
                    if (form["selectSubMenu"] != null)
                    {
                        if (menu.linkType != "Program") // Checks if menu isn't already sub menu.
                        {
                            menu.linkType = "Program";
                            // Update menu number if changed from main menu to sub menu.
                            menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                        }
                        else // If the menu is already a program.
                        {
                            // Compare the menu category from database to the selected menu category.
                            if (string.CompareOrdinal(menu.menuNo, 0, form["selectSubMenu"], 0, 3) != 0)
                            {
                                // Update menu number if menu changed to a different menu category.
                                menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                            }
                        }
                    }
                    else
                    {
                        menu.menuList = db.Menus.ToList();
                        menu.menuTableList = new List<Menu>();
                        TempData["Message"] = "Error: Sub-menu category was not selected.";
                        return View("MenuMaitenance", menu);
                    }
                }

                menu.level = int.Parse(form["selectLinkType"]);
                menu.title = form["title"];
                if (form["linkUrl"] != null) // Optional
                {
                    menu.linkUrl = form["linkUrl"]; 
                }
                else
                {
                    menu.linkUrl = "";
                }
                menu.status = bool.Parse(form["status"]);
                db.SaveChanges();

                menu.menuList = db.Menus.ToList();
                menu.menuTableList = new List<Menu>();
                return View("MenuMaitenance", menu);
            }
        }

        //This function identifies the next menu number for the new added menu.
        //Note: This only works when main menu does not exceed 9 'Menu' pages.
        //      Each sub - menu does not exceed 9 'Program' pages
        public string MenuNo(string linkType, string subMenu)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                string menuNo = "00";

                int iEndNo = 0;
                if (linkType == "1") // Main Menu
                {
                    // SQL selecting all menu where the menuNo is 3 digits from 000 - 009 order number by
                    // biggest to smallest.
                    string temp = "SELECT * FROM Menu WHERE menuNo LIKE '" + menuNo + "[0-9]' ORDER BY menuNo DESC";
                    var endNo = db.Menus.SqlQuery(temp).ToList();
                    if (endNo != null)
                    {
                        // Gets the first menu from the list.
                        Menu menu1 = endNo.First();
                        // Gets the menuNo.
                        iEndNo = int.Parse(menu1.menuNo);
                        // Increase the endNo.
                        iEndNo++;
                        // Creates the new menuNo string.
                        menuNo += iEndNo;
                    }
                    else // If it's the first main menu to be added.
                    {
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                }
                else // Sub-menu
                {
                    // SQL selecting all menu where the menuNo is 3 digits from 0000 - 0009 order number by
                    // biggest to smallest.
                    string temp = "SELECT * FROM Menu WHERE menuNo LIKE '" + subMenu + "[0-9]' ORDER BY menuNo DESC";
                    var endNo = db.Menus.SqlQuery(temp).ToList();
                    if (endNo != null)
                    {
                        Menu menu1 = endNo.First();
                        iEndNo = int.Parse(menu1.menuNo);
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                    else // First sub-menu added to main-menu.
                    {
                        iEndNo++;
                        menuNo += iEndNo;
                    }
                }
                return menuNo;
            }
        }

        // Deleting menu(s) from database function.
        [HttpPost]
        public ActionResult DeleteMenu(FormCollection form)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                ExtendedMenu menu = new ExtendedMenu();

                try
                {
                    // Checkbox is the attribute name. 
                    // Returns values of the boxes that have been checked as csv.
                    string[] checkboxes = form["checkbox"].Split(',');
                    foreach (var menuID in checkboxes)
                    {
                        int iMenuID = Int32.Parse(menuID);
                        Menu deleteMenu = db.Menus.SingleOrDefault(x => x.menuID == iMenuID);
                        db.Menus.Remove(deleteMenu);
                        db.SaveChanges();
                    }
                    menu.menuList = db.Menus.ToList();
                    menu.menuTableList = new List<Menu>();
                    TempData["Message"] = "<script>alert('One or more user has been successfully deleted!')</script>";
                    return View("MenuMaintenance", menu);
                }
                catch (Exception e) // Catch exception when no item from table was selected.
                {
                    menu.menuList = db.Menus.ToList();
                    menu.menuTableList = new List<Menu>();
                    TempData["Message"] = "Error: No itemm was selected from table.";
                    return View("MenuMaintenance", menu);
                }
            }
        }
    }
}