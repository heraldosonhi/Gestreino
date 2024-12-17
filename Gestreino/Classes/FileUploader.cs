using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;
using Gestreino.Classes;

namespace Gestreino.Classes
{
    public static class FileUploader
    {
        /*
          * Web.Config
          * 
          * Allow Server Content Processing under 1.0 GB maxAllowedContentLength=1073741824 in Bytes
          * <requestLimits maxAllowedContentLength="1073741824" />
          * 
          * Allow Server Content Processing under 1.0 GB maxRequestLength=8589934 in KiloBytes
          * <httpRuntime targetFramework="4.5.1" maxRequestLength="8589934" />
          *   
          * Allow Form Content processing under 50 MB : value="50485000" in Bytes 
          * appSettings => <add key="maxRequestLength" value="50485000" />
          * 
          */

        // Define Main Storage location
        public static string FileStorage = "~/uploads/";
        public static string SQLStorage = "uploads/";

        // Define Module Subfolder
        /*
         * 0 => ADM,
         * 1 => GA,
         * 2 => GP,
         * 3 => GPAG
         * 4 => PA
         */
        public static string[] ModuleStorage = { "adm", "ga", "gp", "gpag", "pa" };

        // Define Int Sizes
        public static int OneMB = 1097152; //1MB 
        public static int TwoMB = 2097152; //2MB 
        public static int FourMB = 4097152; //4MB 
        public static int EightMB = 8097152; //8MB 
        public static int ThirtyMB = 30097152; //30MB 
        public static int FiftyMB = 50097152; //50MB 

        // Define Acceptable File Types
        public static string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc", ".docx", "odt", ".xls", ".xlsx"};

        // Define Size Suffixes
        static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        // Convert Byte Sizes
        public static string FormatSize(Int64 bytes)
        {
            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1} {1}", number, suffixes[counter]);
        }
        // Return font-awesome icon 
        public static string FontIconType(string filetype)
        {
            var fa = "fa-file";

            switch (filetype)
            {
                case ".jpg":
                    fa = "fa-file-image"; break;
                case ".jpeg":
                    fa = "fa-file-image"; break;
                case ".png":
                    fa = "fa-file-image"; break;
                case ".gif":
                    fa = "fa-file-image"; break;
                case ".pdf":
                    fa = "fa-file-pdf"; break;
                case ".doc":
                    fa = "fa-file-word"; break;
                case ".docx":
                    fa = "fa-file-word"; break;
                case ".xls":
                    fa = "fa-file-excel"; break;
                case ".xlsx":
                    fa = "fa-file-excel"; break;
            }
            return fa;
        }

        // Directory Factory
        public static string[] DirectoryFactory(string modulestorage, string absolutepath, /*HttpPostedFileBase file*/string FileExtension, string tipodoc, string nomedoc)
        {
            // Define SQL path
            var uploadpath = modulestorage + "/" + DateTime.Now.ToString("MMyyyy") + "/";
            var sqlpath = SQLStorage + uploadpath;
            // Define Absolute path [Upload Directory]
            absolutepath = absolutepath + uploadpath;
            // Get uploaded filename
            //var _filename = System.IO.Path.GetFileName(file.FileName);
            // Create Directory
            if (!Directory.Exists(absolutepath))
                Directory.CreateDirectory(absolutepath);
            // Initialize absolute path
            var extension = FileExtension; //Path.GetExtension(file.FileName);
            // Rename file
            //var filename = tipodoc + "_" + Guid.NewGuid() + extension;
            //change back
            //nomedoc = nomedoc.Replace(" ", "_").ToLower().RemoveDiacritics();
            //nomedoc = nomedoc.RemoveSpecialCharacters();

            var filename = nomedoc + "_" + Guid.NewGuid() + extension;

            // Get full path with filename
            absolutepath = System.IO.Path.Combine(absolutepath, filename);
            sqlpath = System.IO.Path.Combine(sqlpath, filename);
            // Return path
            return new[] { sqlpath, absolutepath, filename };
        }

        // Delete File
        public static bool DeleteFile(string absolutepath)
        {
            bool success = false;
            if ((System.IO.File.Exists(absolutepath)))
            {
                System.IO.File.Delete(absolutepath);
                success = true;
            }
            return success;
        }

