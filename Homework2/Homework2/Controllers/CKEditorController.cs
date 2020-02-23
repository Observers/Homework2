using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Homework2.Models;

namespace Homework2.Controllers
{
    public class CKEditorController : Controller
    {
        // GET: CKEditor
        public ActionResult Editor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(string encodedHtml)
        {
            using (Trainee15Entities db = new Trainee15Entities())
            {
                System.Diagnostics.Debug.WriteLine("--> " + encodedHtml);
                string myDecodedString = Uri.UnescapeDataString(encodedHtml);
                System.Diagnostics.Debug.WriteLine("--> " + myDecodedString);

                var userID = (int)Session["userID"];
                Home editor = new Home();
                if (db.Homes.SingleOrDefault(x => x.userID == userID) != null)
                {
                    editor = db.Homes.SingleOrDefault(x => x.userID == userID);
                    editor.home1 = myDecodedString;
                }
                else
                {
                    editor.userID = userID;
                    editor.home1 = myDecodedString;
                    db.Homes.Add(editor);
                }
                db.SaveChanges();

                return Json(new { newUrl = Url.Action("Index", "Home") }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}