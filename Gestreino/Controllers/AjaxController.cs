using Antlr.Runtime.Misc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Wordprocessing;
using Gestreino.Classes;
using Gestreino.Models;
using JeanPiagetSGA;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Gestreino.Controllers
{
    [Authorize]
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

        public ActionResult PESDadosPessoaisIdent(PES_Dados_Pessoais_Ident MODEL, string action, int? id, int?[] bulkids)
        {
            MODEL.ID = id;
            if (action == "Editar")
            {
                var data = databaseManager.PES_IDENTIFICACAO.Where(x => x.ID == id).ToList();
                var dataLocal = databaseManager.PES_IDENTIFICACAO_LOCAL_EM.Where(x => x.PES_IDENTIFICACAO_ID == id).ToList();
                MODEL.PES_PESSOAS_ID= data.First().PES_PESSOAS_ID;
                MODEL.Numero = data.First().NUMERO;
                MODEL.Observacao = data.First().OBSERVACOES;
                MODEL.PES_TIPO_IDENTIFICACAO= data.First().PES_TIPO_IDENTIFICACAO_ID;
                MODEL.OrgaoEmissor = data.First().ORGAO_EMISSOR;
                MODEL.DateIssue = string.IsNullOrEmpty(data.First().DATA_EMISSAO.ToString()) ? null : DateTime.Parse(data.First().DATA_EMISSAO.ToString()).ToString("dd-MM-yyyy");
                MODEL.DateExpire = string.IsNullOrEmpty(data.First().DATA_VALIDADE.ToString()) ? null : DateTime.Parse(data.First().DATA_VALIDADE.ToString()).ToString("dd-MM-yyyy");
                MODEL.PaisId= dataLocal.First().GRL_ENDERECO_PAIS_ID;
                MODEL.CidadeId = dataLocal.First().GRL_ENDERECO_CIDADE_ID;
                //MODEL.PES_DEFICIENCIA_ID = data.First().PES_PESSOAS_CARACT_TIPO_DEF_ID;
                //MODEL.PES_DEFICIENCIA_GRAU_ID = data.First().PES_PESSOAS_CARACT_GRAU_DEF_ID;
            }

            MODEL.PES_TIPO_IDENTIFICACAO_LIST = databaseManager.PES_TIPO_IDENTIFICACAO.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PaisList = databaseManager.GRL_ENDERECO_PAIS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.CidadeList = databaseManager.GRL_ENDERECO_CIDADE.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });

            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";
            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Athletes/GPManagementUserIdent", MODEL);
        }


        public ActionResult GRLIdentType(PES_TIPO_IDENTIFICACAO MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.PES_TIPO_IDENTIFICACAO.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLIdentType", MODEL);
        }
        public ActionResult GRLEstadoCivil(PES_ESTADO_CIVIL MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.PES_ESTADO_CIVIL.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLEstadoCivil", MODEL);
        }
        public ActionResult GRLEndType(PES_TIPO_ENDERECOS MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.PES_TIPO_ENDERECOS.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLEndType", MODEL);
        }
        public ActionResult GRLProfissao(PES_PROFISSOES MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.PES_PROFISSOES.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLProfissao", MODEL);
        }
        public ActionResult GRLProfContract(PES_PROFISSOES_TIPO_CONTRACTO MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.PES_PROFISSOES_TIPO_CONTRACTO.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLProfContract", MODEL);
        }
        public ActionResult GRLProfRegime(PES_PROFISSOES_REGIME_TRABALHO MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.PES_PROFISSOES_REGIME_TRABALHO.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLProfRegime", MODEL);
        }
        public ActionResult GRLPESAgregado(PES_FAMILIARES_GRUPOS MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.PES_FAMILIARES_GRUPOS.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLPESAgregado", MODEL);
        }
        public ActionResult GRLPESGrupoSang(PES_PESSOAS_CARACT_TIPO_SANG MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.PES_PESSOAS_CARACT_TIPO_SANG.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLPESGrupoSang", MODEL);
        }
        public ActionResult GRLPESDef(PES_PESSOAS_CARACT_TIPO_DEF MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.PES_PESSOAS_CARACT_TIPO_DEF.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.NOME = data.First().NOME;
                MODEL.SIGLA = data.First().SIGLA;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLPESDef", MODEL);
        }

        public ActionResult GRLGTDuracaoPlano(GT_DuracaoPlano MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.GT_DuracaoPlano.Where(x => x.ID == id).ToList();
                MODEL.ID = data.First().ID;
                MODEL.DURACAO = data.First().DURACAO;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLGTDuracaoPlano", MODEL);
        }
          public ActionResult GRLGTFaseTreino(Gestreino.Models.GT_FaseTreino MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.GT_FaseTreino.Where(x => x.ID == id).ToList();
                MODEL.ID = id;
                MODEL.SIGLA = data.First().SIGLA;
                MODEL.GT_Series_ID = data.First().GT_Series_ID;
                MODEL.GT_Repeticoes_ID = data.First().GT_Repeticoes_ID;
                MODEL.GT_Carga_ID = data.First().GT_Carga_ID;
                MODEL.GT_TempoDescanso_ID = data.First().GT_TempoDescanso_ID;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            MODEL.GT_Series_List = databaseManager.GT_Series.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.SERIES.ToString() });
            MODEL.GT_Repeticoes_List = databaseManager.GT_Repeticoes.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.REPETICOES.ToString() });
            MODEL.GT_Carga_List = databaseManager.GT_Carga.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.CARGA.ToString() });
            MODEL.GT_TempoDescanso_List = databaseManager.GT_TempoDescanso.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.TEMPO_DESCANSO });

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLGTFaseTreino", MODEL);
        }

        public ActionResult GRLGTTipoTreino(GT_TipoTreino MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.GT_TipoTreino.Where(x => x.ID == id).ToList();
                MODEL.ID = id.Value;
                MODEL.SIGLA = data.First().SIGLA;
                MODEL.NOME = data.First().NOME;
            }
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("administration/Parameters/GRLGTTipoTreino", MODEL);
        }
        public ActionResult GTExercicio(GTExercicio MODEL, string action, int? id, int?[] bulkids)
        {
            if (action == "Editar")
            {
                var data = databaseManager.GT_Exercicio.Where(x => x.ID == id).ToList();
                MODEL.ID = id.Value;
                MODEL.TipoTreinoId = data.First().GT_TipoTreino_ID;
                MODEL.Nome = data.First().NOME;
                MODEL.Alongamento= data.First().ALONGAMENTO;
                MODEL.Sequencia = data.First().SEQUENCIA;
            }

            MODEL.TipoList = databaseManager.GT_TipoTreino.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });

            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Exercises/GTExercise", MODEL);
        }


        public ActionResult GTTreinos(string action, int? id, int?[] bulkids)
        {
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Plans/Index");
        }
        public ActionResult GTQuest(string action, int? id, int?[] bulkids,string upload)
        {
            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            ViewBag.upload = upload;
            return View("GTManagement/Quest/Index");
        }
        public ActionResult GTAvaliado(GTAvaliado MODEL, string action, int? id, int?[] bulkids)
        {
            MODEL.AthleteId = string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) 
                ? 0 : int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO));
            MODEL.AthleteList = databaseManager.SP_PES_ENT_PESSOAS(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });

            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";

            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("GTManagement/Athletes/GTAvaliado", MODEL);
        }
        private void SetGTAvaliado(int PesId)
        {
            if (PesId > 0)
            {
                var Age = 0;
                var av1 = databaseManager.SP_PES_ENT_PESSOAS(PesId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).Select(x => new { x.NOME_PROPIO, x.APELIDO, x.DATA_NASCIMENTO,x.SEXO }).ToList();
                var av2 = databaseManager.PES_PESSOAS_CARACT.Where(x => x.PES_PESSOAS_ID == PesId).Select(x => new { x.ALTURA, x.PESO }).ToList();

                if (av1.Any())
                {
                    var DateofBirth = string.IsNullOrEmpty(av1.First().DATA_NASCIMENTO) ? (DateTime?)null : DateTime.ParseExact(av1.First().DATA_NASCIMENTO, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (DateofBirth != null)
                        Age = Converters.CalculateAge(DateofBirth.Value);

                    Configs.GESTREINO_AVALIDO_NOME = av1.First().NOME_PROPIO + " " + av1.First().APELIDO;
                    Configs.GESTREINO_AVALIDO_IDADE = Age.ToString();
                    Configs.GESTREINO_AVALIDO_SEXO = av1.First().SEXO;
                }
                if (av2.Any())
                {
                    Configs.GESTREINO_AVALIDO_PESO = av2.First().PESO.Value.ToString("G29");
                    Configs.GESTREINO_AVALIDO_ALTURA = av2.First().ALTURA.Value.ToString("G29");
                }
            }

        }



        // Cookies
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetCookies(string entity,string value)
        {
            try
            {
                Cookies c = new Cookies();
                c.WriteCookie(entity, value);
                string cookievalue;

                if (entity == Cookies.COOKIES_GESTREINO_AVALIADO)
                {
                    SetGTAvaliado(int.Parse(value));
                    return Json(new { result = true, error = string.Empty, reload=true,showToastr = true, toastrMessage = "Submetido com sucesso!" });
                }

                if (Request.Cookies["cookie"] != null)
                {
                  //  cookievalue = Request.Cookies["cookie"].ToString();
                }
                else
                {
                 //   Response.Cookies["cookie"].Value = "cookie value";
                 //   Response.Cookies["cookie"].Expires = DateTime.Now.AddMinutes(1); // add expiry time
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }



 





        // File Management
        public ActionResult FileManagement(Gestreino.Classes.FileUploader.FileUploadModel MODEL, string action, int? id, int?[] bulkids,string upload)
        {
            MODEL.EntityID = id.Value;
            MODEL.EntityName = upload;

            if (action == "Editar")
            {
                MODEL.ID = id.Value;
                // Define tablename and fieldname for Stored Procedure
                string tablename = FileUploader.DecoderFactory(upload)[0];
                string fieldname = FileUploader.DecoderFactory(upload)[1];

                var data = databaseManager.SP_ASSOC_ARQUIVOS(null, MODEL.ID, null, null, null, null, null, null, null, null, null, null, null, tablename, fieldname, null, "R").ToList();
                MODEL.TipoDocId = data.First().GRL_ARQUIVOS_TIPO_DOCS_ID;
                MODEL.Nome= data.First().NOME;
                MODEL.Descricao = data.First().DESCRICAO;
                MODEL.Status= data.First().ACTIVO;
                MODEL.DateAct = string.IsNullOrWhiteSpace(data.First().DATA_ACT) ? null : DateTime.Parse(data.First().DATA_ACT).ToString("dd-MM-yyyy");
                MODEL.DateDisact = string.IsNullOrWhiteSpace(data.First().DATA_DESACT_EXPIRACAO) ? null : DateTime.Parse(data.First().DATA_DESACT_EXPIRACAO).ToString("dd-MM-yyyy");
            }

            int?[] ids = new int?[] { id.Value };
            if (action.Contains("Multiplos")) ids = bulkids;
            if (action.Contains("Multiplos")) action = "Remover";
            MODEL.TipoDocList = databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME.ToString() });
            
            ViewBag.bulkids = ids;
            ViewBag.Action = action;
            return View("FileManagement", MODEL);
        }
        [HttpPost]
        public ActionResult GetFiles(int? EntityId, int? ArquivoId, string upload)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Nome = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Descricao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Documento = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Tipo = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Tamanho = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Estado = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var DataAct = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
            var DataDesact = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[8][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[9][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[10][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[11][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            EntityId = EntityId == null ? null : EntityId;
            ArquivoId = ArquivoId == null ? null : ArquivoId;

            // Define tablename and fieldname for Stored Procedure
            string tablename = FileUploader.DecoderFactory(upload)[0];
            string fieldname = FileUploader.DecoderFactory(upload)[1];

            var v = (from a in databaseManager.SP_ASSOC_ARQUIVOS(EntityId, ArquivoId, null, null, null, null, null, null, null, null, null, null, null, tablename, fieldname , null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
            if (!string.IsNullOrEmpty(Descricao)) v = v.Where(a => a.DESCRICAO != null && a.DESCRICAO.ToUpper().Contains(Descricao.ToUpper()));
            if (!string.IsNullOrEmpty(Documento)) v = v.Where(a => a.ARQ_TIPO_DOC != null && a.ARQ_TIPO_DOC.ToUpper().Contains(Documento.ToUpper()));
            if (!string.IsNullOrEmpty(Tipo)) v = v.Where(a => a.ARQ_TIPO_DOC != null && a.ARQ_TIPO_DOC.ToString() == Tipo);
            if (!string.IsNullOrEmpty(Tamanho)) v = v.Where(a => a.ARQ_TAMANHO != null && a.ARQ_TAMANHO.ToString()== Tamanho);
            if (!string.IsNullOrEmpty(Estado)) v = v.Where(a => a.ACTIVO != null && a.ACTIVO.Contains(Estado));
            if (!string.IsNullOrEmpty(DataAct)) v = v.Where(a => a.DATA_ACT != null && a.DATA_ACT.ToUpper().Contains(DataAct.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse
            if (!string.IsNullOrEmpty(DataDesact)) v = v.Where(a => a.DATA_DESACT_EXPIRACAO != null && a.DATA_DESACT_EXPIRACAO.ToUpper().Contains(DataDesact.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse
            if (!string.IsNullOrEmpty(Insercao)) v = v.Where(a => a.INSERCAO != null && a.INSERCAO.ToUpper().Contains(Insercao.ToUpper()));
            if (!string.IsNullOrEmpty(DataInsercao)) v = v.Where(a => a.DATA_INSERCAO != null && a.DATA_INSERCAO.ToUpper().Contains(DataInsercao.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse
            if (!string.IsNullOrEmpty(Actualizacao)) v = v.Where(a => a.ACTUALIZACAO != null && a.ACTUALIZACAO.ToUpper().Contains(Actualizacao.ToUpper()));
            if (!string.IsNullOrEmpty(DataActualizacao)) v = v.Where(a => a.DATA_ACTUALIZACAO != null && a.DATA_ACTUALIZACAO.ToUpper().Contains(DataActualizacao.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse


            //ORDER RESULT SET
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                if (sortColumnDir == "asc")
                {
                    switch (sortColumn)
                    {
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
                        case "DESCRICAO": v = v.OrderBy(s => s.DESCRICAO); break;
                        case "DOCUMENTO": v = v.OrderBy(s => s.ARQ_TIPO_DOC); break;
                        case "TIPO": v = v.OrderBy(s => s.GRL_ARQUIVOS_TIPO_DOCS_ID); break;
                        case "TAMANHO": v = v.OrderBy(s => s.ARQ_TAMANHO); break;
                        case "ESTADO": v = v.OrderBy(s => s.ACTIVO); break;
                        case "DATAACTIVACAO": v = v.OrderBy(s => s.DATA_ACT); break;
                        case "DATADESACTIVACAO": v = v.OrderBy(s => s.DATA_DESACT_EXPIRACAO); break;
                        case "INSERCAO": v = v.OrderBy(s => s.INSERCAO); break;
                        case "DATAINSERCAO": v = v.OrderBy(s => s.DATA_INSERCAO); break;
                        case "ACTUALIZACAO": v = v.OrderBy(s => s.ACTUALIZACAO); break;
                        case "DATAACTUALIZACAO": v = v.OrderBy(s => s.DATA_ACTUALIZACAO); break;
                    }
                }
                else
                {
                    switch (sortColumn)
                    {
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
                        case "DESCRICAO": v = v.OrderByDescending(s => s.DESCRICAO); break;
                        case "DOCUMENTO": v = v.OrderByDescending(s => s.ARQ_TIPO_DOC); break;
                        case "TIPO": v = v.OrderByDescending(s => s.GRL_ARQUIVOS_TIPO_DOCS_ID); break;
                        case "TAMANHO": v = v.OrderByDescending(s => s.ARQ_TAMANHO); break;
                        case "ESTADO": v = v.OrderByDescending(s => s.ACTIVO); break;
                        case "DATAACTIVACAO": v = v.OrderByDescending(s => s.DATA_ACT); break;
                        case "DATADESACTIVACAO": v = v.OrderByDescending(s => s.DATA_DESACT_EXPIRACAO); break;
                        case "INSERCAO": v = v.OrderByDescending(s => s.INSERCAO); break;
                        case "DATAINSERCAO": v = v.OrderByDescending(s => s.DATA_INSERCAO); break;
                        case "ACTUALIZACAO": v = v.OrderByDescending(s => s.ACTUALIZACAO); break;
                        case "DATAACTUALIZACAO": v = v.OrderByDescending(s => s.DATA_ACTUALIZACAO); break;
                    }
                }
            }

            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            TempData["QUERYRESULT"] = v.ToList();

            //RETURN RESPONSE JSON PARSE
            return Json(new
            {
                draw = draw,
                recordsFiltered = totalRecords,
                recordsTotal = totalRecords,
                data = data.Select(x => new
                {
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_DELETE) ? "none" : "",
                    Id = x.ID,
                    NOME = x.NOME,
                    DESCRICAO = x.DESCRICAO,
                    DOCUMENTO = x.ARQ_TIPO_DOC,
                    TIPO = x.ARQ_TIPO,
                    TAMANHO = x.ARQ_TAMANHO,
                    ESTADO = x.ACTIVO,
                    DATAACTIVACAO = x.DATA_ACT,
                    DATADESACTIVACAO = x.DATA_DESACT_EXPIRACAO,
                    INSERCAO = x.INSERCAO,
                    DATAINSERCAO = x.DATA_INSERCAO,
                    ACTUALIZACAO = x.ACTUALIZACAO,
                    DATAACTUALIZACAO = x.DATA_ACTUALIZACAO,
                    Link=x.ARQ_URL
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }

        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFile(HttpPostedFileBase file, Gestreino.Classes.FileUploader.FileUploadModel MODEL)
        {
            try
            {
                //  VALIDATE FORM FIRST
                if (!ModelState.IsValid)
                {
                    string errors = string.Empty;
                    ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                    return Json(new { result = false, error = errors });
                }
                if (!string.IsNullOrWhiteSpace(MODEL.DateAct) && DateTime.ParseExact(MODEL.DateDisact, "dd-MM-yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(MODEL.DateAct, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                {
                    return Json(new { result = false, error = "Data de Emissão deve ser inferior a Data de Validade!" });
                }
                var DateAct = string.IsNullOrWhiteSpace(MODEL.DateAct) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateAct, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DateDisact = string.IsNullOrWhiteSpace(MODEL.DateDisact) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateDisact, "dd-MM-yyyy", CultureInfo.InvariantCulture);


                // Get Allowed size
                var allowedSize = Classes.FileUploader.TwoMB; // 2.0 MB
                var entity = MODEL.EntityName;

                if (file != null)
                {
                    if (file.ContentLength > 0 && file.ContentLength < Convert.ToDouble(WebConfigurationManager.AppSettings["maxRequestLength"]))
                    {
                        // Get Module Subfolder
                        var modulestorage = FileUploader.ModuleStorage[Convert.ToInt32(FileUploader.DecoderFactory(entity)[2])];

                        // Get Document Type Id
                        //var tipoidentname = databaseManager.PES_TIPO_IDENTIFICACAO.Where(x => x.ID == MODEL.PES_TIPO_IDENTIFICACAO).Select(x => x.NOME).FirstOrDefault();
                        var tipodoc = string.Empty;
                        var tipodocid = 0;
                        if (databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.ID == MODEL.TipoDocId).ToList().Count > 0)
                        {
                            tipodoc = databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.ID == MODEL.TipoDocId).Select(x => x.NOME).FirstOrDefault().ToLower();
                            tipodocid = databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.ID == MODEL.TipoDocId).Select(x => x.ID).FirstOrDefault();
                        }
                        else
                            return Json(new { result = false, error = "Arquivo não encontrado, certifique-se que o mesmo seja registado nas parametrizações!" });

                        // Get file size
                        var size = file.ContentLength;
                        // Get file type
                        var type = System.IO.Path.GetExtension(file.FileName).ToLower();
                        // Get directory
                        string[] DirectoryFactory = FileUploader.DirectoryFactory(modulestorage, Server.MapPath(FileUploader.FileStorage),System.IO.Path.GetExtension(file.FileName), tipodoc, MODEL.Nome);
                        /*
                         * 0 => sqlpath,
                         * 1 => path,
                         * 2 => filename
                         */
                        var sqlpath = DirectoryFactory[0];
                        var path = DirectoryFactory[1];
                        var filename = DirectoryFactory[2];
                        // Define tablename and fieldname for Stored Procedure
                        string tablename = FileUploader.DecoderFactory(entity)[0];
                        string fieldname = FileUploader.DecoderFactory(entity)[1];

                        // Check file type
                        if (!FileUploader.allowedExtensions.Contains(type))
                            return Json(new { result = false, error = "Formato inválido!, por favor adicionar um documento válido com a capacidade permitida!" });

                        // Check file size
                        if (size > allowedSize)
                            return Json(new { result = false, error = "Tamanho do documento deve ser inferior a " + FileUploader.FormatSize(allowedSize) + "!" });

                        var Active = MODEL.Status == "Activo" ? true : false;

                        // Upload file to folder
                        file.SaveAs(path);
                        // Create file reference in SQL Database
                        var createFile = databaseManager.SP_ASSOC_ARQUIVOS(MODEL.ID, null, MODEL.Nome, MODEL.Descricao, Active, DateAct, DateDisact, tipodocid, filename, null, type, size, sqlpath, tablename, fieldname, int.Parse(User.Identity.GetUserId()), Convert.ToChar('C').ToString()).ToList();
                    }
                    else
                    {
                        return Json(new { result = false, error = "Por favor adicionar um documento válido com a capacidade permitida!" });
                    }
                }
                     ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "tblInstituicoesArquivos", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFile(Gestreino.Classes.FileUploader.FileUploadModel MODEL)
        {
            try
            {
                //  VALIDATE FORM FIRST
                if (!ModelState.IsValid)
                {
                    string errors = string.Empty;
                    ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                    return Json(new { result = false, error = errors });
                }
                if (!string.IsNullOrWhiteSpace(MODEL.DateAct) && DateTime.ParseExact(MODEL.DateDisact, "dd-MM-yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(MODEL.DateAct, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                {
                    return Json(new { result = false, error = "Data de Emissão deve ser inferior a Data de Validade!" });
                }
                var DateAct = string.IsNullOrWhiteSpace(MODEL.DateAct) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateAct, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DateDisact = string.IsNullOrWhiteSpace(MODEL.DateDisact) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateDisact, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var Active = MODEL.Status == "Activo" ? true : false;

                // Define tablename and fieldname for Stored Procedure
                string tablename = FileUploader.DecoderFactory(MODEL.EntityName)[0];
                string fieldname = FileUploader.DecoderFactory(MODEL.EntityName)[1];

                // Update
                var update = databaseManager.SP_ASSOC_ARQUIVOS(null, MODEL.ID, MODEL.Nome, MODEL.Descricao, Active, DateAct, DateDisact, MODEL.TipoDocId, null, null, null, null, null, tablename, fieldname, int.Parse(User.Identity.GetUserId()), Convert.ToChar('U').ToString()).ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "tblInstituicoesArquivos", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(Gestreino.Classes.FileUploader.FileUploadModel MODEL,int?[] ids)
        {
            try
            {
                //  VALIDATE FORM FIRST
                if (!ModelState.IsValid)
                {
                    string errors = string.Empty;
                    ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                    return Json(new { result = false, error = errors });
                }
                // Define tablename and fieldname for Stored Procedure
                string tablename = FileUploader.DecoderFactory(MODEL.EntityName)[0];
                string fieldname = FileUploader.DecoderFactory(MODEL.EntityName)[1];

                // Delete
                foreach (var i in ids)
                {
                    databaseManager.SP_ASSOC_ARQUIVOS(null, i, null, null, null, null,null, null, null, null, null, null, null, tablename, null, null, Convert.ToChar('D').ToString()).ToList();
                 }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "tblInstituicoesArquivos", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }



    }
}