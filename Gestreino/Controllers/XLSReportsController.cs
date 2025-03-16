using ClosedXML.Excel;
using Gestreino.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Gestreino.Controllers
{
    public class XLSReportsController : Controller
    {
        // GET: XLSReports
        public ActionResult ExportToExcel(string section,string applyFilter)
        {
            try
            {
                XLSReports xls = new XLSReports();
                IList queryfilter = TempData["QUERYRESULT"] as IList;
                IList query = TempData["QUERYRESULT_ALL"] as IList;

                bool filter = Convert.ToBoolean(applyFilter);
                query = filter == true ? queryfilter : query;
                var bytearr = xls.ExportToExcel(section, query);

                var content = bytearr;
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "gestreino_xlsreports_"+section+".xlsx");
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}