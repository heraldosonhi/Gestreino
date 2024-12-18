using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Gestreino.Classes;
using Gestreino.Models;
using JeanPiagetSGA;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static Gestreino.Classes.SelectValues;

namespace Gestreino.Controllers
{
    [Authorize]
    public class GTManagementController : Controller
    {
        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();
        // _MenuLeftBarLink
        int _MenuLeftBarLink_Athletes = 201;
        int _MenuLeftBarLink_FileManagement = 202;

        // GET: GTManagement
        public ActionResult Index()
        {
            return View();
        }

        // GET: GTManagement
        public ActionResult Athletes()
        {
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Athletes;
            return View("Athletes/Index");
        }

        // GET: GTManagement
        public ActionResult NewAthlete(Gestreino.Models.Athlete MODEL)
        {
            MODEL.Caract_DuracaoPlanoList = databaseManager.GT_DuracaoPlano.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.DURACAO).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DURACAO.ToString() });
            MODEL.EstadoCivilList = databaseManager.PES_ESTADO_CIVIL.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PAIS_LIST = databaseManager.GRL_ENDERECO_PAIS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.CIDADE_LIST = databaseManager.GRL_ENDERECO_CIDADE.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.MUN_LIST = databaseManager.GRL_ENDERECO_MUN_DISTR.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.ENDERECO_TIPO_LIST = databaseManager.PES_TIPO_ENDERECOS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.ENDERECO_PAIS_ID = Configs.INST_MDL_ADM_VLRID_ADDR_STANDARD_COUNTRY;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Athletes;
            return View("Athletes/NewAthlete", MODEL);
        }
        // GET: GTManagement
        public ActionResult UpdateAthlete(int? Id, Gestreino.Models.Athlete MODEL)
        {
            if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }

            var data = databaseManager.SP_PES_ENT_PESSOAS(Id, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).ToList();
            if (!data.Any()) return RedirectToAction("", "home");
            var dataCaract = databaseManager.PES_PESSOAS_CARACT.Where(x => x.PES_PESSOAS_ID == Id).ToList();
            var nacionalidade = databaseManager.PES_NACIONALIDADE.Where(x => x.PES_PESSOAS_ID == Id).Select(x => x.GRL_ENDERECO_PAIS_ID).ToArray();
            var endereco = databaseManager.PES_ENDERECOS.Where(x => x.PES_PESSOAS_ID == Id).ToList();
            var naturalidade = databaseManager.PES_NATURALIDADE.Where(x => x.PES_PESSOAS_ID == Id).ToList();

            MODEL.ID = data.First().ID;
            MODEL.Numero = data.First().PES_NUMERO;
            MODEL.Nome = data.First().NOME;
            MODEL.Sexo = data.First().SEXO == "Masculino" ? 1 : 0;
            MODEL.DataNascimento = string.IsNullOrEmpty(data.First().DATA_NASCIMENTO.ToString()) ? null : DateTime.Parse(data.First().DATA_NASCIMENTO.ToString()).ToString("dd-MM-yyyy");
            MODEL.EstadoCivil = data.First().PES_ESTADO_CIVIL_ID;
            MODEL.NIF = data.First().NIF;
            MODEL.NacionalidadeId = nacionalidade;
            MODEL.Telephone = (!string.IsNullOrEmpty(data.First().TELEFONE.ToString())) ? data.First().TELEFONE.ToString() : null;
            MODEL.TelephoneAlternativo = (!string.IsNullOrEmpty(data.First().TELEFONE_ALTERNATIVO.ToString())) ? data.First().TELEFONE_ALTERNATIVO.ToString() : null;
            MODEL.Email = data.First().EMAIL;

            if (naturalidade.Any())
            {
                MODEL.NAT_PAIS_ID = naturalidade.First().GRL_ENDERECO_PAIS_ID;
                MODEL.NAT_CIDADE_ID = naturalidade.First().GRL_ENDERECO_CIDADE_ID;
                MODEL.NAT_MUN_ID = naturalidade.First().GRL_ENDERECO_MUN_DISTR_ID;
            }
            if (endereco.Any())
            {
                MODEL.EndNumero = endereco.First().NUMERO;
                MODEL.Rua = endereco.First().RUA;
                MODEL.Morada = endereco.First().MORADA;
                MODEL.ENDERECO_PAIS_ID = endereco.First().GRL_ENDERECO_PAIS_ID;
                MODEL.ENDERECO_CIDADE_ID = endereco.First().GRL_ENDERECO_CIDADE_ID;
                MODEL.ENDERECO_MUN_ID = endereco.First().GRL_ENDERECO_MUN_DISTR_ID;
                MODEL.ENDERECO_TIPO = endereco.First().PES_TIPO_ENDERECOS_ID;
            }
            //
            if (dataCaract.Any())
            {
                MODEL.Caract_Altura = (!string.IsNullOrEmpty(dataCaract.First().ALTURA.ToString())) ? (dataCaract.First().ALTURA ?? 0).ToString("G29").Replace(",", ".") : null;
                MODEL.Caract_VO2 = dataCaract.First().VO2;
                MODEL.Caract_Peso = (!string.IsNullOrEmpty(dataCaract.First().PESO.ToString())) ? (dataCaract.First().PESO ?? 0).ToString("G29").Replace(",", ".") : null;
                MODEL.Caract_MassaGorda = dataCaract.First().MASSAGORDA;
                MODEL.Caract_IMC = dataCaract.First().IMC;
                MODEL.Caract_DuracaoPlano = dataCaract.First().GT_DuracaoPlano_ID;
                MODEL.Caract_FCRepouso = dataCaract.First().FCREPOUSO;
                //MODEL.Caract_Protocolo = dataCaract.First().PRO;
                MODEL.Caract_FCMaximo = dataCaract.First().FCMAXIMO;
                MODEL.Caract_TASistolica = dataCaract.First().TASISTOLICA;
                MODEL.Caract_TADistolica = dataCaract.First().TADISTOLICA;
                //
                MODEL.FR_Hipertensao = dataCaract.First().FR_HT.Value;
                MODEL.FR_Tabaco = dataCaract.First().FR_TB.Value;
                MODEL.FR_Hiperlipidemia = dataCaract.First().FR_HL.Value;
                MODEL.FR_Obesidade = dataCaract.First().FR_OB.Value;
                MODEL.FR_Diabetes = dataCaract.First().FR_DB.Value;
                MODEL.FR_Inactividade = dataCaract.First().FR_IN.Value;
                MODEL.FR_Heriditariedade = dataCaract.First().FR_HE.Value;
                MODEL.FR_Examescomplementares = dataCaract.First().FR_EC.Value;
                MODEL.FR_Outros = dataCaract.First().FR_OT.Value;
                //
                MODEL.OB_Actividade = dataCaract.First().OB_AC.Value;
                MODEL.OB_Controlopeso = dataCaract.First().OB_CP.Value;
                MODEL.OB_PrevenirIdade = dataCaract.First().OB_PI.Value;
                MODEL.OB_TreinoDesporto = dataCaract.First().OB_TP.Value;
                MODEL.OB_AumentarMassa = dataCaract.First().OB_AM.Value;
                MODEL.OB_BemEstar = dataCaract.First().OB_BE.Value;
                MODEL.OB_Tonificar = dataCaract.First().OB_TO.Value;
                MODEL.OB_Outros = dataCaract.First().OB_OT.Value;
            }
            // MODEL.TIPO_SANGUE_LIST = databaseManager.PES_PESSOAS_CARACT_TIPO_SANG.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.Caract_DuracaoPlanoList = databaseManager.GT_DuracaoPlano.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.DURACAO).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DURACAO.ToString() });
            MODEL.EstadoCivilList = databaseManager.PES_ESTADO_CIVIL.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PAIS_LIST = databaseManager.GRL_ENDERECO_PAIS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.CIDADE_LIST = databaseManager.GRL_ENDERECO_CIDADE.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.MUN_LIST = databaseManager.GRL_ENDERECO_MUN_DISTR.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.ENDERECO_TIPO_LIST = databaseManager.PES_TIPO_ENDERECOS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.ENDERECO_PAIS_ID = Configs.INST_MDL_ADM_VLRID_ADDR_STANDARD_COUNTRY;

            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Athletes;
            return View("Athletes/UpdateAthlete", MODEL);
        }

        // GET: GTManagement
        public ActionResult ViewAthletes(int? Id, Gestreino.Models.Athlete MODEL)
        {
            //if (!AcessControl.Authorized(AcessControl.GP_USERS_LIST_VIEW_SEARCH)) return View("Lockout");
            if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }

            var data = databaseManager.SP_PES_ENT_PESSOAS(Id, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).ToList();
            if (!data.Any()) return RedirectToAction("", "home");
            var dataCaract = databaseManager.PES_PESSOAS_CARACT.Where(x => x.PES_PESSOAS_ID == Id).ToList();
            var dataEnd = databaseManager.SP_PES_ENT_PESSOAS_ENDERECO(Id, null, null, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "R").ToList();
            MODEL.ID = Id;
            var DateofBirth = string.IsNullOrEmpty(data.First().DATA_NASCIMENTO) ? (DateTime?)null : DateTime.ParseExact(data.First().DATA_NASCIMENTO, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (DateofBirth != null)
                MODEL.Age = Converters.CalculateAge(DateofBirth.Value);


            MODEL.PES_DEFICIENCIA_LIST = databaseManager.PES_PESSOAS_CARACT_TIPO_DEF.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });

            MODEL.PES_PROFISSAO_LIST = databaseManager.PES_PROFISSOES.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PES_Contracto_LIST = databaseManager.PES_PROFISSOES_TIPO_CONTRACTO.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PES_Regime_LIST = databaseManager.PES_PROFISSOES_REGIME_TRABALHO.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.PES_FAMILIARES_GRUPOS_LIST = databaseManager.PES_FAMILIARES_GRUPOS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
           

            ViewBag.imgSrc = (string.IsNullOrEmpty(data.First().FOTOGRAFIA)) ? "/Assets/images/user-avatar.jpg" : "/" + data.First().FOTOGRAFIA;
            ViewBag.data = data;
            ViewBag.dataCaract = dataCaract;
            ViewBag.dataEnd = dataEnd;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Athletes;
            return View("Athletes/ViewAthletes", MODEL);
        }

        // Ajax Table
        [HttpPost]
        public ActionResult GetUsers()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var User = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            //var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Socio = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Telefone = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Email = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            //var Utilizador = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            // GET TABLE CONTENT

            var v = (from a in databaseManager.SP_PES_ENT_PESSOAS(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(User)) v = v.Where(a => a.LOGIN != null && a.LOGIN.ToUpper().Contains(User.ToUpper())
            || a.NOME != null && a.NOME.ToUpper().Contains(User.ToUpper()));
            //if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
            //if (!string.IsNullOrEmpty(Genero)) v = v.Where(a => a.SEXO != null && a.SEXO.ToString().Contains(Genero == "1" ? "Masculino" : "Feminino"));
            //if (!string.IsNullOrEmpty(DataNascimento)) v = v.Where(a => a.DATA_NASCIMENTO != null && a.DATA_NASCIMENTO.ToUpper().Contains(DataNascimento.Replace("-", "/").ToUpper()));
            if (!string.IsNullOrEmpty(Socio)) v = v.Where(a => a.PES_NUMERO != null && a.PES_NUMERO.ToString() == Socio);
            if (!string.IsNullOrEmpty(Telefone)) v = v.Where(a => a.TELEFONE != null && a.TELEFONE.ToString().Contains(Telefone.ToUpper()));
            if (!string.IsNullOrEmpty(Email)) v = v.Where(a => a.EMAIL != null && a.EMAIL.ToString().ToUpper().Contains(Email.ToUpper()));
            //if (!string.IsNullOrEmpty(Utilizador)) v = v.Where(a => a.GRUPO_UTILIZADORES != null && a.GRUPO_UTILIZADORES.ToUpper().Contains(Utilizador.ToUpper()));
            if (!string.IsNullOrEmpty(Insercao)) v = v.Where(a => a.INSERCAO != null && a.INSERCAO.ToUpper().Contains(Insercao.ToUpper()));
            if (!string.IsNullOrEmpty(DataInsercao)) v = v.Where(a => a.DATA_INSERCAO != null && a.DATA_INSERCAO.ToUpper().Contains(DataInsercao.Replace("-", "/").ToUpper()));
            if (!string.IsNullOrEmpty(Actualizacao)) v = v.Where(a => a.ACTUALIZACAO != null && a.ACTUALIZACAO.ToUpper().Contains(Actualizacao.ToUpper()));
            if (!string.IsNullOrEmpty(DataActualizacao)) v = v.Where(a => a.DATA_ACTUALIZACAO != null && a.DATA_ACTUALIZACAO.ToUpper().Contains(DataActualizacao.Replace("-", "/").ToUpper()));

            //ORDER RESULT SET
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                if (sortColumnDir == "asc")
                {
                    switch (sortColumn)
                    {
                        case "USER": v = v.OrderBy(s => s.LOGIN); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
                        case "SOCIO": v = v.OrderBy(s => s.SEXO); break;
                        case "TELEFONE": v = v.OrderBy(s => s.TELEFONE); break;
                        case "EMAIL": v = v.OrderBy(s => s.EMAIL); break;
                        //case "UTILIZADOR": v = v.OrderBy(s => s.GRUPO_UTILIZADORES); break;
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
                        case "USER": v = v.OrderByDescending(s => s.LOGIN); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
                        case "SOCIO": v = v.OrderByDescending(s => s.SEXO); break;
                        case "TELEFONE": v = v.OrderByDescending(s => s.TELEFONE); break;
                        case "EMAIL": v = v.OrderByDescending(s => s.EMAIL); break;
                        //case "UTILIZADOR": v = v.OrderByDescending(s => s.GRUPO_UTILIZADORES); break;
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_EDIT) ? "none" : "",
                    //AccessControlUser = !AcessControl.Authorized(AcessControl.ADM_USERS_USERS_LIST_VIEW_SEARCH) ? "none" : "",
                    Id = x.ID,
                    UtilizadorId = x.UTILIZADORES_ID,
                    NOME = x.NOME,
                    USER = x.LOGIN,
                    SOCIO = x.PES_NUMERO,
                    FOTOGRAFIA = (string.IsNullOrEmpty(x.FOTOGRAFIA)) ? "/Assets/images/user-avatar.jpg" : "/" + x.FOTOGRAFIA,
                    TELEFONE = x.TELEFONE,
                    EMAIL = x.EMAIL,
                    //UTILIZADOR = x.GRUPO_UTILIZADORES,
                    INSERCAO = x.INSERCAO,
                    DATAINSERCAO = x.DATA_INSERCAO,
                    ACTUALIZACAO = x.ACTUALIZACAO,
                    DATAACTUALIZACAO = x.DATA_ACTUALIZACAO
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }
        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewAthlete(Gestreino.Models.Athlete MODEL, string returnUrl)
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

                if (Converters.WordCount(MODEL.Nome) <= 1)
                    return Json(new { result = false, error = "Nome completo deve conter mais de uma palavra!" });

                var DateofBirth = string.IsNullOrWhiteSpace(MODEL.DataNascimento) ? (DateTime?)null : DateTime.ParseExact(MODEL.DataNascimento, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (DateofBirth != null)
                {
                    MODEL.Age = Converters.CalculateAge(DateofBirth.Value);
                    if (MODEL.Age < 15)
                        return Json(new { result = false, error = "Não pode ter uma idade inferior a 15 anos!" });
                }

                if (databaseManager.PES_CONTACTOS.Where(a => a.EMAIL == MODEL.Email).ToList().Count() > 0)
                {
                    if (!string.IsNullOrEmpty(MODEL.Email)) return Json(new { result = false, error = "Este endereço de email já encontra-se em uso!" });
                }

                Decimal Telephone = (!string.IsNullOrEmpty(MODEL.Telephone)) ? Convert.ToDecimal(MODEL.Telephone) : 0;
                Decimal TelephoneAlternativo = (!string.IsNullOrEmpty(MODEL.TelephoneAlternativo)) ? Convert.ToDecimal(MODEL.TelephoneAlternativo) : 0;
                Decimal Fax = (!string.IsNullOrEmpty(MODEL.Fax)) ? Convert.ToDecimal(MODEL.Fax) : 0; ;
                var Peso = (MODEL.Caract_Peso != null) ? decimal.Parse(MODEL.Caract_Peso, CultureInfo.InvariantCulture) : (Decimal?)null;
                var Altura = (MODEL.Caract_Altura != null) ? decimal.Parse(MODEL.Caract_Altura, CultureInfo.InvariantCulture) : (Decimal?)null;

                //Create or update User
                var Login = Converters.GetFirstAndLastName(MODEL.Nome).Replace(" ", "").ToLower();

                if (databaseManager.UTILIZADORES.Where(x => x.LOGIN == Login).Any())
                    Login = Login + "" + (databaseManager.UTILIZADORES.Count() + 1);


                // if (databaseManager.PES_CONTACTOS.Where(x => x.EMAIL == MODEL.Email).Any())
                //    return Json(new { result = false, error = "Endereço de email já se encontra registado, por favor verifique a seleção!" });


                var Status = true;
                var PasswordField = Login;
                //var DateIni = string.IsNullOrWhiteSpace(MODEL.DateAct) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateAct, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //var DateEnd = string.IsNullOrWhiteSpace(MODEL.DateDisact) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateDisact, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                // Create Salted Password
                var Salt = Crypto.GenerateSalt(64);
                var Password = Crypto.Hash(PasswordField + Salt);
                // Remove whitespaces and parse datetime strings //TrimStart() //Trim()

                // Create
                var create = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(null, null, null, Login, MODEL.Nome, Telephone, !string.IsNullOrEmpty(MODEL.Email) ? MODEL.Email.Trim() : null, Password, Salt, Status, null, null, true, int.Parse(User.Identity.GetUserId()), "C").ToList();
                // Get PesId
                var UserId = create.First().ID;
                var PesId = databaseManager.PES_PESSOAS.Where(x => x.UTILIZADORES_ID == UserId).Select(x => x.ID).FirstOrDefault();

                // Create User and Pes
                var UpdatePes = databaseManager.SP_PES_ENT_PESSOAS(PesId, MODEL.Nome, MODEL.Sexo == 1 ? "M" : "F", DateofBirth, MODEL.EstadoCivil, MODEL.NIF, null, MODEL.NAT_PAIS_ID, MODEL.NAT_CIDADE_ID, MODEL.NAT_MUN_ID, Telephone, TelephoneAlternativo, Fax, MODEL.Email, MODEL.CodigoPostal, MODEL.URL, MODEL.Numero, int.Parse(User.Identity.GetUserId()), "U").ToList();

                // Create or Update Caract
                var createCaract = databaseManager.SP_PES_ENT_PESSOAS_CARACT(null, PesId, null, Altura, Peso, MODEL.Caract_FCRepouso, MODEL.Caract_FCMaximo, MODEL.Caract_TASistolica, MODEL.Caract_TADistolica, MODEL.Caract_MassaGorda, MODEL.Caract_VO2, MODEL.Caract_DuracaoPlano, MODEL.Caract_IMC, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, MODEL.FR_Hipertensao, MODEL.FR_Tabaco, MODEL.FR_Hiperlipidemia, MODEL.FR_Obesidade, MODEL.FR_Diabetes, MODEL.FR_Inactividade, MODEL.FR_Heriditariedade, MODEL.FR_Examescomplementares, MODEL.FR_Outros, MODEL.OB_Actividade, MODEL.OB_Controlopeso, MODEL.OB_PrevenirIdade, MODEL.OB_TreinoDesporto, MODEL.OB_AumentarMassa, MODEL.OB_BemEstar, MODEL.OB_Tonificar, MODEL.OB_Outros, null, int.Parse(User.Identity.GetUserId()), "C").ToList();

                // Create or Update Address
                var createAddress = databaseManager.SP_PES_ENT_PESSOAS_ENDERECO(null, PesId, MODEL.ENDERECO_TIPO, true, MODEL.EndNumero, MODEL.Rua, MODEL.Morada, MODEL.ENDERECO_PAIS_ID, MODEL.ENDERECO_CIDADE_ID, MODEL.ENDERECO_MUN_ID, DateTime.Now, null, int.Parse(User.Identity.GetUserId()), "C").ToList();

                // Create nationality rows
                if (MODEL.NacionalidadeId != null)
                {
                    var removenationality = databaseManager.SP_PES_ENT_PESSOAS(PesId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "DN").ToList();

                    foreach (int item in MODEL.NacionalidadeId)
                    {
                        var addnationality = databaseManager.SP_PES_ENT_PESSOAS(MODEL.ID, null, null, null, null, null, null, item, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "IN").ToList();
                    }
                }
                returnUrl = "/gtmanagement/viewathletes/" + PesId;
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, url = returnUrl, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAthlete(Gestreino.Models.Athlete MODEL, string returnUrl)
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

                if (Converters.WordCount(MODEL.Nome) <= 1)
                    return Json(new { result = false, error = "Nome completo deve conter mais de uma palavra!" });

                var DateofBirth = string.IsNullOrWhiteSpace(MODEL.DataNascimento) ? (DateTime?)null : DateTime.ParseExact(MODEL.DataNascimento, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                if (DateofBirth != null)
                {
                    MODEL.Age = Converters.CalculateAge(DateofBirth.Value);
                    if (MODEL.Age < 15)
                        return Json(new { result = false, error = "Não pode ter uma idade inferior a 15 anos!" });
                }
                if (databaseManager.PES_CONTACTOS.Where(a => a.EMAIL == MODEL.Email && a.PES_PESSOAS_ID != MODEL.ID).ToList().Count() > 0)
                {
                    if (!string.IsNullOrEmpty(MODEL.Email)) return Json(new { result = false, error = "Este endereço de email já encontra-se em uso!" });
                }

                Decimal Telephone = (!string.IsNullOrEmpty(MODEL.Telephone)) ? Convert.ToDecimal(MODEL.Telephone) : 0;
                Decimal TelephoneAlternativo = (!string.IsNullOrEmpty(MODEL.TelephoneAlternativo)) ? Convert.ToDecimal(MODEL.TelephoneAlternativo) : 0;
                Decimal Fax = (!string.IsNullOrEmpty(MODEL.Fax)) ? Convert.ToDecimal(MODEL.Fax) : 0; ;
                var Peso = (MODEL.Caract_Peso != null) ? decimal.Parse(MODEL.Caract_Peso, CultureInfo.InvariantCulture) : (Decimal?)null;
                var Altura = (MODEL.Caract_Altura != null) ? decimal.Parse(MODEL.Caract_Altura, CultureInfo.InvariantCulture) : (Decimal?)null;

                // Create User and Pes
                var UpdatePes = databaseManager.SP_PES_ENT_PESSOAS(MODEL.ID, MODEL.Nome, MODEL.Sexo == 1 ? "M" : "F", DateofBirth, MODEL.EstadoCivil, MODEL.NIF, null, MODEL.NAT_PAIS_ID, MODEL.NAT_CIDADE_ID, MODEL.NAT_MUN_ID, Telephone, TelephoneAlternativo, Fax, MODEL.Email, MODEL.CodigoPostal, MODEL.URL, MODEL.Numero, int.Parse(User.Identity.GetUserId()), "U").ToList();

                // Create or Update Caract
                var updateCaract = databaseManager.SP_PES_ENT_PESSOAS_CARACT(null, MODEL.ID, null, Altura, Peso, MODEL.Caract_FCRepouso, MODEL.Caract_FCMaximo, MODEL.Caract_TASistolica, MODEL.Caract_TADistolica, MODEL.Caract_MassaGorda, MODEL.Caract_VO2, MODEL.Caract_DuracaoPlano, MODEL.Caract_IMC, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, MODEL.FR_Hipertensao, MODEL.FR_Tabaco, MODEL.FR_Hiperlipidemia, MODEL.FR_Obesidade, MODEL.FR_Diabetes, MODEL.FR_Inactividade, MODEL.FR_Heriditariedade, MODEL.FR_Examescomplementares, MODEL.FR_Outros, MODEL.OB_Actividade, MODEL.OB_Controlopeso, MODEL.OB_PrevenirIdade, MODEL.OB_TreinoDesporto, MODEL.OB_AumentarMassa, MODEL.OB_BemEstar, MODEL.OB_Tonificar, MODEL.OB_Outros, null, int.Parse(User.Identity.GetUserId()), "C").ToList();

                // Create or Update Address
                var createAddress = databaseManager.SP_PES_ENT_PESSOAS_ENDERECO(null, MODEL.ID, MODEL.ENDERECO_TIPO, true, MODEL.EndNumero, MODEL.Rua, MODEL.Morada, MODEL.ENDERECO_PAIS_ID, MODEL.ENDERECO_CIDADE_ID, MODEL.ENDERECO_MUN_ID, DateTime.Now, null, int.Parse(User.Identity.GetUserId()), "C").ToList();

                // Create nationality rows
                if (MODEL.NacionalidadeId != null)
                {
                    var removenationality = databaseManager.SP_PES_ENT_PESSOAS(MODEL.ID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "DN").ToList();

                    foreach (int item in MODEL.NacionalidadeId)
                    {
                        var addnationality = databaseManager.SP_PES_ENT_PESSOAS(MODEL.ID, null, null, null, null, null, null, item, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "IN").ToList();
                    }
                }
                returnUrl = "/gtmanagement/viewathletes/" + MODEL.ID;
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, url = returnUrl, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }


        // Get
        public ActionResult ProfilePhoto(int? Id, Athlete MODEL)
        {
            //if (AcessControl.Authorized(AcessControl.GP_USERS_ALTER_PHOTOGRAPH) || AcessControl.Authorized(AcessControl.GA_ENROLLMENTS_NEW) || AcessControl.Authorized(AcessControl.GA_ENROLLMENTS_NEW_EXCEPTION) || AcessControl.Authorized(AcessControl.GA_APPLICATIONS_ENROL_STUDENTS)) { } else return View("Lockout");
            if (Id == null || Id <= 0) { return RedirectToAction("users", "gpmanagement"); }
            var item = databaseManager.SP_PES_ENT_PESSOAS(Id, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).ToList();
            ViewBag.item = item;
            if (item.Count == 0) return RedirectToAction("users", "gpmanagement");
            MODEL.UserID = item.FirstOrDefault().UTILIZADORES_ID;
            MODEL.ID = item.FirstOrDefault().ID;

            ViewBag.imgSrc = (string.IsNullOrEmpty(item[0].FOTOGRAFIA)) ? "/Assets/images/user-avatar.jpg" : "/" + item[0].FOTOGRAFIA;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Athletes;
            return View("Athletes/ProfilePhoto", MODEL);
        }
        // Get 
        [HttpGet]
        public ActionResult WebCam()
        {
            // if (AcessControl.Authorized(AcessControl.GP_USERS_ALTER_PHOTOGRAPH) || AcessControl.Authorized(AcessControl.GA_ENROLLMENTS_NEW) || AcessControl.Authorized(AcessControl.GA_ENROLLMENTS_NEW_EXCEPTION) || AcessControl.Authorized(AcessControl.GA_APPLICATIONS_ENROL_STUDENTS)) { } else return View("Lockout");
            ViewBag.LeftBarLinkActive = 0;
            return View("Athletes/WebCam");
        }
        // Update
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateProfilePhoto(int? UserId, int? PES_PESSOA_ID, string WebcamImgBase64, HttpPostedFileBase file, string returnUrl)
        {

            //if (AcessControl.Authorized(AcessControl.GP_USERS_ALTER_PHOTOGRAPH) || AcessControl.Authorized(AcessControl.GA_ENROLLMENTS_NEW) || AcessControl.Authorized(AcessControl.GA_ENROLLMENTS_NEW_EXCEPTION) || AcessControl.Authorized(AcessControl.GA_APPLICATIONS_ENROL_STUDENTS)) { }
            //else return Json(new { result = false, error = "Acesso não autorizado!" });

            // Get Allowed size
            var allowedSize = Classes.FileUploader.TwoMB; // 2.0 MB
            // Get Document Type Id
            var tipoidentname = "Fotografia pessoal";
            var entity = "pespessoas";
            var sqlpath = string.Empty;

            int filesize = 0;
            string filetype = string.Empty;
            MemoryStream ms = new MemoryStream();

            try
            {
                if (!string.IsNullOrEmpty(WebcamImgBase64))
                {
                    var base64String = WebcamImgBase64.Split(',')[1];
                    byte[] imageBytes = Convert.FromBase64String(base64String);
                    ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);

                    filesize = Convert.ToInt32(ms.Length);
                    filetype = ".jpeg";
                }
                else
                {
                    if (file != null)
                    {
                        filesize = file.ContentLength;
                        filetype = System.IO.Path.GetExtension(file.FileName);
                    }
                }

                if (filesize > 0 /*&& filesize < Convert.ToDouble(WebConfigurationManager.AppSettings["maxRequestLength"])*/)
                {
                    // Get Module Subfolder
                    var modulestorage = FileUploader.ModuleStorage[Convert.ToInt32(FileUploader.DecoderFactory(entity)[2])];
                    // Get file size
                    var size = filesize;
                    // Get file type
                    var type = filetype.ToLower();
                    // Get directory
                    string[] DirectoryFactory = FileUploader.DirectoryFactory(modulestorage, Server.MapPath(FileUploader.FileStorage), filetype, null, tipoidentname + "-" + PES_PESSOA_ID);
                    /*
                    * 0 => sqlpath,
                    * 1 => path,
                    * 2 => filename
                    */
                    sqlpath = DirectoryFactory[0];
                    var path = DirectoryFactory[1];
                    var filename = DirectoryFactory[2];
                    //Define tablename and fieldname for Stored Procedure
                    string tablename = FileUploader.DecoderFactory(entity)[0];
                    string fieldname = FileUploader.DecoderFactory(entity)[1];

                    // Check file type
                    if (!FileUploader.allowedExtensions.Contains(type))
                        return Json(new { result = false, error = "Formato inválido!, por favor adicionar um documento válido com a capacidade permitida!" });

                    // Check file size
                    if (size > allowedSize)
                        return Json(new { result = false, error = "Tamanho do documento deve ser inferior a " + FileUploader.FormatSize(allowedSize) + "!" });

                    if (!string.IsNullOrEmpty(WebcamImgBase64)) (System.Drawing.Image.FromStream(ms, true)).Save(path, ImageFormat.Jpeg);
                    else file.SaveAs(path);

                    using (var db = databaseManager)
                    {
                        // make sure you have the right column/variable used here
                        var row = db.PES_PESSOAS.FirstOrDefault(x => x.ID == PES_PESSOA_ID);

                        if (row == null)
                            return Json(new { result = false, error = "ID inválido: " + PES_PESSOA_ID });

                        // this variable is tracked by the db context
                        row.FOTOGRAFIA = sqlpath;
                        db.SaveChanges();

                        if (UserId == int.Parse(User.Identity.GetUserId()))
                        {
                            // Update User Details
                            var claimsIdentity = User.Identity as ClaimsIdentity;
                            // check for existing claim and remove it
                            var existingClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
                            if (existingClaim != null)
                                claimsIdentity.RemoveClaim(existingClaim);
                            // update profile photo identity claim
                            claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, sqlpath));
                            var authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
                            authenticationManager.AuthenticationResponseGrant = new Microsoft.Owin.Security.AuthenticationResponseGrant(new ClaimsPrincipal(claimsIdentity), new Microsoft.Owin.Security.AuthenticationProperties() { IsPersistent = true });

                        }
                    }

                    // Return to Url
                    returnUrl = "/gpmanagement/viewusers/" + PES_PESSOA_ID;
                    ModelState.Clear();
                }
                else
                {
                    return Json(new { result = false, error = "Por favor adicionar um documento válido com a capacidade permitida!" });
                }
            }
            catch (Exception e)
            {
                return Json(new { result = false, error = e.Message });
            }
            return Json(new { result = true, imageUrl = sqlpath, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }


















        /*
     ******************************************
     *******************************************
     DADOS PESSOAIS PROFISSIONAIS :: READ
     ******************************************
     *******************************************
    */
        // Ajax Table
        [HttpPost]
        public ActionResult GetUsersProfessional(int? Id)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Empresa = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Funcao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Contracto = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Regime = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataIni = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataFim = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var Descricao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[8][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[9][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[10][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            // GET TABLE CONTENT

            var v = (from a in databaseManager.SP_PES_ENT_PESSOAS_PROFISSOES(null,Id, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Empresa)) v = v.Where(a => a.EMPRESA != null && a.EMPRESA.ToUpper().Contains(Empresa.ToUpper()));
            if (!string.IsNullOrEmpty(Funcao)) v = v.Where(a => a.PES_PROFISSOES_ID != null && a.PES_PROFISSOES_ID.ToString() == Funcao);
            if (!string.IsNullOrEmpty(Contracto)) v = v.Where(a => a.PES_PROFISSOES_TIPO_CONTRACTO_ID != null && a.PES_PROFISSOES_TIPO_CONTRACTO_ID.ToString() == Contracto);
            if (!string.IsNullOrEmpty(Regime)) v = v.Where(a => a.PES_PROFISSOES_REGIME_TRABALHO_ID != null && a.PES_PROFISSOES_REGIME_TRABALHO_ID.ToString() == Regime);
            //if (!string.IsNullOrEmpty(Unidade)) v = v.Where(a => a.INST_UNIDADES_ID != null && a.INST_UNIDADES_ID.ToString() == Unidade);
            //if (!string.IsNullOrEmpty(Espaco)) v = v.Where(a => a.INST_ESPACOS_ID != null && a.INST_ESPACOS_ID.ToString() == Espaco);
            if (!string.IsNullOrEmpty(DataIni)) v = v.Where(a => a.DATA_INICIO != null && a.DATA_INICIO.ToUpper().Contains(DataIni.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse
            if (!string.IsNullOrEmpty(DataFim)) v = v.Where(a => a.DATA_FIM != null && a.DATA_FIM.ToUpper().Contains(DataFim.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse
            if (!string.IsNullOrEmpty(Descricao)) v = v.Where(a => a.DESCRICAO != null && a.DESCRICAO.ToUpper().ToString().Contains(Descricao.ToUpper()));
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
                        case "EMPRESA": v = v.OrderBy(s => s.EMPRESA); break;
                        case "FUNCAO": v = v.OrderBy(s => s.PROFISSAO); break;
                        case "CONTRACTO": v = v.OrderBy(s => s.CONT_NOME); break;
                        case "REGIME": v = v.OrderBy(s => s.REGIME_NOME); break;
                        case "DATAINICIAL": v = v.OrderBy(s => s.DATA_INICIO); break;
                        case "DATAFIM": v = v.OrderBy(s => s.DATA_FIM); break;
                        case "DESCRICAO": v = v.OrderBy(s => s.DESCRICAO); break;
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
                        case "EMPRESA": v = v.OrderByDescending(s => s.EMPRESA); break;
                        case "FUNCAO": v = v.OrderByDescending(s => s.PROFISSAO); break;
                        case "CONTRACTO": v = v.OrderByDescending(s => s.CONT_NOME); break;
                        case "REGIME": v = v.OrderByDescending(s => s.REGIME_NOME); break;
                        case "DATAINICIAL": v = v.OrderByDescending(s => s.DATA_INICIO); break;
                        case "DATAFIM": v = v.OrderByDescending(s => s.DATA_FIM); break;
                        case "DESCRICAO": v = v.OrderByDescending(s => s.DESCRICAO); break;
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_PROFESSIONAL_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_PROFESSIONAL_DELETE) ? "none" : "",
                    Id = x.ID,
                    EMPRESA = x.EMPRESA,
                    FUNCAO = x.PROFISSAO,
                    CONTRACTO = x.CONT_NOME,
                    REGIME = x.REGIME_NOME,
                    DATAINICIAL = x.DATA_INICIO,
                    DATAFIM = x.DATA_FIM,
                    DESCRICAO = Converters.StripHTML(x.DESCRICAO),
                    INSERCAO = x.INSERCAO,
                    DATAINSERCAO = x.DATA_INSERCAO,
                    ACTUALIZACAO = x.ACTUALIZACAO,
                    DATAACTUALIZACAO = x.DATA_ACTUALIZACAO
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }
        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProfessional(PES_Dados_Pessoais_Professional MODEL)
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
                if (!string.IsNullOrWhiteSpace(MODEL.DateEnd) && DateTime.ParseExact(MODEL.DateEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(MODEL.DateIni, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                {
                    return Json(new { result = false, error = "Data Inicial deve ser inferior a Data de Fim!" });
                }

                var DateIni = string.IsNullOrWhiteSpace(MODEL.DateIni) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateIni, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DateEnd = string.IsNullOrWhiteSpace(MODEL.DateEnd) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string Empresa = MODEL.Empresa;

                // Create
                var create = databaseManager.SP_PES_ENT_PESSOAS_PROFISSOES(null,MODEL.ID, MODEL.PES_PROFISSOES_REGIME_ID, MODEL.PES_PROFISSOES_CONTRACTO_ID, MODEL.PES_PROFISSAO_ID,Empresa, DateIni, DateEnd, MODEL.Descricao, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserProfissaoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfessional(PES_Dados_Pessoais_Professional MODEL)
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
                if (!string.IsNullOrWhiteSpace(MODEL.DateEnd) && DateTime.ParseExact(MODEL.DateEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(MODEL.DateIni, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                {
                    return Json(new { result = false, error = "Data Inicial deve ser inferior a Data de Fim!" });
                }

                var DateIni = string.IsNullOrWhiteSpace(MODEL.DateIni) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateIni, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DateEnd = string.IsNullOrWhiteSpace(MODEL.DateEnd) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //bool intext = (MODEL.INT_EXT == "Interno") ? intext = true : false;
                string Empresa = MODEL.Empresa;

                // Update
                var update = databaseManager.SP_PES_ENT_PESSOAS_PROFISSOES(MODEL.ID, null,MODEL.PES_PROFISSOES_REGIME_ID, MODEL.PES_PROFISSOES_CONTRACTO_ID, MODEL.PES_PROFISSAO_ID, Empresa, DateIni, DateEnd, MODEL.Descricao, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserProfissaoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProfessional(int[] Ids)
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
                if (Ids.Length == 0)
                    return Json(new { result = false, error = "Nenhum item selecionado para remoção!" });

                foreach (var i in Ids)
                {
                    var delete = databaseManager.SP_PES_ENT_PESSOAS_PROFISSOES(i, null, null, null, null, null, null, null, null, null, Convert.ToChar('D').ToString()).ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserProfissaoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }





        /*
        ******************************************
        *******************************************
        DADOS PESSOAIS FAMILIARES :: READ
        ******************************************
        *******************************************
       */
        // Ajax Table
        [HttpPost]
        public ActionResult GetUsersFamily(int? Id)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Agregado = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Profissao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Telefone = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var TelefoneAlternativo = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Fax = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var Email = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
            var URL = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();
            var Endereco = Request.Form.GetValues("columns[8][search][value]").FirstOrDefault();
            var Morada = Request.Form.GetValues("columns[9][search][value]").FirstOrDefault();
            var Rua = Request.Form.GetValues("columns[10][search][value]").FirstOrDefault();
            var Numero = Request.Form.GetValues("columns[11][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[12][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[13][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[14][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[15][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            // GET TABLE CONTENT

            var v = (from a in databaseManager.SP_PES_ENT_PESSOAS_FAM(null,Id, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Agregado)) v = v.Where(a => a.PES_FAMILIARES_GRUPOS_ID != null && a.PES_FAMILIARES_GRUPOS_ID.ToString() == Agregado);
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
            if (!string.IsNullOrEmpty(Profissao)) v = v.Where(a => a.PES_PROFISSOES_ID != null && a.PES_PROFISSOES_ID.ToString() == Profissao);
            if (!string.IsNullOrEmpty(Telefone)) v = v.Where(a => a.TELEFONE != null && a.TELEFONE.ToString().Contains(Telefone.ToUpper()));
            if (!string.IsNullOrEmpty(TelefoneAlternativo)) v = v.Where(a => a.TELEFONE_ALTERNATIVO != null && a.TELEFONE_ALTERNATIVO.ToString().Contains(TelefoneAlternativo.ToUpper()));
            if (!string.IsNullOrEmpty(Fax)) v = v.Where(a => a.FAX != null && a.FAX.ToString().Contains(Fax.ToUpper()));
            if (!string.IsNullOrEmpty(Email)) v = v.Where(a => a.EMAIL != null && a.EMAIL.ToString().Contains(Email.ToUpper()));
            if (!string.IsNullOrEmpty(URL)) v = v.Where(a => a.URL != null && a.URL.ToString().Contains(URL.ToUpper()));
            if (!string.IsNullOrEmpty(Endereco)) v = v.Where(a => a.PAIS_NOME != null && (a.PAIS_NOME.ToUpper() + " " + a.CIDADE_NOME.ToUpper() + " " + a.MUN_NOME.ToUpper()).Contains(Endereco.ToUpper()));
            if (!string.IsNullOrEmpty(Morada)) v = v.Where(a => a.MORADA != null && a.MORADA.ToString().Contains(Morada.ToUpper()));
            if (!string.IsNullOrEmpty(Rua)) v = v.Where(a => a.RUA != null && a.RUA.ToString().ToUpper().Contains(Rua.ToUpper()));
            if (!string.IsNullOrEmpty(Numero)) v = v.Where(a => a.NUMERO != null && a.NUMERO.ToString().Contains(Numero.ToUpper()));
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
                        case "AGREGADO": v = v.OrderBy(s => s.PES_FAMILIARES_GRUPOS_ID); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
                        case "PROFISSAO": v = v.OrderBy(s => s.PES_PROFISSOES_ID); break;
                        case "TELEFONE": v = v.OrderBy(s => s.TELEFONE); break;
                        case "TELEFONEALTERNATIVO": v = v.OrderBy(s => s.TELEFONE_ALTERNATIVO); break;
                        case "FAX": v = v.OrderBy(s => s.FAX); break;
                        case "EMAIL": v = v.OrderBy(s => s.EMAIL); break;
                        case "URL": v = v.OrderBy(s => s.URL); break;
                        case "ENDERECO": v = v.OrderBy(s => s.CIDADE_NOME); break;
                        case "MORADA": v = v.OrderBy(s => s.MORADA); break;
                        case "RUA": v = v.OrderBy(s => s.RUA); break;
                        case "NUMERO": v = v.OrderBy(s => s.NUMERO); break;
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
                        case "AGREGADO": v = v.OrderByDescending(s => s.PES_FAMILIARES_GRUPOS_ID); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
                        case "PROFISSAO": v = v.OrderByDescending(s => s.PES_PROFISSOES_ID); break;
                        case "TELEFONE": v = v.OrderByDescending(s => s.TELEFONE); break;
                        case "TELEFONEALTERNATIVO": v = v.OrderByDescending(s => s.TELEFONE_ALTERNATIVO); break;
                        case "FAX": v = v.OrderByDescending(s => s.FAX); break;
                        case "EMAIL": v = v.OrderByDescending(s => s.EMAIL); break;
                        case "URL": v = v.OrderByDescending(s => s.URL); break;
                        case "ENDERECO": v = v.OrderByDescending(s => s.CIDADE_NOME); break;
                        case "MORADA": v = v.OrderByDescending(s => s.MORADA); break;
                        case "RUA": v = v.OrderByDescending(s => s.RUA); break;
                        case "NUMERO": v = v.OrderByDescending(s => s.NUMERO); break;
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_FAM_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_FAM_DELETE) ? "none" : "",
                    Id = x.ID,
                    AGREGADO = x.AGREGADO,
                    NOME = x.NOME,
                    PROFISSAO = x.PROFISSAO,
                    TELEFONE = x.TELEFONE == 0 ? null : x.TELEFONE,
                    TELEFONEALTERNATIVO = x.TELEFONE_ALTERNATIVO,
                    FAX = x.FAX,
                    EMAIL = x.EMAIL,
                    URL = x.URL,
                    ENDERECO = x.MUN_NOME + " " + x.CIDADE_NOME + " " + x.PAIS_NOME,
                    MORADA = x.MORADA,
                    RUA = x.RUA,
                    NUMERO = x.NUMERO,
                    INSERCAO = x.INSERCAO,
                    DATAINSERCAO = x.DATA_INSERCAO,
                    ACTUALIZACAO = x.ACTUALIZACAO,
                    DATAACTUALIZACAO = x.DATA_ACTUALIZACAO
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }
        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFamilyAgregado(PES_Dados_Pessoais_Agregado MODEL)
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
                if (databaseManager.PES_PESSOAS_FAM.Where(a => a.PES_PESSOAS_ID == MODEL.ID && a.PES_FAMILIARES_GRUPOS_ID == MODEL.PES_FAMILIARES_GRUPOS_ID).ToList().Count() > 0)
                {
                    return Json(new { result = false, error = "Agregado já encontra-se registado!" });
                }

                Decimal Telephone = (!string.IsNullOrEmpty(MODEL.Telephone)) ? Convert.ToDecimal(MODEL.Telephone) : 0;
                Decimal TelephoneAlternativo = (!string.IsNullOrEmpty(MODEL.TelephoneAlternativo)) ? Convert.ToDecimal(MODEL.TelephoneAlternativo) : 0;
                Decimal Fax = (!string.IsNullOrEmpty(MODEL.Fax)) ? Convert.ToDecimal(MODEL.Fax) : 0; ;

                // Create
                var create = databaseManager.SP_PES_ENT_PESSOAS_FAM(null,MODEL.ID, MODEL.PES_FAMILIARES_GRUPOS_ID, MODEL.PES_PROFISSAO_ID, MODEL.Nome, Telephone, TelephoneAlternativo, Fax, (!string.IsNullOrEmpty(MODEL.Email)) ? MODEL.Email.Trim().ToLower() : MODEL.Email, (!string.IsNullOrEmpty(MODEL.URL)) ? MODEL.URL.Trim().ToLower() : MODEL.URL, MODEL.Numero, !string.IsNullOrEmpty(MODEL.Rua) ? MODEL.Rua.Trim() : MODEL.Rua, !string.IsNullOrEmpty(MODEL.Morada) ? MODEL.Morada.Trim() : MODEL.Morada, MODEL.PaisId, MODEL.CidadeId, MODEL.DistrictoId, MODEL.Isento, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserFamilyTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateFamilyAgregado(PES_Dados_Pessoais_Agregado MODEL)
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
                if (databaseManager.PES_PESSOAS_FAM.Where(a => a.ID != MODEL.ID && a.PES_FAMILIARES_GRUPOS_ID == MODEL.PES_FAMILIARES_GRUPOS_ID && a.PES_PESSOAS_ID == MODEL.PES_PESSOAS_ID).ToList().Count() > 0)
                {
                    return Json(new { result = false, error = "Agregado já encontra-se registado!" });
                }

                Decimal Telephone = (!string.IsNullOrEmpty(MODEL.Telephone)) ? Convert.ToDecimal(MODEL.Telephone) : 0;
                Decimal TelephoneAlternativo = (!string.IsNullOrEmpty(MODEL.TelephoneAlternativo)) ? Convert.ToDecimal(MODEL.TelephoneAlternativo) : 0;
                Decimal Fax = (!string.IsNullOrEmpty(MODEL.Fax)) ? Convert.ToDecimal(MODEL.Fax) : 0; ;

                // Update
                var update = databaseManager.SP_PES_ENT_PESSOAS_FAM(MODEL.ID,null, MODEL.PES_FAMILIARES_GRUPOS_ID, MODEL.PES_PROFISSAO_ID, MODEL.Nome, Telephone, TelephoneAlternativo, Fax, (!string.IsNullOrEmpty(MODEL.Email)) ? MODEL.Email.Trim().ToLower() : MODEL.Email, (!string.IsNullOrEmpty(MODEL.URL)) ? MODEL.URL.Trim().ToLower() : MODEL.URL, MODEL.Numero, !string.IsNullOrEmpty(MODEL.Rua) ? MODEL.Rua.Trim() : MODEL.Rua, !string.IsNullOrEmpty(MODEL.Morada) ? MODEL.Morada.Trim() : MODEL.Morada, MODEL.PaisId, MODEL.CidadeId, MODEL.DistrictoId, MODEL.Isento, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserFamilyTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFamilyAgregado(int[] Ids)
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
                if (Ids.Length == 0)
                    return Json(new { result = false, error = "Nenhum item selecionado para remoção!" });

                foreach (var i in Ids)
                {
                    var delete = databaseManager.SP_PES_ENT_PESSOAS_FAM(i, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('D').ToString()).ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserFamilyTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }




        /*
    ******************************************
    *******************************************
    DADOS PESSOAIS DEFICIENCIAS :: READ
    ******************************************
    *******************************************
   */
        // Ajax Table
        [HttpPost]
        public ActionResult GetUsersDisability(int? Id)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Deficiencia = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Grau = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Descricao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            // GET TABLE CONTENT

            var v = (from a in databaseManager.SP_PES_ENT_PESSOAS_DEFICIENCIA(null,Id, null,null, null, null, Convert.ToChar('R').ToString()).ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Deficiencia)) v = v.Where(a => a.PES_PESSOAS_CARACT_TIPO_DEF_ID != null && a.PES_PESSOAS_CARACT_TIPO_DEF_ID.ToString() == Deficiencia);
            if (!string.IsNullOrEmpty(Grau)) v = v.Where(a => a.PES_PESSOAS_CARACT_GRAU_DEF_ID != null && a.PES_PESSOAS_CARACT_GRAU_DEF_ID.ToString() == Grau);
            if (!string.IsNullOrEmpty(Descricao)) v = v.Where(a => a.DESCRICAO != null && a.DESCRICAO.ToUpper().Contains(Descricao.ToUpper()));
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
                        case "DEFICIENCIA": v = v.OrderBy(s => s.PES_PESSOAS_CARACT_TIPO_DEF_ID); break;
                        case "GRAU": v = v.OrderBy(s => s.PES_PESSOAS_CARACT_GRAU_DEF_ID); break;
                        case "DESCRICAO": v = v.OrderBy(s => s.DESCRICAO); break;
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
                        case "DEFICIENCIA": v = v.OrderByDescending(s => s.PES_PESSOAS_CARACT_TIPO_DEF_ID); break;
                        case "GRAU": v = v.OrderByDescending(s => s.PES_PESSOAS_CARACT_GRAU_DEF_ID); break;
                        case "DESCRICAO": v = v.OrderByDescending(s => s.DESCRICAO); break;
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_DEFICIENCY_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_DEFICIENCY_DELETE) ? "none" : "",
                    Id = x.ID,
                    DEFICIENCIA = x.NOME,
                    GRAU = x.GRAU,
                    DESCRICAO = Converters.StripHTML(x.DESCRICAO),
                    INSERCAO = x.INSERCAO,
                    DATAINSERCAO = x.DATA_INSERCAO,
                    ACTUALIZACAO = x.ACTUALIZACAO,
                    DATAACTUALIZACAO = x.DATA_ACTUALIZACAO
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }
        // Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDisability(PES_Dados_Pessoais_Deficiencia MODEL)
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
                if (databaseManager.PES_PESSOAS_CARACT_DEFICIENCIA.Where(a => a.PES_PESSOAS_ID == MODEL.ID && a.PES_PESSOAS_CARACT_TIPO_DEF_ID == MODEL.PES_DEFICIENCIA_ID && a.PES_PESSOAS_CARACT_GRAU_DEF_ID == MODEL.PES_DEFICIENCIA_GRAU_ID).ToList().Count() > 0)
                {
                    return Json(new { result = false, error = "Este tipo de deficiência e grau já encontra-se registado!" });
                }

                // Create
                var create = databaseManager.SP_PES_ENT_PESSOAS_DEFICIENCIA(null,MODEL.ID,MODEL.PES_DEFICIENCIA_ID, MODEL.PES_DEFICIENCIA_GRAU_ID, (!string.IsNullOrEmpty(MODEL.Descricao)) ? MODEL.Descricao.Trim().ToLower() : MODEL.Descricao, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserDisabilityTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDisability(PES_Dados_Pessoais_Deficiencia MODEL)
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
                if (databaseManager.PES_PESSOAS_CARACT_DEFICIENCIA.Where(a => a.ID != MODEL.ID && a.PES_PESSOAS_CARACT_TIPO_DEF_ID == MODEL.PES_DEFICIENCIA_ID && a.PES_PESSOAS_CARACT_GRAU_DEF_ID == MODEL.PES_DEFICIENCIA_GRAU_ID).ToList().Count() > 0)
                {
                    return Json(new { result = false, error = "Este tipo de deficiência e grau já encontra-se registado!" });
                }

                // Update
                var update = databaseManager.SP_PES_ENT_PESSOAS_DEFICIENCIA(MODEL.ID, null,MODEL.PES_DEFICIENCIA_ID, MODEL.PES_DEFICIENCIA_GRAU_ID, (!string.IsNullOrEmpty(MODEL.Descricao)) ? MODEL.Descricao.Trim().ToLower() : MODEL.Descricao, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserDisabilityTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDisability(int[] Ids)
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
                if (Ids.Length == 0)
                    return Json(new { result = false, error = "Nenhum item selecionado para remoção!" });

                foreach (var i in Ids)
                {
                    var delete = databaseManager.SP_PES_ENT_PESSOAS_DEFICIENCIA(i, null, null,null, null, null, Convert.ToChar('D').ToString()).ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserDisabilityTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }






    }
}