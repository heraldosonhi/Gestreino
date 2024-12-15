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
        public ActionResult Users(Gestreino.Models.Users MODEL, string action, int? id, int?[] bulkids)
        {

            MODEL.Status = 1;

            if (id>0)
            {
                var data = databaseManager.UTILIZADORES.Where(x => x.ID == id).ToList();
                MODEL.Login = data.First().LOGIN;
                MODEL.Status = data.First().ACTIVO?1:0;
                MODEL.Id = id;
            }

            ViewBag.Action = action;
            return View("GTManagement/Users/Index",MODEL);
        }
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
            return View("GTManagement/Access/Groups", MODEL);
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
            return View("GTManagement/Access/Profiles", MODEL);
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
            return View("GTManagement/Access/Atoms", MODEL);
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
            return View("GTManagement/Access/AppendItems", MODEL);
        }




        //GRL ENTITIES
        public ActionResult GRLDocType(GRL_ARQUIVOS_TIPO_DOCS MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Parameters/GRLDocType", MODEL);
        }
        public ActionResult GRLTokenType(GRL_TOKENS_TIPOS MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.GRL_TOKENS_TIPOS.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Parameters/GRLTokenType", MODEL);
        }
        public ActionResult GRLEndPais(GRL_ENDERECO_PAIS MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.GRL_ENDERECO_PAIS.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
                MODEL.CODIGO = data.First().CODIGO;
                MODEL.INDICATIVO = data.First().INDICATIVO;
                MODEL.NACIONALIDADE_MAS = data.First().NACIONALIDADE_MAS;
                MODEL.NACIONALIDADE_FEM = data.First().NACIONALIDADE_FEM;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Parameters/GRLEndPais", MODEL);
        }
        public ActionResult GRLEndCidade(GRLEndCidade MODEL, string action, int? id, int?[] bulkids)
        {

            MODEL.PAIS_LIST = databaseManager.GRL_ENDERECO_PAIS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });

            if (action == "Editar")
            {
                var data = databaseManager.GRL_ENDERECO_CIDADE.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
                MODEL.ENDERECO_PAIS_ID = data.First().ENDERECO_PAIS_ID;
            }

            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Parameters/GRLEndCidade", MODEL);
        }
        public ActionResult GRLEndDistr(GRLEndDistr MODEL, string action, int? id, int?[] bulkids)
        {
            MODEL.CIDADE_LIST = databaseManager.GRL_ENDERECO_CIDADE.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });

            if (action == "Editar")
            {
                var data = databaseManager.GRL_ENDERECO_MUN_DISTR.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
                MODEL.ENDERECO_CIDADE_ID = data.First().ENDERECO_CIDADE_ID;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Parameters/GRLEndDistr", MODEL);
        }

    }
}