﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Homework2.Controllers
{
    public class HomeController : Controller
    {
        // Home landing page function
        public ActionResult Index()
        {
            return View();
        }
    }
}