        // Get Table and Field Name
        public static string[] DecoderFactory(string entityname)
        {
            List<string> names = new List<string>();

            /*
              * 0 => Tablename,
              * 1 => Fieldname,
              * 2 => Directoryname [FileUploader.ModuleStorage]    
             */

            switch (entityname)
            {
                case "institution":
                    names.Add("INST_INSTITUICAO_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("INST_INSTITUICAO_ID"); // SECOND IN ARRAY!
                    names.Add("0"); // ModuleStorage
                    break;
                case "institutioncampus":
                    names.Add("INST_CAMPUS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("INST_CAMPUS_ID"); // SECOND IN ARRAY!
                    names.Add("0"); // ModuleStorage
                    break;
                case "institutionbuilding":
                    names.Add("INST_EDIFICIOS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("INST_EDIFICIOS_ID"); // SECOND IN ARRAY!
                    names.Add("0"); // ModuleStorage
                    break;
                case "institutionfloor":
                    names.Add("INST_PISOS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("INST_PISOS_ID"); // SECOND IN ARRAY!
                    names.Add("0"); // ModuleStorage
                    break;
                case "institutionspace":
                    names.Add("INST_IESPACOS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("INST_ESPACOS_ID"); // SECOND IN ARRAY!
                    names.Add("0"); // ModuleStorage
                    break;
                case "gaprocess":
                    names.Add("CAND_PROCESSOS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("CAND_PROCESSOS_ID"); // SECOND IN ARRAY!
                    names.Add("1"); // ModuleStorage
                    break;
                case "pespessoas":
                    names.Add("PES_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("PES_PESSOAS_ID"); // SECOND IN ARRAY!
                    names.Add("2"); // ModuleStorage
                    break;
                case "candidaturas":
                    names.Add("CAND_CANDIDATURAS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("CAND_CANDIDATURAS_ID"); // SECOND IN ARRAY!
                    names.Add("1"); // ModuleStorage
                    break;
                case "gacourses":
                    names.Add("GA_CURSOS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("GA_CURSOS_ID"); // SECOND IN ARRAY!
                    names.Add("1"); // ModuleStorage
                    break;
                case "gastudyplans":
                    names.Add("GA_PLANOS_ESTUDOS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("GA_PLANOS_ESTUDOS_ID"); // SECOND IN ARRAY!
                    names.Add("1"); // ModuleStorage
                    break;
                case "gastudents":
                    names.Add("GA_ALUNOS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("GA_ALUNOS_ID"); // SECOND IN ARRAY!
                    names.Add("1"); // ModuleStorage
                    break;
                case "gaequivalencias":
                    names.Add("GA_ALUNOS_PROCESSOS_EQUIVALENCIAS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("GA_ALUNOS_PROCESSOS_EQUIVALENCIAS_ID"); // SECOND IN ARRAY!
                    names.Add("1"); // ModuleStorage
                    break;
                case "checkingaccount":
                    names.Add("GPAG_CONTA_CORRENTE_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("GPAG_CONTA_CORRENTE_ID"); // SECOND IN ARRAY!
                    names.Add("3"); // ModuleStorage
                    break;
                case "portal":
                    names.Add("PORTAL_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("PORTAL_ID"); // SECOND IN ARRAY!
                    names.Add("4"); // ModuleStorage
                    break;
                case "gaannouncements":
                    names.Add("PORTAL_COMUNICADOS_ARQUIVOS"); // FIRST IN ARRAY!
                    names.Add("PORTAL_COMUNICADOS_ID"); // SECOND IN ARRAY!
                    names.Add("4"); // ModuleStorage
                    break;
            }
            return names.ToArray();
        }

        public class FileUploadModel
        {
            //[Required]
            [Display(Name = "Nome")]
            [StringLength(255, ErrorMessage = "{0} Permite apenas {1} caracteres")]
            [DataType(DataType.Text)]
            public string Nome { get; set; }

            [AllowHtml]
            //[Required]
            [Display(Name = "Descrição")]
            [StringLength(255, ErrorMessage = "{0} Permite apenas {1} caracteres")]
            [DataType(DataType.Text)]
            public string Descricao { get; set; }

            //[Required]
            [Display(Name = "Estado")]
            [DataType(DataType.Text)]
            public string Status { get; set; }

            //[Required]
            [Display(Name = "Data Activação")]
            [DataType(DataType.Text)]
            public string DateAct { get; set; }

            [Display(Name = "Data Desactivação")]
            [DataType(DataType.Text)]
            public string DateDisact { get; set; }

            [Display(Name = "Agendar Activação")]
            public bool ScheduledStatus { get; set; }

            [Display(Name = "Agendado")]
            public string StatusPending { get; set; }

            [Display(Name = "Documento")]
            public string TipoDoc { get; set; }

            // Dropdown list
            //[Required]
            public int? TipoDocId { get; set; } //public int?
            public IEnumerable<System.Web.Mvc.SelectListItem> TipoDocList { get; set; }

            //[Required]
            public int EntityID { get; set; }

            [Required]
            public string EntityName { get; set; }
            public int ID { get; set; }
        }


        //Fancytree
        public class FileFancyModel
        {
            public string title { get; set; }
            public string key { get; set; }
            public bool folder { get; set; }
            public List<ChildrenFancyModel> children { get; set; }

            public FileFancyModel()
            {
                children = new List<ChildrenFancyModel>();
            }
        }

        
        public class FileFancyInfoModel
        {
            public int FileId { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
        }

        public class ChildrenFancyModel
        {
            public string title { get; set; }
            public string key { get; set; }
            public string path { get; set; }
            public string arquivo { get; set; }
            public string type { get; set; }
        }

        // Get Month
        public static string DecodeMonth(int month)
        {
            string monthname = string.Empty;
            switch (month)
            {
                case 1: monthname = "Janeiro"; break;
                case 2: monthname = "Fevereiro"; break;
                case 3: monthname = "Março"; break;
                case 4: monthname = "Abril"; break;
                case 5: monthname = "Maio"; break;
                case 6: monthname = "Junho"; break;
                case 7: monthname = "Julho"; break;
                case 8: monthname = "Agosto"; break;
                case 9: monthname = "Setembro"; break;
                case 10: monthname = "Outubro"; break;
                case 11: monthname = "Novembro"; break;
                case 12: monthname = "Dezembro"; break;
            }
            return monthname;
        }
    }
}