using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework2.Models;

namespace Homework2.Controllers
{
    public class MenuMaintenanceController : Controller
    {
        public ActionResult MenuMaintenance(ExtendedMenu menus)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                menus.menuList = db.Menus.ToList();
                menus.menuTableList = new List<Menu>();
                return View(menus);
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
                if (status == "1")
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

                if (checkboxes.Length != 1)
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
                System.Diagnostics.Debug.WriteLine("ID:" + form["menuID"]);

                int menuID = int.Parse(form["menuID"]);
                menu.menu = db.Menus.SingleOrDefault(x => x.menuID == menuID);
                if (form["selectLinkType"] == "1")
                {
                    if (menu.linkType != "Menu")
                    {
                        menu.linkType = "Menu";
                        menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                    }
                }
                else
                {
                    if (form["selectLinkType"] == "2")
                    {
                        if (menu.linkType != "Program")
                        {
                            menu.linkType = "Program";
                            menu.menuNo = MenuNo(form["selectLinkType"], form["selectSubMenu"]);
                        }
                        else
                        {
                            if (string.CompareOrdinal(menu.menuNo, 0, form["selectSubMenu"], 0, 3) != 0)
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
            using (Trainee15Entities db = new Trainee15Entities())
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