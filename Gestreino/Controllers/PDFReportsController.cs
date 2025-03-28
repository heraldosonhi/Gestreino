using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;
using iText.StyledXmlParser.Jsoup.Nodes;
using DocumentFormat.OpenXml.EMMA;
//using System.Web.Http.Results;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Windows;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.Web.Services.Description;
using iText.Kernel.Pdf.Layer;
//using Org.BouncyCastle.Ocsp;
using System.Web.UI;
using DocumentFormat.OpenXml.Bibliography;
using static ClosedXML.Excel.XLPredefinedFormat;
using System.Data.Entity;
using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel;
using System.Collections;
using Gestreino.Classes;
using Gestreino;
using iText.Html2pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Policy;



namespace Gestreino.Controllers
{
    public class PDFReportsController : Controller
    {
        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();


        // GET: PDFReports
        public ActionResult BodyMass(int? Id)
        {
            if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }
            
            var data = databaseManager.GT_Treino.Where(x => x.ID == Id).ToList();
               if (!data.Any()) return RedirectToAction("", "home");
               if(data.Select(x=>x.GT_TipoTreino_ID).FirstOrDefault()!=Configs.GT_EXERCISE_TYPE_BODYMASS)
                return RedirectToAction("", "home");

            var path = Path.Combine(Server.MapPath("~/"), string.Empty);
            var html = PDFReports.BodyMassPrintReport(Id, data, path, string.Empty);

            var workStream = new MemoryStream();
            PdfWriter writer = new PdfWriter(workStream);//file
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.LEGAL);
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(html, pdf, converterProperties);

            var bytearr = workStream.ToArray();
            var content = bytearr;
            return File(content, "application/pdf", "gestreinoplanomusculacao"+Id+".pdf");
        }

        public ActionResult Cardio(int? Id)
        {
            var data = databaseManager.GT_Treino.Where(x => x.ID == Id).ToList();
            if (!data.Any()) return RedirectToAction("", "home");
            if (data.Select(x => x.GT_TipoTreino_ID).FirstOrDefault() != Configs.GT_EXERCISE_TYPE_CARDIO)
                return RedirectToAction("", "home");

            var path = Path.Combine(Server.MapPath("~/"), string.Empty);
            var html = PDFReports.CardioPrintReport(Id, data, path, string.Empty);

            var workStream = new MemoryStream();
            PdfWriter writer = new PdfWriter(workStream);//file
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4.Rotate());
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(html, pdf, converterProperties);

            var bytearr = workStream.ToArray();
            var content = bytearr;
            return File(content, "application/pdf", "gestreinoplanocardio" + Id + ".pdf");
        }
        public ActionResult MainReport()
        {
            var PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;
            if(PEsId==null || PEsId==0) return RedirectToAction("", "home");

            var path = Path.Combine(Server.MapPath("~/"), string.Empty);
            var html = PDFReports.MainReportPrint(PEsId, path, string.Empty);

            var workStream = new MemoryStream();
            PdfWriter writer = new PdfWriter(workStream);//file
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.A4);
            ConverterProperties converterProperties = new ConverterProperties();
            HtmlConverter.ConvertToPdf(html, pdf, converterProperties);

            var bytearr = workStream.ToArray();
            var content = bytearr;
            return File(content, "application/pdf", "relatoriogestreino" + PEsId + ".pdf");
        }
        

    }
}