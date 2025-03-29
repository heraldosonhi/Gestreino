using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestreino.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                 return RedirectToAction("login", "account");
            return View();
        }
        [AllowAnonymous]
        public ActionResult Help()
        {
            return View();
        }
    }
}