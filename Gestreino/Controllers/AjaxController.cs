using Antlr.Runtime.Misc;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using DocumentFormat.OpenXml.Spreadsheet;
using Gestreino.Classes;
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
            return View("administration/Users/Index",MODEL);
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
            return View("administration/Access/Groups", MODEL);
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
            return View("administration/Access/Profiles", MODEL);
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
            return View("administration/Access/Atoms", MODEL);
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
            return View("administration/Access/AppendItems", MODEL);
        }


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
            return View("administration/Parameters/GRLDocType", MODEL);
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
            return View("administration/Parameters/GRLTokenType", MODEL);
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
            return View("administration/Parameters/GRLEndPais", MODEL);
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
            return View("administration/Parameters/GRLEndCidade", MODEL);
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
            return View("administration/Parameters/GRLEndDistr", MODEL);
        }




        public ActionResult PESFamily(PES_Dados_Pessoais_Agregado MODEL, string action, int? id, int?[] bulkids)
        {
            MODEL.ID = id;
            MODEL.PES_FAMILIARES_GRUPOS_LIST = databaseManager.PES_FAMILIARES_GRUPOS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PES_PROFISSAO_LIST = databaseManager.PES_PROFISSOES.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PaisList = databaseManager.GRL_ENDERECO_PAIS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.CidadeList = databaseManager.GRL_ENDERECO_CIDADE.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.DistrictoList = databaseManager.GRL_ENDERECO_MUN_DISTR.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });

            
            if (action == "Editar")
            {

                var data = databaseManager.PES_PESSOAS_FAM.Where(x => x.ID == id).ToList();
                var dataCon = databaseManager.PES_PESSOAS_FAM_CONTACTOS.Where(x => x.PES_PESSOAS_FAM_ID == id).ToList();
                var dataEnd = databaseManager.PES_PESSOAS_FAM_ENDERECOS.Where(x => x.PES_PESSOAS_FAM_ID == id).ToList();
                MODEL.PES_FAMILIARES_GRUPOS_ID = data.First().PES_FAMILIARES_GRUPOS_ID;
                MODEL.Nome = data.First().NOME;
                MODEL.PES_PROFISSAO_ID= data.First().PES_PROFISSOES_ID;
                MODEL.Isento = data.First().ISENTO;

                MODEL.Telephone = (!string.IsNullOrEmpty(dataCon.First().TELEFONE.ToString())) ? dataCon.First().TELEFONE.ToString() : null;
                MODEL.TelephoneAlternativo = (!string.IsNullOrEmpty(dataCon.First().TELEFONE_ALTERNATIVO.ToString())) ? dataCon.First().TELEFONE_ALTERNATIVO.ToString() : null;
                MODEL.Fax = (!string.IsNullOrEmpty(dataCon.First().FAX.ToString())) ? dataCon.First().FAX.ToString() : null;
                MODEL.Email = dataCon.First().EMAIL;
                MODEL.URL = dataCon.First().URL;

                MODEL.PaisId = dataEnd.First().GRL_ENDERECO_PAIS_ID;
                MODEL.CidadeId = dataEnd.First().GRL_ENDERECO_CIDADE_ID;
                MODEL.DistrictoId = dataEnd.First().GRL_ENDERECO_MUN_DISTR_ID;
                MODEL.Numero = dataEnd.First().NUMERO;
                MODEL.Rua = dataEnd.First().RUA;
                MODEL.Morada = dataEnd.First().MORADA;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Athletes/GPManagementUserAgregado", MODEL);
        }
        public ActionResult PESProfessional(PES_Dados_Pessoais_Professional MODEL, string action, int? id, int?[] bulkids)
        {

            MODEL.ID = id;
            MODEL.PES_PROFISSAO_LIST = databaseManager.PES_PROFISSOES.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PES_PROFISSOES_CONTRACTO_LIST = databaseManager.PES_PROFISSOES_TIPO_CONTRACTO.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PES_PROFISSOES_REGIME_LIST = databaseManager.PES_PROFISSOES_REGIME_TRABALHO.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            
            if (action == "Editar")
            {
               var data = databaseManager.PES_PESSOAS_PROFISSOES.Where(x => x.ID == id).ToList();
                MODEL.PES_PROFISSAO_ID = data.First().PES_PROFISSOES_ID;
                MODEL.Empresa = data.First().EMPRESA;
                MODEL.PES_PROFISSOES_CONTRACTO_ID = data.First().PES_PROFISSOES_ID;
                MODEL.PES_PROFISSOES_REGIME_ID = data.First().PES_PROFISSOES_ID;
                MODEL.Descricao = data.First().DESCRICAO;
                MODEL.DateIni= string.IsNullOrEmpty(data.First().DATA_INICIO.ToString()) ? null : DateTime.Parse(data.First().DATA_INICIO.ToString()).ToString("dd-MM-yyyy");
                MODEL.DateEnd = string.IsNullOrEmpty(data.First().DATA_FIM.ToString()) ? null : DateTime.Parse(data.First().DATA_FIM.ToString()).ToString("dd-MM-yyyy");
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Athletes/GPManagementUserProfessional", MODEL);
        }
        public ActionResult PESDisability(PES_Dados_Pessoais_Deficiencia MODEL, string action, int? id, int?[] bulkids)
        {
            MODEL.ID = id;
            if (action == "Editar")
            {
                var data = databaseManager.PES_PESSOAS_CARACT_DEFICIENCIA.Where(x => x.ID == id).ToList();
                MODEL.Descricao = data.First().DESCRICAO;
                MODEL.PES_DEFICIENCIA_ID = data.First().PES_PESSOAS_CARACT_TIPO_DEF_ID;
                MODEL.PES_DEFICIENCIA_GRAU_ID = data.First().PES_PESSOAS_CARACT_GRAU_DEF_ID;
            }

            MODEL.PES_DEFICIENCIA_LIST = databaseManager.PES_PESSOAS_CARACT_TIPO_DEF.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });

            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";
            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Athletes/GPManagementUserDisability", MODEL);
        }

    }
}