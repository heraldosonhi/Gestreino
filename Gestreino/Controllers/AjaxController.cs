using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestreino.Controllers
{
    public class AjaxController : Controller
    {
        // GET: Ajax
        public ActionResult Users(Gestreino.Models.Users MODEL)
        {



            return View("GTManagement/Users",MODEL);
        }
    }
}