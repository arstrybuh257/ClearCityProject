using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClearCity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //ExcelHelper.Test();
            return View();
        }

    }
}