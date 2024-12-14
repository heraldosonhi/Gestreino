using Antlr.Runtime.Misc;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using DocumentFormat.OpenXml.Spreadsheet;
using Gestreino.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestreino.Controllers
{
    public class AjaxController : Controller
    {
        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();

        // GET: Ajax
        public ActionResult Users(Gestreino.Models.Users MODEL)
        {
            return View("GTManagement/Users",MODEL);
        }



        // GET: Ajax
        public ActionResult Groups(UTILIZADORES_ACESSO_GRUPOS MODEL,string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.UTILIZADORES_ACESSO_GRUPOS.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.SIGLA = data.First().SIGLA;
                MODEL.NOME = data.First().NOME;
                MODEL.DESCRICAO = data.First().DESCRICAO;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Groups", MODEL);
        }
        public ActionResult Profiles(UTILIZADORES_ACESSO_PERFIS MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.UTILIZADORES_ACESSO_PERFIS.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.DESCRICAO = data.First().DESCRICAO;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Profiles", MODEL);
        }
        public ActionResult Atoms(UTILIZADORES_ACESSO_ATOMOS MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.UTILIZADORES_ACESSO_ATOMOS.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.DESCRICAO = data.First().DESCRICAO;
            }
            int?[] ids = new int?[] {id.Value};
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Atoms", MODEL);
        }


        public ActionResult UserGroups(AccessAppendItems MODEL, string action, int? id, int?[] bulkids)
        {
            
            MODEL.Id = id;

            int?[] ids = new int?[] { id.Value };
            //if (action.Contains("Multiplos")) ids = bulkids;
            //if (action.Contains("Multiplos")) action = "Remover";

            if(action== "RemoverMultiplosGroupUtil") {
                ids = bulkids;
                action = "RemoverGroupUtil";
            }
            if (action == "RemoverMultiplosGroupAtom")
            {
                ids = bulkids;
                action = "RemoverGroupAtom";
            }

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/AppendItems", MODEL);
        }

    }
}