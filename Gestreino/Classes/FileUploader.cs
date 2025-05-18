using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Gestreino.Classes
{
    public static class FileUploader
    {
        public static string FileStorage = "~/uploads/";
        public static string SQLStorage = "uploads/";
        public static string[] ModuleStorage = { "adm","pes", "gt" };
        public static string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc", ".docx", "odt", ".xls", ".xlsx" };
        static readonly string[] suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };

        public static int OneMB = 1097152; 
        public static int TwoMB = 2097152; 
        public static int FourMB = 4097152; 
        public static int EightMB = 8097152; 
        public static int ThirtyMB = 30097152; 
        public static int FiftyMB = 50097152; 

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

        public static string[] DirectoryFactory(string modulestorage, string absolutepath, /*HttpPostedFileBase file*/string FileExtension, string tipodoc, string nomedoc)
        {
            var uploadpath = modulestorage + "/" + DateTime.Now.ToString("MMyyyy") + "/";
            var sqlpath = SQLStorage + uploadpath;

            absolutepath = absolutepath + uploadpath;

            if (!Directory.Exists(absolutepath))
                Directory.CreateDirectory(absolutepath);

            var extension = FileExtension;
            var filename = nomedoc + "_" + Guid.NewGuid() + extension;

            absolutepath = System.IO.Path.Combine(absolutepath, filename);
            sqlpath = System.IO.Path.Combine(sqlpath, filename);

            return new[] { sqlpath, absolutepath, filename };
        }

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

        public static string[] DecoderFactory(string entityname)
        {
            List<string> names = new List<string>();

            switch (entityname)
            {
                case "institution":
                    names.Add("INST_APLICACAO_ARQUIVOS"); 
                    names.Add("INST_APLICACAO_ID"); 
                    names.Add("0"); 
                    break;
                case "pespessoas":
                    names.Add("PES_ARQUIVOS"); 
                    names.Add("PES_PESSOAS_ID");
                    names.Add("1"); 
                    break;
                case "gtexercicios":
                    names.Add("GT_Exercicio_ARQUIVOS"); 
                    names.Add("GT_Exercicio_ID"); 
                    names.Add("2"); 
                    break;
            }
            return names.ToArray();
        }

        public class FileUploadModel
        {
            [Display(Name = "Nome")]
            [StringLength(255, ErrorMessage = "{0} Permite apenas {1} caracteres")]
            [DataType(DataType.Text)]
            public string Nome { get; set; }

            [AllowHtml]
            [Display(Name = "Descrição")]
            [StringLength(255, ErrorMessage = "{0} Permite apenas {1} caracteres")]
            [DataType(DataType.Text)]
            public string Descricao { get; set; }

            [Display(Name = "Estado")]
            [DataType(DataType.Text)]
            public string Status { get; set; }

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

            public int? TipoDocId { get; set; }
            public IEnumerable<System.Web.Mvc.SelectListItem> TipoDocList { get; set; }

            public int EntityID { get; set; }

            [Required]
            public string EntityName { get; set; }
            public int ID { get; set; }
        }

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