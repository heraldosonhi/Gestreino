using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestreino.Controllers
{
    [Authorize]
    public class GTManagementController : Controller
    {
        // GET: GTManagement
        public ActionResult Index()
        {
            return View();
        }

        // GET: GTManagement
        public ActionResult Athletes()
        {
            return View("Athletes/Index");
        }
        
    }
}