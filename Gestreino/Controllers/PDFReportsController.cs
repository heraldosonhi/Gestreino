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

           
            //var path = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.Personal),"Sample1.txt");
            var path = Path.Combine(Server.MapPath("~/"), string.Empty);
            var html = PDFReports.BodyMassReport(Id, path, string.Empty);

            var data = databaseManager.GT_Treino.Where(x => x.ID == Id).ToList();
               if (!data.Any()) return RedirectToAction("", "home");
               if() 

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
            ConverterProperties converterProperties = new ConverterProperties();

            // Create a Temp PDF file temporary, remove this when the whole invoice is created to manaqge it as stream byte[]
            string file = System.IO.Path.Combine("", $@"Documents\Temp_Invoice_.pdf");
            var html = PDFReports.BodyMassReport(0, string.Empty, string.Empty);


            using (System.IO.StreamReader reader = new System.IO.StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/BodyMassReport.html")))
            {
                //    html = reader.ReadToEnd();
            }
            // html = html.Replace("{inst_nome}", Configs.INST_INSTITUICAO_NOME);
            //   html = html.Replace("{inst_sigla}", Configs.INST_INSTITUICAO_SIGLA);



            var workStream = new MemoryStream();
            PdfWriter writer = new PdfWriter(workStream);//file
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.LEGAL);
            HtmlConverter.ConvertToPdf(html, pdf, converterProperties);
            //document.Close();

            //iText.Layout.Document pdfModifier = document;
            var bytearr = workStream.ToArray();

            var content = bytearr;
            return File(content, "application/pdf", "gestreinopdf.pdf");

        }
        public ActionResult Reports()
        {
            ConverterProperties converterProperties = new ConverterProperties();

            // Create a Temp PDF file temporary, remove this when the whole invoice is created to manaqge it as stream byte[]
            string file = System.IO.Path.Combine("", $@"Documents\Temp_Invoice_.pdf");
            var html = PDFReports.BodyMassReport(0, string.Empty, string.Empty);


            using (System.IO.StreamReader reader = new System.IO.StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/BodyMassReport.html")))
            {
                //    html = reader.ReadToEnd();
            }
            // html = html.Replace("{inst_nome}", Configs.INST_INSTITUICAO_NOME);
            //   html = html.Replace("{inst_sigla}", Configs.INST_INSTITUICAO_SIGLA);



            var workStream = new MemoryStream();
            PdfWriter writer = new PdfWriter(workStream);//file
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(iText.Kernel.Geom.PageSize.LEGAL);
            HtmlConverter.ConvertToPdf(html, pdf, converterProperties);
            //document.Close();

            //iText.Layout.Document pdfModifier = document;
            var bytearr = workStream.ToArray();

            var content = bytearr;
            return File(content, "application/pdf", "gestreinopdf.pdf");

        }
        

    }
}