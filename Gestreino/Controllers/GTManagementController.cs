﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Gestreino.Classes;
using Gestreino.Models;
using JeanPiagetSGA;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
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
//
using System.Reflection;
using DocumentFormat.OpenXml.Drawing.Charts;
using static Gestreino.Models.CoronaryRisk;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;

namespace Gestreino.Controllers
{
    [Authorize]
    public class GTManagementController : Controller
    {
        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();
        private System.Collections.Specialized.StringDictionary DictRespostas;

        // _MenuLeftBarLink
        int _MenuLeftBarLink_Athletes = 201;
        int _MenuLeftBarLink_PlanBodyMass = 202;
        int _MenuLeftBarLink_PlanCardio = 203;
        int _MenuLeftBarLink_Exercices = 204;
        int _MenuLeftBarLink_Quest_Anxient = 205;
        int _MenuLeftBarLink_Quest_SelfConcept = 206;
        int _MenuLeftBarLink_Quest_CoronaryRisk = 207;
        int _MenuLeftBarLink_Quest_Health= 208;
        int _MenuLeftBarLink_Quest_Flex = 209;
        int _MenuLeftBarLink_Quest_BodyComposition = 210;
        int _MenuLeftBarLink_Quest_Cardio = 211;
        int _MenuLeftBarLink_FileManagement = 0;

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
            //MODEL.ENDERECO_PAIS_ID = Configs.INST_MDL_ADM_VLRID_ADDR_STANDARD_COUNTRY;

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

                if (Peso!=null && Altura!=null)
                    MODEL.Caract_IMC = Convert.ToInt32(Peso / ((Altura / 100) * (Altura / 100)));
              
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
                        var addnationality = databaseManager.SP_PES_ENT_PESSOAS(PesId, null, null, null, null, null, null, item, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "IN").ToList();
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

        // Update
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

                if (Peso != null && Altura != null)
                    MODEL.Caract_IMC = Convert.ToInt32(Peso / ((Altura / 100) * (Altura / 100)));

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





        /*
        ******************************************
        *******************************************
        DADOS PESSOAIS IDENTIFICACAO :: READ
        ******************************************
        *******************************************
       */
        // Ajax Table
        [HttpPost]
        public ActionResult GetUsersIdentification(int? Id)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Identificacao = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Numero = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var DataEmissao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataValidade = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var LocalEmissao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var OrgaoEmissao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var Observacao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[8][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[9][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[10][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            // GET TABLE CONTENT

            var v = (from a in databaseManager.SP_PES_ENT_PESSOAS_IDENTIFICACAO(Id, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Identificacao)) v = v.Where(a => a.IDENTIFICACAO_ID != null && a.IDENTIFICACAO_ID.ToString() == Identificacao);
            if (!string.IsNullOrEmpty(Numero)) v = v.Where(a => a.NUMERO != null && a.NUMERO.ToUpper().Contains(Numero.ToUpper()));
            if (!string.IsNullOrEmpty(DataEmissao)) v = v.Where(a => a.DATA_EMISSAO != null && a.DATA_EMISSAO.ToUpper().Contains(DataEmissao.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse
            if (!string.IsNullOrEmpty(DataValidade)) v = v.Where(a => a.DATA_VALIDADE != null && a.DATA_VALIDADE.ToUpper().Contains(DataValidade.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse
            if (!string.IsNullOrEmpty(LocalEmissao)) v = v.Where(a => a.PAIS != null && (a.PAIS.ToUpper() + " " + a.CIDADE.ToUpper() + " " + a.MUN.ToUpper()).Contains(LocalEmissao.ToUpper()));
            if (!string.IsNullOrEmpty(OrgaoEmissao)) v = v.Where(a => a.ORGAO_EMISSOR != null && a.ORGAO_EMISSOR.ToUpper().Contains(OrgaoEmissao.ToUpper()));
            if (!string.IsNullOrEmpty(Observacao)) v = v.Where(a => a.OBSERVACOES != null && a.OBSERVACOES.ToString().Contains(Observacao.ToUpper()));
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
                        case "IDENTIFICACAO": v = v.OrderBy(s => s.IDENTIFICACAO); break;
                        case "NUMERO": v = v.OrderBy(s => s.NUMERO); break;
                        case "DATAEMISSAO": v = v.OrderBy(s => s.DATA_EMISSAO); break;
                        case "DATAVALIDADE": v = v.OrderBy(s => s.DATA_VALIDADE); break;
                        case "LOCALEMISSAO": v = v.OrderBy(s => s.CIDADE); break;
                        case "OBSERVACAO": v = v.OrderBy(s => s.OBSERVACOES); break;
                        case "ORGAOEMISSOR": v = v.OrderBy(s => s.ORGAO_EMISSOR); break;
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
                        case "IDENTIFICACAO": v = v.OrderByDescending(s => s.IDENTIFICACAO); break;
                        case "NUMERO": v = v.OrderByDescending(s => s.NUMERO); break;
                        case "DATAEMISSAO": v = v.OrderByDescending(s => s.DATA_EMISSAO); break;
                        case "DATAVALIDADE": v = v.OrderByDescending(s => s.DATA_VALIDADE); break;
                        case "LOCALEMISSAO": v = v.OrderByDescending(s => s.CIDADE); break;
                        case "OBSERVACAO": v = v.OrderByDescending(s => s.OBSERVACOES); break;
                        case "ORGAOEMISSOR": v = v.OrderByDescending(s => s.ORGAO_EMISSOR); break;
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_IDENTIFICATION_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_IDENTIFICATION_DELETE) ? "none" : "",
                    Id = x.ID,
                    IDENTIFICACAO = x.IDENTIFICACAO,
                    NUMERO = x.NUMERO,
                    DATAEMISSAO = x.DATA_EMISSAO,
                    DATAVALIDADE = x.DATA_VALIDADE,
                    LOCALEMISSAO = x.MUN + " " + x.CIDADE + " " + x.PAIS,
                    ORGAOEMISSOR = x.ORGAO_EMISSOR,
                    OBSERVACAO = Converters.StripHTML(x.OBSERVACOES),
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
        public ActionResult AddIdentification(HttpPostedFileBase file, PES_Dados_Pessoais_Ident MODEL)
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
                if (!string.IsNullOrWhiteSpace(MODEL.DateExpire) && DateTime.ParseExact(MODEL.DateExpire, "dd-MM-yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(MODEL.DateIssue, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                {
                    return Json(new { result = false, error = "Data de Emissão deve ser inferior a Data de Validade!" });
                }
                if (databaseManager.PES_IDENTIFICACAO.Where(a => a.PES_TIPO_IDENTIFICACAO_ID == MODEL.PES_TIPO_IDENTIFICACAO && a.PES_PESSOAS_ID == MODEL.ID).ToList().Count() > 0)
                {
                    return Json(new { result = false, error = "Tipo de Identificação pessoal já encontra-se registada!" });
                }

                var DateIni = string.IsNullOrWhiteSpace(MODEL.DateIssue) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateIssue, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DateEnd = string.IsNullOrWhiteSpace(MODEL.DateExpire) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateExpire, "dd-MM-yyyy", CultureInfo.InvariantCulture);


                // Validate BI number of digits
                //if (MODEL.PES_TIPO_IDENTIFICACAO == Classes.Configs.INST_MDL_ADM_VLRID_TIPODOC_BI && MODEL.Numero.Length < Classes.Configs.INST_MDL_GP_BI_MAXLENGTH)
                //    return Json(new { result = false, error = "Documento de Identificação (BI) deve conter 14 Dígitos!" });

                // Validate IDs number of digits
                if (MODEL.Numero.Length > Classes.Configs.INST_MDL_GP_BI_MAXLENGTH)
                    return Json(new { result = false, error = "Número de Identificação deve conter menos de 14 Dígitos!" });

                // Validate Identity Document Issue Date
                if (DateIni >= DateEnd)
                    return Json(new { result = false, error = "Data de Validade do Documento de Identificação inferior a data de Emissão!" });

                if (DateEnd < DateTime.Now)
                    return Json(new { result = false, error = "Data de Validade do documento de identificação vencida!" });

                if (databaseManager.PES_IDENTIFICACAO.Where(a => a.PES_TIPO_IDENTIFICACAO_ID == MODEL.PES_TIPO_IDENTIFICACAO && a.PES_PESSOAS_ID == MODEL.ID).ToList().Count() > 0)
                    return Json(new { result = false, error = "Tipo de Identificação pessoal já encontra-se registada!" });


                // Get Allowed size
                var allowedSize = Classes.FileUploader.TwoMB; // 2.0 MB
                var entity = "pespessoas";

                if (file != null)
                {
                    if (file.ContentLength > 0 && file.ContentLength < Convert.ToDouble(WebConfigurationManager.AppSettings["maxRequestLength"]))
                    {
                        // Get Module Subfolder
                        var modulestorage = FileUploader.ModuleStorage[Convert.ToInt32(FileUploader.DecoderFactory(entity)[2])];

                        // Get Document Type Id
                        var tipoidentname = databaseManager.PES_TIPO_IDENTIFICACAO.Where(x => x.ID == MODEL.PES_TIPO_IDENTIFICACAO).Select(x => x.NOME).FirstOrDefault();
                        var tipodoc = string.Empty;
                        var tipodocid = 0;
                        if (databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.NOME == tipoidentname).ToList().Count > 0)
                        {
                            tipodoc = databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.NOME == tipoidentname).Select(x => x.NOME).FirstOrDefault().ToLower();
                            tipodocid = databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.NOME == tipoidentname).Select(x => x.ID).FirstOrDefault();
                        }
                        else
                            return Json(new { result = false, error = "Arquivo " + tipoidentname + " não encontrado, certifique-se que o mesmo seja registado nas parametrizações!" });

                        // Get file size
                        var size = file.ContentLength;
                        // Get file type
                        var type = System.IO.Path.GetExtension(file.FileName).ToLower();
                        // Get directory
                        string[] DirectoryFactory = FileUploader.DirectoryFactory(modulestorage, Server.MapPath(FileUploader.FileStorage), Path.GetExtension(file.FileName), tipodoc, tipoidentname + "-" + MODEL.Numero);
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

                        var Active = true;

                        // Upload file to folder
                        file.SaveAs(path);
                        // Create file reference in SQL Database
                        var createFile = databaseManager.SP_ASSOC_ARQUIVOS(MODEL.ID, null, tipoidentname + " - " + MODEL.Numero.ToUpper(), null, Active, null, null, tipodocid, filename, null, type, size, sqlpath, tablename, fieldname, int.Parse(User.Identity.GetUserId()), Convert.ToChar('C').ToString()).ToList();
                    }
                    else
                    {
                        return Json(new { result = false, error = "Por favor adicionar um documento válido com a capacidade permitida!" });
                    }
                }

                // Create
                var create = databaseManager.SP_PES_ENT_PESSOAS_IDENTIFICACAO(MODEL.ID, MODEL.PES_TIPO_IDENTIFICACAO, MODEL.Numero.ToUpper(), DateIni, DateEnd, MODEL.Observacao, MODEL.OrgaoEmissor, MODEL.PaisId, MODEL.CidadeId, null, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserIdentTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateIdentification(PES_Dados_Pessoais_Ident MODEL)
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
                if (!string.IsNullOrWhiteSpace(MODEL.DateExpire) && DateTime.ParseExact(MODEL.DateExpire, "dd-MM-yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(MODEL.DateIssue, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                {
                    return Json(new { result = false, error = "Data de Emissão deve ser inferior a Data de Validade!" });
                }
                if (databaseManager.PES_IDENTIFICACAO.Where(a => a.PES_TIPO_IDENTIFICACAO_ID == MODEL.PES_TIPO_IDENTIFICACAO && a.PES_PESSOAS_ID == MODEL.PES_PESSOAS_ID && a.ID != MODEL.ID).ToList().Count() > 0)
                    return Json(new { result = false, error = "Tipo de Identificação pessoal já encontra-se registada!" });

                var DateIni = string.IsNullOrWhiteSpace(MODEL.DateIssue) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateIssue, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DateEnd = string.IsNullOrWhiteSpace(MODEL.DateExpire) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateExpire, "dd-MM-yyyy", CultureInfo.InvariantCulture);


                // Validate BI number of digits
                //if (MODEL.PES_TIPO_IDENTIFICACAO == Classes.Configs.INST_MDL_ADM_VLRID_ARQUIVO_LOGOTIPO && MODEL.Numero.Length < Classes.Configs.INST_MDL_GP_BI_MAXLENGTH)
                //    return Json(new { result = false, error = "Documento de Identificação (BI) deve conter 14 Dígitos!" });

                // Validate IDs number of digits
                if (MODEL.Numero.Length > Classes.Configs.INST_MDL_GP_BI_MAXLENGTH)
                    return Json(new { result = false, error = "Número de Identificação deve conter menos de 14 Dígitos!" });

                // Validate Identity Document Issue Date
                if (DateIni >= DateEnd)
                    return Json(new { result = false, error = "Data de Validade do Documento de Identificação inferior a data de Emissão!" });

                if (DateEnd < DateTime.Now)
                    return Json(new { result = false, error = "Data de Validade do documento de identificação vencida!" });

                // Update
                var update = databaseManager.SP_PES_ENT_PESSOAS_IDENTIFICACAO(MODEL.ID, MODEL.PES_TIPO_IDENTIFICACAO, MODEL.Numero.ToUpper(), DateIni, DateEnd, MODEL.Observacao, MODEL.OrgaoEmissor, MODEL.PaisId, MODEL.CidadeId, null, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserIdentTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteIdentification(int[] Ids)
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
                    var delete = databaseManager.SP_PES_ENT_PESSOAS_IDENTIFICACAO(i, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('D').ToString()).ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserIdentTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }






        // GT Exercicios
        // GET: GTManagement
        public ActionResult Exercises(Gestreino.Models.GTExercicio MODEL)
        {
            MODEL.TipoList = databaseManager.GT_TipoTreino.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Exercices;
            return View("Plans/Exercises/Index", MODEL);
        }
        public ActionResult ViewExercises(int? Id, Gestreino.Models.GTExercicio MODEL)
        {
            //if (!AcessControl.Authorized(AcessControl.GP_USERS_LIST_VIEW_SEARCH)) return View("Lockout");
            if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }

            var data = databaseManager.SP_GT_ENT_EXERCICIO(Id,null, null, null, null, null, Convert.ToChar('R').ToString()).ToList();
            if (!data.Any()) return RedirectToAction("", "home");
            MODEL.ID = Id;
          
            ViewBag.data = data;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Exercices;
            return View("Plans/Exercises/ViewExercise", MODEL);
        }
        [HttpPost]
        public ActionResult GetGRLExercicioTable()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Treino = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Alongamento = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Sequencia = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GT_ENT_EXERCICIO(null, null, null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Treino)) v = v.Where(a => a.GT_TipoTreino_ID != null && a.GT_TipoTreino_ID.ToString() == Treino);
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.nome != null && a.nome.ToUpper() == Nome.ToUpper());
            if (!string.IsNullOrEmpty(Alongamento)) v = v.Where(a => a.ALONGAMENTO != null && a.ALONGAMENTO.ToString() == Alongamento);
            if (!string.IsNullOrEmpty(Sequencia)) v = v.Where(a => a.SEQUENCIA != null && a.SEQUENCIA.ToString() == Sequencia);
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
                        case "TREINO": v = v.OrderBy(s => s.tr_nome); break;
                        case "NOME": v = v.OrderBy(s => s.nome); break;
                        case "ALONGAMENTO": v = v.OrderBy(s => s.ALONGAMENTO); break;
                        case "SEQUENCIA": v = v.OrderBy(s => s.SEQUENCIA); break;
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
                        case "TREINO": v = v.OrderByDescending(s => s.tr_nome); break;
                        case "NOME": v = v.OrderByDescending(s => s.nome); break;
                        case "ALONGAMENTO": v = v.OrderByDescending(s => s.ALONGAMENTO); break;
                        case "SEQUENCIA": v = v.OrderByDescending(s => s.SEQUENCIA); break;
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
                    TREINO = x.tr_nome,
                    NOME = x.nome,
                    ALONGAMENTO=x.ALONGAMENTO,
                    SEQUENCIA=x.SEQUENCIA,
                    INSERCAO = x.INSERCAO,
                    DATAINSERCAO = x.DATA_INSERCAO,
                    ACTUALIZACAO = x.ACTUALIZACAO,
                    DATAACTUALIZACAO = x.DATA_ACTUALIZACAO
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewGRLExercicioTable(GTExercicio MODEL)
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
                // Create
                var create = databaseManager.SP_GT_ENT_EXERCICIO(null, MODEL.TipoTreinoId, MODEL.Nome,MODEL.Alongamento,MODEL.Sequencia, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLExercicioTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLExercicioTable(GTExercicio MODEL)
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
                
                // Update
                var create = databaseManager.SP_GT_ENT_EXERCICIO(MODEL.ID, MODEL.TipoTreinoId, MODEL.Nome, MODEL.Alongamento, MODEL.Sequencia, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLExercicioTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLExercicioTable(int?[] ids)
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

                // Delete
                foreach (var i in ids)
                {
                    databaseManager.SP_GT_ENT_EXERCICIO(i, null, null, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLExercicioTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }



        //PLANOS
        public ActionResult BodyMassPlans(Gestreino.Models.GT_TreinoBodyMass MODEL,int? Id,string predefined)
        {
            //if (!AcessControl.Authorized(AcessControl.GP_USERS_LIST_VIEW_SEARCH)) return View("Lockout");
            //if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }

            



            MODEL.GT_Series_List = databaseManager.GT_Series.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.SERIES.ToString() });
            MODEL.GT_Repeticoes_List = databaseManager.GT_Repeticoes.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.REPETICOES.ToString() });
            MODEL.GT_Carga_List = databaseManager.GT_Carga.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.CARGA.ToString() });
            MODEL.GT_TempoDescanso_List = databaseManager.GT_TempoDescanso.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.TEMPO_DESCANSO });
            MODEL.FaseTreinoList = databaseManager.GT_FaseTreino.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.SIGLA });
            MODEL.GTTreinoList = databaseManager.GT_Treino.Where(x=>x.DATA_REMOCAO==null && !string.IsNullOrEmpty(x.NOME) && x.GT_TipoTreino_ID == Configs.GT_EXERCISE_TYPE_BODYMASS).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.DateIni =  DateTime.Parse(DateTime.Now.ToString()).ToString("dd-MM-yyyy");

            MODEL.GTTipoTreinoId = Configs.GT_EXERCISE_TYPE_BODYMASS;
            MODEL.PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;
            ViewBag.exercises = databaseManager.GT_Exercicio.Where(x => x.DATA_REMOCAO==null && x.GT_TipoTreino_ID==Configs.GT_EXERCISE_TYPE_BODYMASS).ToList();
           
            var upload = "gtexercicios";
            List<ExerciseArq> ExerciseArqList = new List<ExerciseArq>();
            List<ExerciseArq> ExerciseArqListTreino = new List<ExerciseArq>();
            // Define tablename and fieldname for Stored Procedure
            string tablename = FileUploader.DecoderFactory(upload)[0];
            string fieldname = FileUploader.DecoderFactory(upload)[1];
            ExerciseArqList =
                              (from j1 in databaseManager.GT_Exercicio
                               join j2 in databaseManager.GT_Exercicio_ARQUIVOS on j1.ID equals j2.GT_Exercicio_ID
                               join j3 in databaseManager.GRL_ARQUIVOS on j2.ARQUIVOS_ID equals j3.ID
                               where j1.DATA_REMOCAO == null && j2.DATA_REMOCAO == null && j1.GT_TipoTreino_ID==Configs.GT_EXERCISE_TYPE_BODYMASS && j3.GRL_ARQUIVOS_TIPO_DOCS_ID == Configs.INST_MDL_ADM_VLRID_ARQUIVO_LOGOTIPO && j2.ACTIVO==true
                               select new ExerciseArq() { ExerciseId = j1.ID, Name = j1.NOME, LogoPath = string.IsNullOrEmpty(j3.CAMINHO_URL) ? "" : "/"+j3.CAMINHO_URL }).ToList();

            if (Id > 0)
            {
                if(databaseManager.GT_Treino.Where(x=>x.ID==Id).Count()==0)
                    return RedirectToAction("bodymassplans", "gtmanagement", new { Id=string.Empty});

                MODEL.ID = Id;
                var treino = databaseManager.SP_GT_ENT_TREINO(Id, null, MODEL.GTTipoTreinoId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList();
                //ViewBag.treino = treino;
                MODEL.ExerciseArqListTreino = (from j1 in databaseManager.GT_ExercicioTreino
                                           join j2 in databaseManager.GT_Exercicio on j1.GT_Exercicio_ID equals j2.ID
                                           where j1.GT_Treino_ID==Id
                                           select new ExerciseArq() { Name=j2.NOME,ExerciseId=j1.GT_Exercicio_ID,GT_Series_ID=j1.GT_Series_ID ,GT_Repeticoes_ID=j1.GT_Repeticoes_ID,GT_TempoDescanso_ID=j1.GT_TempoDescanso_ID,GT_Carga_ID=j1.GT_Carga_ID,REPETICOES_COMPLETADAS=j1.REPETICOES_COMPLETADAS,CARGA_USADA=j1.CARGA_USADA,ONERM=j1.ONERM,ORDEM=j1.ORDEM }).ToList();

                if (string.IsNullOrEmpty(predefined))
                {
                    if (!string.IsNullOrEmpty(treino.First().DATA_INICIO.ToString()))
                        MODEL.DateIni = DateTime.Parse(treino.First().DATA_INICIO.ToString()).ToString("dd-MM-yyyy");

                    if (treino.First().pes_id != MODEL.PEsId)
                        return RedirectToAction("bodymassplans", "gtmanagement", new { Id = string.Empty });
                }
                else
                {
                    Boolean n=false;
                    if(!Boolean.TryParse(predefined,out n))
                          return RedirectToAction("", "home");
                    MODEL.predefined = Convert.ToBoolean(predefined);
                    MODEL.DateIni = DateTime.Parse(DateTime.Now.ToString()).ToString("dd-MM-yyyy");
                    MODEL.GTTreinoId = Id;
                }
            }

            MODEL.ExerciseArqList = ExerciseArqList;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_PlanBodyMass;
            return View("Plans/BodyMass/NewPlan",MODEL);
        }
        public ActionResult CardioPlans(Gestreino.Models.GT_TreinoBodyMass MODEL, int? Id, string predefined)
        {
            //if (!AcessControl.Authorized(AcessControl.GP_USERS_LIST_VIEW_SEARCH)) return View("Lockout");
            //if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }

            MODEL.GT_DuracaoTreinoCardioList = databaseManager.GT_DuracaoTreinoCardio.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DURACAO.ToString() + "'" });
            MODEL.GTTreinoList = databaseManager.GT_Treino.Where(x => x.DATA_REMOCAO == null && !string.IsNullOrEmpty(x.NOME) && x.GT_TipoTreino_ID == Configs.GT_EXERCISE_TYPE_CARDIO).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.DateIni = DateTime.Parse(DateTime.Now.ToString()).ToString("dd-MM-yyyy");

            MODEL.GTTipoTreinoId = Configs.GT_EXERCISE_TYPE_CARDIO;
            MODEL.PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;
            ViewBag.exercises = databaseManager.GT_Exercicio.Where(x => x.DATA_REMOCAO == null && x.GT_TipoTreino_ID == Configs.GT_EXERCISE_TYPE_CARDIO).ToList();

            var upload = "gtexercicios";
            List<ExerciseArq> ExerciseArqList = new List<ExerciseArq>();
            List<ExerciseArq> ExerciseArqListTreino = new List<ExerciseArq>();
            // Define tablename and fieldname for Stored Procedure
            string tablename = FileUploader.DecoderFactory(upload)[0];
            string fieldname = FileUploader.DecoderFactory(upload)[1];
            ExerciseArqList =
                              (from j1 in databaseManager.GT_Exercicio
                               join j2 in databaseManager.GT_Exercicio_ARQUIVOS on j1.ID equals j2.GT_Exercicio_ID
                               join j3 in databaseManager.GRL_ARQUIVOS on j2.ARQUIVOS_ID equals j3.ID
                               where j1.DATA_REMOCAO == null && j2.DATA_REMOCAO == null && j1.GT_TipoTreino_ID == Configs.GT_EXERCISE_TYPE_CARDIO && j3.GRL_ARQUIVOS_TIPO_DOCS_ID == Configs.INST_MDL_ADM_VLRID_ARQUIVO_LOGOTIPO && j2.ACTIVO == true
                               select new ExerciseArq() { ExerciseId = j1.ID, Name = j1.NOME, LogoPath = string.IsNullOrEmpty(j3.CAMINHO_URL) ? "" : "/" + j3.CAMINHO_URL }).ToList();

            if (Id > 0)
            {
                if (databaseManager.GT_Treino.Where(x => x.ID == Id).Count() == 0)
                    return RedirectToAction("cardioplans", "gtmanagement", new { Id = string.Empty });

                MODEL.ID = Id;
                var treino = databaseManager.SP_GT_ENT_TREINO(Id, null, MODEL.GTTipoTreinoId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList();

                MODEL.ExerciseArqListTreino = (from j1 in databaseManager.GT_ExercicioTreinoCardio
                                               join j2 in databaseManager.GT_Exercicio on j1.GT_Exercicio_ID equals j2.ID
                                               where j1.GT_Treino_ID == Id
                                               select new ExerciseArq() { Name = j2.NOME, ExerciseId = j1.GT_Exercicio_ID, GT_DuracaoTreinoCardio_ID = j1.GT_DuracaoTreinoCardio_ID, FC = j1.FC, Nivel = j1.NIVEL, Distancia = j1.DISTANCIA, ORDEM = j1.ORDEM }).ToList();

                if (string.IsNullOrEmpty(predefined))
                {
                    if (!string.IsNullOrEmpty(treino.First().DATA_INICIO.ToString()))
                        MODEL.DateIni = DateTime.Parse(treino.First().DATA_INICIO.ToString()).ToString("dd-MM-yyyy");
                    if (!string.IsNullOrEmpty(treino.First().DATA_FIM.ToString()))
                        MODEL.DateEnd = DateTime.Parse(treino.First().DATA_FIM.ToString()).ToString("dd-MM-yyyy");
                    MODEL.Observacoes = treino.First().OBSERVACOES;
                    MODEL.Periodizacao = treino.First().PERIODIZACAO;

                    if (treino.First().pes_id != MODEL.PEsId)
                        return RedirectToAction("bodymassplans", "gtmanagement", new { Id = string.Empty });
                }
                else
                {
                    Boolean n = false;
                    if (!Boolean.TryParse(predefined, out n))
                        return RedirectToAction("", "home");
                    MODEL.predefined = Convert.ToBoolean(predefined);
                    MODEL.DateIni = DateTime.Parse(DateTime.Now.ToString()).ToString("dd-MM-yyyy");
                    MODEL.GTTreinoId = Id;
                }
            }
            MODEL.ExerciseArqList = ExerciseArqList;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_PlanCardio;
            return View("Plans/Cardio/NewPlan", MODEL);
        }
        [HttpGet]
        public ActionResult GetFasesTreino(int? Id)
        {
            var v = (from j1 in databaseManager.GT_FaseTreino
                     join j2 in databaseManager.GT_Series on j1.GT_Series_ID equals j2.ID
                     join j3 in databaseManager.GT_Repeticoes on j1.GT_Repeticoes_ID equals j3.ID
                     join j4 in databaseManager.GT_Carga on j1.GT_Carga_ID equals j4.ID
                     join j5 in databaseManager.GT_TempoDescanso on j1.GT_TempoDescanso_ID equals j5.ID
                     where j1.ID==Id && j1.DATA_REMOCAO==null
                     select new { j1.GT_Series_ID,j1.GT_Repeticoes_ID,j1.GT_Carga_ID,j1.GT_TempoDescanso_ID,j2.SERIES, j3.REPETICOES, j4.CARGA, j5.TEMPO_DESCANSO}).ToList();
          
            return Json(v.Select(x => new
            {
                GT_Series_ID=x.GT_Series_ID,
                GT_Repeticoes_ID=x.GT_Repeticoes_ID,
                GT_Carga_ID=x.GT_Carga_ID,
                GT_TempoDescanso_ID=x.GT_TempoDescanso_ID,
                SERIES = x.SERIES,
                REPETICOES = x.REPETICOES,
                CARGA = x.CARGA,
                TEMPO_DESCANSO = x.TEMPO_DESCANSO
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetGTTreinoTable(int? PesId,int? GTTipoTreinoId)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var DateIni = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var DateEnd = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Obs = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            PesId = PesId > 0 ? PesId : null;
            GTTipoTreinoId = GTTipoTreinoId > 0 ? GTTipoTreinoId : null;
            var v = (from a in databaseManager.SP_GT_ENT_TREINO(null, PesId, GTTipoTreinoId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(DateIni)) v = v.Where(a => a.DATA_INICIO != null && a.DATA_INICIO.ToUpper().Contains(DateIni.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse
            if (!string.IsNullOrEmpty(DateEnd)) v = v.Where(a => a.DATA_FIM != null && a.DATA_FIM.ToUpper().Contains(DateEnd.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse
            if (!string.IsNullOrEmpty(Obs)) v = v.Where(a => a.OBSERVACOES != null && a.OBSERVACOES.ToUpper().Contains(Obs.ToUpper()));
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
                        case "DATEINI": v = v.OrderBy(s => s.DATA_INICIO); break;
                        case "DATEFIM": v = v.OrderBy(s => s.DATA_FIM); break;
                        case "OBS": v = v.OrderBy(s => s.OBSERVACOES); break;
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
                        case "DATEINI": v = v.OrderByDescending(s => s.DATA_INICIO); break;
                        case "DATEFIM": v = v.OrderByDescending(s => s.DATA_FIM); break;
                        case "OBS": v = v.OrderByDescending(s => s.OBSERVACOES); break;
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
                    DATEINI = x.DATA_INICIO,
                    DATEFIM = x.DATA_FIM,
                    OBS = x.OBSERVACOES,
                    INSERCAO = x.INSERCAO,
                    DATAINSERCAO = x.DATA_INSERCAO,
                    ACTUALIZACAO = x.ACTUALIZACAO,
                    DATAACTUALIZACAO = x.DATA_ACTUALIZACAO,
                    LINK=x.GT_TipoTreino_ID==Configs.GT_EXERCISE_TYPE_BODYMASS? "/gtmanagement/bodymassplans/"+x.ID : "/gtmanagement/cardioplans/" + x.ID
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GTPlans(GT_TreinoBodyMass MODEL, int?[] exIds, int?[] exSeries, int?[] exRepeticoes, int?[] exCarga, int?[] exTempo, int?[] exReps, int?[] exCargaUsada, string[] exRM,/**/ int?[] exDuracao, int?[] exFC, int?[] exNivel, string[]exDistancia)
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

                if(exIds==null)
                    return Json(new { result = false, error = "Não tem exercício alocado no plano!" });


                var DateIni = string.IsNullOrWhiteSpace(MODEL.DateIni) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateIni, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DateEnd = string.IsNullOrWhiteSpace(MODEL.DateEnd) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                if (!string.IsNullOrEmpty(MODEL.DateEnd))
                {
                    if (!string.IsNullOrWhiteSpace(MODEL.DateIni) && DateTime.ParseExact(MODEL.DateEnd, "dd-MM-yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(MODEL.DateIni, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                      return Json(new { result = false, error = "Data de início deve ser inferior a Data de fim!" });
                }

                if (MODEL.ID > 0)
                {
                    //Update
                    var update = databaseManager.SP_GT_ENT_TREINO(MODEL.ID, null, null, MODEL.Nome, MODEL.FaseTreinoId, MODEL.Periodizacao, DateIni, DateEnd, MODEL.Observacoes, null, null, null, null, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "U").ToList();
                }
                else
                {
                    // Create
                    var create = databaseManager.SP_GT_ENT_TREINO(null, MODEL.PEsId, MODEL.GTTipoTreinoId, MODEL.Nome, MODEL.FaseTreinoId, MODEL.Periodizacao, DateIni, DateEnd, MODEL.Observacoes, null, null, null, null, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "C").ToList();
                    MODEL.ID = create.First().ID;
                }

                //Remove first
                var delete = databaseManager.SP_GT_ENT_TREINO(MODEL.ID, null, MODEL.GTTipoTreinoId, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), MODEL.GTTipoTreinoId==Configs.GT_EXERCISE_TYPE_BODYMASS?"DB":"DC").ToList();
                
                Decimal RMs = 0;
                Decimal Distancia = 0;

                if (MODEL.GTTipoTreinoId == Configs.GT_EXERCISE_TYPE_BODYMASS)
                {
                     for (int x= 0;x < exIds.Length;x++)
                {
                    if (exRM!=null && !string.IsNullOrEmpty(exRM[x]))
                                Convert.ToDecimal(exRM[x].Replace(".",","));
                    databaseManager.SP_GT_ENT_TREINO(MODEL.ID, null, MODEL.GTTipoTreinoId, null, null, null, null, null, null, exIds[x], exSeries[x], exRepeticoes[x], exTempo[x], exCarga[x], exReps[x], exCargaUsada[x], RMs, null, null, null, null, x, int.Parse(User.Identity.GetUserId()), MODEL.GTTipoTreinoId == Configs.GT_EXERCISE_TYPE_BODYMASS ? "CB" : "CC").ToList();
                }
                }
                if (MODEL.GTTipoTreinoId == Configs.GT_EXERCISE_TYPE_CARDIO)
                {
                    for (int x = 0; x < exIds.Length; x++)
                    {
                        if (exDistancia != null && !string.IsNullOrEmpty(exDistancia[x]))
                            Convert.ToDecimal(exDistancia[x].Replace(".", ","));
                        databaseManager.SP_GT_ENT_TREINO(MODEL.ID, null, MODEL.GTTipoTreinoId, null, null, null, null, null, null, exIds[x], null, null, null, null, null, null, null, exDuracao[x], exFC[x], exNivel[x], Distancia, x, int.Parse(User.Identity.GetUserId()), MODEL.GTTipoTreinoId == Configs.GT_EXERCISE_TYPE_BODYMASS ? "CB" : "CC").ToList();
                    }
                }

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, reload=true, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGTPlans(int?[] ids)
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

                // Delete
                foreach (var i in ids)
                {
                    databaseManager.SP_GT_ENT_TREINO(i, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()),"D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GTTreinoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }






        // Avaliacao psicologica
        public ActionResult Anxiety(GT_Quest_Anxient MODEL,int? Id)
        {
            if (Id > 0)
            {
                var data = databaseManager.GT_RespAnsiedadeDepressao.Where(x => x.ID == Id).ToList();
                if (data.Count() == 0)
                    return RedirectToAction("anxient", "gtmanagement", new { Id = string.Empty });
                ViewBag.data = data;
                MODEL.ID = Id;
                MODEL.q1 = data.First().RESP_01;
                MODEL.q2 = data.First().RESP_02;
                MODEL.q3 = data.First().RESP_03;
                MODEL.q4 = data.First().RESP_04;
                MODEL.q5 = data.First().RESP_05;
                MODEL.q6 = data.First().RESP_06;
                MODEL.q7 = data.First().RESP_07;
                MODEL.q8 = data.First().RESP_08;
                MODEL.q9 = data.First().RESP_09;
                MODEL.q10 = data.First().RESP_10;
                MODEL.q11 = data.First().RESP_11;
                MODEL.q12 = data.First().RESP_12;
                MODEL.q13 = data.First().RESP_13;
                MODEL.q14 = data.First().RESP_14;
            }

            MODEL.PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Quest_Anxient;
            return View("Quest/Anxiety", MODEL);
        }
        public ActionResult GetGTQuestTable(int? PesId, string GT_Res,int? TipoId)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Insercao = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            PesId = PesId > 0 ? PesId : null;
            var Link = string.Empty;
            if (GT_Res == "GT_RespAnsiedadeDepressao") Link = "/gtmanagement/anxiety/";
            if (GT_Res == "GT_RespAutoConceito") Link = "/gtmanagement/selfconcept/";
            if (GT_Res == "GT_RespRisco") Link = "/gtmanagement/coronaryrisk/";
            if (GT_Res == "GT_RespProblemasSaude") Link = "/gtmanagement/health/";
            if (GT_Res == "GT_RespFlexiTeste") Link = "/gtmanagement/flexibility/";
            if (GT_Res == "GT_RespComposicao") Link = "/gtmanagement/bodycomposition/";

            TipoId = TipoId > 0 ? TipoId : null;
            var v = (from a in databaseManager.SP_GT_ENT_Resp(TipoId, PesId, GT_Res,  null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
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
                    INSERCAO = x.INSERCAO,
                    DATAINSERCAO = x.DATA_INSERCAO,
                    ACTUALIZACAO = x.ACTUALIZACAO,
                    DATAACTUALIZACAO = x.DATA_ACTUALIZACAO,
                    UPLOAD= GT_Res,
                    LINK = Link + x.ID
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Anxiety(GT_Quest_Anxient MODEL,int? frmaction,string returnUrl)
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

                //You could possibly use Reflection to do this.
                PropertyInfo[] properties = typeof(GT_Quest_Anxient).GetProperties();
                List<string> f = new List<string> { };

                foreach (PropertyInfo property in properties)
                {
                    var val = property.GetValue(MODEL);
                    if (val != null && property.Name.Contains("q"))
                        f.Add(val.ToString());
                }

                int totalPropertiesInClass = 14;
               
                if (frmaction == 1)
                {
                    if (f.Count() < totalPropertiesInClass)
                        return Json(new { result = false, error = "Existem perguntas por responder!" });

                    if (MODEL.ID > 0)
                    {
                        (from c in databaseManager.GT_RespAnsiedadeDepressao
                         where c.ID == MODEL.ID
                         select c).ToList().ForEach(fx => {
                             fx.RESP_01 = MODEL.q1;
                             fx.RESP_02 = MODEL.q2;
                             fx.RESP_03 = MODEL.q3;
                             fx.RESP_04 = MODEL.q4;
                             fx.RESP_05 = MODEL.q5;
                             fx.RESP_06 = MODEL.q6;
                             fx.RESP_07 = MODEL.q7;
                             fx.RESP_08 = MODEL.q8;
                             fx.RESP_09 = MODEL.q9;
                             fx.RESP_10 = MODEL.q10;
                             fx.RESP_11 = MODEL.q11;
                             fx.RESP_12 = MODEL.q12;
                             fx.RESP_13 = MODEL.q13;
                             fx.RESP_14 = MODEL.q14;
                             fx.RESP_SUMMARY = int.Parse(GetResult(MODEL));
                             fx.RESP_DESCRICAO = GetResultQuest(MODEL);
                             fx.ACTUALIZADO_POR = int.Parse(User.Identity.GetUserId()); fx.DATA_ACTUALIZACAO = DateTime.Now; });
                             databaseManager.SaveChanges();
                    }
                    else
                    {
                        GT_RespAnsiedadeDepressao fx = new GT_RespAnsiedadeDepressao();
                        fx.GT_SOCIOS_ID = databaseManager.GT_SOCIOS.Where(x => x.PES_PESSOAS_ID == MODEL.PEsId).Select(x => x.ID).FirstOrDefault();
                        fx.RESP_01 = MODEL.q1;
                        fx.RESP_02 = MODEL.q2;
                        fx.RESP_03 = MODEL.q3;
                        fx.RESP_04 = MODEL.q4;
                        fx.RESP_05 = MODEL.q5;
                        fx.RESP_06 = MODEL.q6;
                        fx.RESP_07 = MODEL.q7;
                        fx.RESP_08 = MODEL.q8;
                        fx.RESP_09 = MODEL.q9;
                        fx.RESP_10 = MODEL.q10;
                        fx.RESP_11 = MODEL.q11;
                        fx.RESP_12 = MODEL.q12;
                        fx.RESP_13 = MODEL.q13;
                        fx.RESP_14 = MODEL.q14;
                        fx.RESP_SUMMARY = int.Parse(GetResult(MODEL));
                        fx.RESP_DESCRICAO = GetResultQuest(MODEL);
                        fx.INSERIDO_POR = int.Parse(User.Identity.GetUserId());
                        fx.DATA_INSERCAO = DateTime.Now;
                        databaseManager.GT_RespAnsiedadeDepressao.Add(fx);
                        databaseManager.SaveChanges();
                    }
                }
                else {
                    if (f.Count() < totalPropertiesInClass)
                        return Json(new { result = false, error = "Existem perguntas por responder!" });
                    return Json(new { result = true, success = GetResultQuest(MODEL) });
                }
           
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GTQuestTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGTQuest(int?[] ids,string upload)
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

                // Delete
                foreach (var i in ids)
                {
                    databaseManager.SP_GT_ENT_Resp(i, null, upload, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GTQuestTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        public ActionResult SelfConcept(GT_Quest_SelfConcept MODEL, int? Id)
        {

            if (Id > 0)
            {
                var data = databaseManager.GT_RespAutoConceito.Where(x => x.ID == Id).ToList();
                if (data.Count() == 0)
                    return RedirectToAction("selfconcept", "gtmanagement", new { Id = string.Empty });
                ViewBag.data = data;
                MODEL.ID = Id;
                MODEL.q1 = data.First().RESP_01;
                MODEL.q2 = data.First().RESP_02;
                MODEL.q3 = data.First().RESP_03;
                MODEL.q4 = data.First().RESP_04;
                MODEL.q5 = data.First().RESP_05;
                MODEL.q6 = data.First().RESP_06;
                MODEL.q7 = data.First().RESP_07;
                MODEL.q8 = data.First().RESP_08;
                MODEL.q9 = data.First().RESP_09;
                MODEL.q10 = data.First().RESP_10;
                MODEL.q11 = data.First().RESP_11;
                MODEL.q12 = data.First().RESP_12;
                MODEL.q13 = data.First().RESP_13;
                MODEL.q14 = data.First().RESP_14;
                MODEL.q15 = data.First().RESP_15;
                MODEL.q16 = data.First().RESP_16;
                MODEL.q17 = data.First().RESP_17;
                MODEL.q18 = data.First().RESP_18;
                MODEL.q19 = data.First().RESP_19;
                MODEL.q20 = data.First().RESP_20;
            }

            MODEL.PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Quest_SelfConcept;
            return View("Quest/SelfConcept", MODEL);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelfConcept(GT_Quest_SelfConcept MODEL, int? frmaction, string returnUrl)
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

                //You could possibly use Reflection to do this.
                PropertyInfo[] properties = typeof(GT_Quest_SelfConcept).GetProperties();
                List<string> f = new List<string> { };
                int iValue;

                foreach (PropertyInfo property in properties)
                {
                    var val = property.GetValue(MODEL);
                    if (val != null && property.Name.Contains("q"))
                        f.Add(val.ToString());
                }

                int totalPropertiesInClass = 20;

                if (frmaction == 1)
                {
                    if (f.Count() < totalPropertiesInClass)
                        return Json(new { result = false, error = "Existem perguntas por responder!" });

                    if (MODEL.ID > 0)
                    {
                        (from c in databaseManager.GT_RespAutoConceito
                         where c.ID == MODEL.ID
                         select c).ToList().ForEach(fx => {
                             fx.RESP_01 = MODEL.q1;
                             fx.RESP_02 = MODEL.q2;
                             fx.RESP_03 = MODEL.q3;
                             fx.RESP_04 = MODEL.q4;
                             fx.RESP_05 = MODEL.q5;
                             fx.RESP_06 = MODEL.q6;
                             fx.RESP_07 = MODEL.q7;
                             fx.RESP_08 = MODEL.q8;
                             fx.RESP_09 = MODEL.q9;
                             fx.RESP_10 = MODEL.q10;
                             fx.RESP_11 = MODEL.q11;
                             fx.RESP_12 = MODEL.q12;
                             fx.RESP_13 = MODEL.q13;
                             fx.RESP_14 = MODEL.q14;
                             fx.RESP_15 = MODEL.q15;
                             fx.RESP_16 = MODEL.q16;
                             fx.RESP_17 = MODEL.q17;
                             fx.RESP_18 = MODEL.q18;
                             fx.RESP_19 = MODEL.q19;
                             fx.RESP_20 = MODEL.q20;
                             fx.RESP_DESCRICAO = GetResultQuestSelfConcept(MODEL,out iValue);
                             fx.RESP_SUMMARY = iValue;
                             fx.ACTUALIZADO_POR = int.Parse(User.Identity.GetUserId()); fx.DATA_ACTUALIZACAO = DateTime.Now;
                         });
                        databaseManager.SaveChanges();
                    }
                    else
                    {
                        GT_RespAutoConceito fx = new GT_RespAutoConceito();
                        fx.GT_SOCIOS_ID = databaseManager.GT_SOCIOS.Where(x => x.PES_PESSOAS_ID == MODEL.PEsId).Select(x => x.ID).FirstOrDefault();
                        fx.RESP_01 = MODEL.q1;
                        fx.RESP_02 = MODEL.q2;
                        fx.RESP_03 = MODEL.q3;
                        fx.RESP_04 = MODEL.q4;
                        fx.RESP_05 = MODEL.q5;
                        fx.RESP_06 = MODEL.q6;
                        fx.RESP_07 = MODEL.q7;
                        fx.RESP_08 = MODEL.q8;
                        fx.RESP_09 = MODEL.q9;
                        fx.RESP_10 = MODEL.q10;
                        fx.RESP_11 = MODEL.q11;
                        fx.RESP_12 = MODEL.q12;
                        fx.RESP_13 = MODEL.q13;
                        fx.RESP_14 = MODEL.q14;
                        fx.RESP_15 = MODEL.q15;
                        fx.RESP_16 = MODEL.q16;
                        fx.RESP_17 = MODEL.q17;
                        fx.RESP_18 = MODEL.q18;
                        fx.RESP_19 = MODEL.q19;
                        fx.RESP_20 = MODEL.q20;
                        fx.RESP_DESCRICAO = GetResultQuestSelfConcept(MODEL, out iValue);
                        fx.RESP_SUMMARY = iValue;
                        fx.INSERIDO_POR = int.Parse(User.Identity.GetUserId());
                        fx.DATA_INSERCAO = DateTime.Now;
                        databaseManager.GT_RespAutoConceito.Add(fx);
                        databaseManager.SaveChanges();
                    }
                }
                else
                {
                    if (f.Count() < totalPropertiesInClass)
                        return Json(new { result = false, error = "Existem perguntas por responder!" });

                    return Json(new { result = true, success = GetResultQuestSelfConcept(MODEL, out iValue) });
                }

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GTQuestTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Risco coronario
        public ActionResult CoronaryRisk(CoronaryRisk MODEL, int? Id)
        {

            MODEL.PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;
            MODEL.txtIMC = databaseManager.PES_PESSOAS_CARACT.Where(x => x.PES_PESSOAS_ID == MODEL.PEsId).Select(X => X.IMC).FirstOrDefault();
            string sex = databaseManager.PES_PESSOAS.Where(x => x.ID == MODEL.PEsId).Select(X => X.SEXO).FirstOrDefault();
            MODEL.IdadeQuery = sex == "M" ? "Tem idade superior a 45 anos?" : "Tem idade superior a 55 anos?";

            if (!string.IsNullOrEmpty(Configs.GESTREINO_AVALIDO_IDADE))
            {
                if(sex=="M")
                    MODEL.q1 = int.Parse(Configs.GESTREINO_AVALIDO_IDADE) > 45 ? 1 : 0;
                else
                    MODEL.q1 = int.Parse(Configs.GESTREINO_AVALIDO_IDADE) > 55 ? 1 : 0;
            }

            if (Id > 0)
            {
                var data = databaseManager.GT_RespRisco.Where(x => x.ID == Id).ToList();
                if (data.Count() == 0)
                    return RedirectToAction("coronaryrisk", "gtmanagement", new { Id = string.Empty });
                ViewBag.data = data;
                MODEL.ID = Id;
                MODEL.q2 = data.First().radHeredMasc.HasValue? Convert.ToInt32(data.First().radHeredMasc): (int?)null;
                MODEL.q16 = data.First().radHeredFem.HasValue ? Convert.ToInt32(data.First().radHeredFem) : (int?)null;
                MODEL.q3 = data.First().radTabacFuma.HasValue ? Convert.ToInt32(data.First().radTabacFuma) : (int?)null;
                MODEL.q4 = data.First().radTabacFuma6.HasValue ? Convert.ToInt32(data.First().radTabacFuma6) : (int?)null;
                MODEL.txtCigarrosMedia = data.First().txtCigarrosMedia;
                MODEL.q5 = data.First().radTensao.HasValue ? Convert.ToInt32(data.First().radTensao) : (int?)null;
                MODEL.txtMaxSistolica = data.First().txtMaxSistolica ;
                MODEL.txtMinSistolica = data.First().txtMinSistolica;
                MODEL.txtMaxDistolica = data.First().txtMaxDistolica;
                MODEL.txtMinDistolica = data.First().txtMinDistolica;
                MODEL.q6 = data.First().radMedicacao.HasValue ? Convert.ToInt32(data.First().radMedicacao) : (int?)null;
                MODEL.txtMedicamento = data.First().txtMedicamento;
                MODEL.q7 = data.First().radColesterol1.HasValue ? Convert.ToInt32(data.First().radColesterol1) : (int?)null;
                MODEL.q8 = data.First().radColesterol2.HasValue ? Convert.ToInt32(data.First().radColesterol2) : (int?)null;
                MODEL.q9 = data.First().radColesterol3.HasValue ? Convert.ToInt32(data.First().radColesterol3) : (int?)null;
                MODEL.q10 = data.First().radColesterol4.HasValue ? Convert.ToInt32(data.First().radColesterol4) : (int?)null;
                MODEL.q11 = data.First().radColesterol5.HasValue ? Convert.ToInt32(data.First().radColesterol5) : (int?)null;
                MODEL.q12 = data.First().radGlicose.HasValue ? Convert.ToInt32(data.First().radGlicose) : (int?)null;
                MODEL.txtGlicose1 = data.First().txtGlicose1;
                MODEL.txtGlicose2 = data.First().txtGlicose2;
                MODEL.q13 = data.First().radInactividade1.HasValue ? Convert.ToInt32(data.First().radInactividade1) : (int?)null;
                MODEL.q14 = data.First().radInactividade2.HasValue ? Convert.ToInt32(data.First().radInactividade2) : (int?)null;
                MODEL.q15 = data.First().radInactividade3.HasValue ? Convert.ToInt32(data.First().radInactividade3) : (int?)null;
                MODEL.txtPerimetro = data.First().txtPerimetro;
                MODEL.txtCardiaca = data.First().txtCardiaca;
                MODEL.txtVascular = data.First().txtVascular;
                MODEL.txtCerebroVascular = data.First().txtCerebroVascular;
                MODEL.txtCardioVascularOutras = data.First().txtCardioVascularOutras;
                MODEL.txtObstrucao = data.First().txtObstrucao;
                MODEL.txtAsma = data.First().txtAsma;
                MODEL.txtFibrose = data.First().txtFibrose;
                MODEL.txtPulmomarOutras = data.First().txtPulmomarOutras;
                MODEL.txtDiabetes1 = data.First().txtDiabetes1;
                MODEL.txtDiabetes2 = data.First().txtDiabetes2;
                MODEL.txtTiroide = data.First().txtTiroide;
                MODEL.txtRenais = data.First().txtRenais;
                MODEL.txtFigado = data.First().txtFigado;
                MODEL.txtMetabolicaOutras = data.First().txtMetabolicaOutras;
                MODEL.chkCardiaca = !string.IsNullOrEmpty(data.First().txtCardiaca) ? true : false;
                MODEL.chkVascular = !string.IsNullOrEmpty(data.First().txtVascular) ? true : false;
                MODEL.chkCerebroVascular = !string.IsNullOrEmpty(data.First().txtCerebroVascular) ? true : false;
                MODEL.chkCardioVascularOutras = !string.IsNullOrEmpty(data.First().txtCardioVascularOutras) ? true : false;
                MODEL.chkObstrucao = !string.IsNullOrEmpty(data.First().txtObstrucao) ? true : false;
                MODEL.chkAsma = !string.IsNullOrEmpty(data.First().txtAsma) ? true : false;
                MODEL.chkFibrose = !string.IsNullOrEmpty(data.First().txtFibrose) ? true : false;
                MODEL.chkPulmomarOutras = !string.IsNullOrEmpty(data.First().txtPulmomarOutras) ? true : false;
                MODEL.chkDiabetes1 = !string.IsNullOrEmpty(data.First().txtDiabetes1) ? true : false;
                MODEL.chkDiabetes2 = !string.IsNullOrEmpty(data.First().txtDiabetes2) ? true : false;
                MODEL.chkTiroide = !string.IsNullOrEmpty(data.First().txtTiroide) ? true : false;
                MODEL.chkRenais = !string.IsNullOrEmpty(data.First().txtRenais) ? true : false;
                MODEL.chkFigado = !string.IsNullOrEmpty(data.First().txtFigado) ? true : false;
                MODEL.chkMetabolicaOutras = !string.IsNullOrEmpty(data.First().txtMetabolicaOutras) ? true : false;
                MODEL.chkDor = data.First().chkDor.Value;
                MODEL.chkRespiracao = data.First().chkRespiracao.Value;
                MODEL.chkTonturas = data.First().chkTonturas.Value;
                MODEL.chkDispeneia = data.First().chkDispeneia.Value;
                MODEL.chkEdema = data.First().chkEdema.Value;
                MODEL.chkPalpitacoes = data.First().chkPalpitacoes.Value;
                MODEL.chkClaudicacao = data.First().chkClaudicacao.Value;
                MODEL.chkMurmurio = data.First().chkMurmurio.Value;
                MODEL.chkfadiga = data.First().chkfadiga.Value;
            }

            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Quest_CoronaryRisk;
            return View("Quest/CoronaryRisk", MODEL);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CoronaryRisk(CoronaryRisk MODEL, int? frmaction, string returnUrl)
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

                string sValue = string.Empty;
                GetResultQuestCoronaryRisk(MODEL, out sValue);

                if (frmaction == 1)
                {
                    if (MODEL.ID > 0)
                    {
                        (from c in databaseManager.GT_RespRisco
                         where c.ID == MODEL.ID
                         select c).ToList().ForEach(fx => {
                             fx.radIdade = MODEL.q1!=null? Convert.ToBoolean(MODEL.q1): (Boolean?)null;
                             fx.radHeredMasc = MODEL.q2 != null ? Convert.ToBoolean(MODEL.q2) : (Boolean?)null;
                             fx.radHeredFem = MODEL.q16 != null ? Convert.ToBoolean(MODEL.q16) : (Boolean?)null;
                             fx.radTabacFuma = MODEL.q3 != null ? Convert.ToBoolean(MODEL.q3) : (Boolean?)null;
                             fx.radTabacFuma6 = MODEL.q4 != null ? Convert.ToBoolean(MODEL.q4) : (Boolean?)null;
                             fx.txtCigarrosMedia = MODEL.txtCigarrosMedia;
                             fx.radTensao = MODEL.q5 != null ? Convert.ToBoolean(MODEL.q5) : (Boolean?)null;
                             fx.txtMaxSistolica = MODEL.txtMaxSistolica;
                             fx.txtMinSistolica = MODEL.txtMinSistolica;
                             fx.txtMaxDistolica = MODEL.txtMaxDistolica;
                             fx.txtMinDistolica = MODEL.txtMinDistolica;
                             fx.radMedicacao = MODEL.q6 != null ? Convert.ToBoolean(MODEL.q6) : (Boolean?)null;
                             fx.txtMedicamento = MODEL.txtMedicamento;
                             fx.radColesterol1 = MODEL.q7 != null ? Convert.ToBoolean(MODEL.q7) : (Boolean?)null;
                             fx.radColesterol2 = MODEL.q8 != null ? Convert.ToBoolean(MODEL.q8) : (Boolean?)null;
                             fx.radColesterol3 = MODEL.q9 != null ? Convert.ToBoolean(MODEL.q9) : (Boolean?)null;
                             fx.radColesterol4 = MODEL.q10 != null ? Convert.ToBoolean(MODEL.q10) : (Boolean?)null;
                             fx.radColesterol5 = MODEL.q11 != null ? Convert.ToBoolean(MODEL.q11) : (Boolean?)null;
                             fx.radGlicose = MODEL.q12 != null ? Convert.ToBoolean(MODEL.q12) : (Boolean?)null;
                             fx.txtGlicose1 = MODEL.txtGlicose1;
                             fx.txtGlicose2 = MODEL.txtGlicose2;
                             fx.radInactividade1 = MODEL.q13 != null ? Convert.ToBoolean(MODEL.q13) : (Boolean?)null;
                             fx.radInactividade2 = MODEL.q14 != null ? Convert.ToBoolean(MODEL.q14) : (Boolean?)null;
                             fx.radInactividade3 = MODEL.q15 != null ? Convert.ToBoolean(MODEL.q15) : (Boolean?)null;
                             fx.txtPerimetro = MODEL.txtPerimetro;
                             fx.txtCardiaca = MODEL.txtCardiaca;
                             fx.txtVascular = MODEL.txtVascular;
                             fx.txtCerebroVascular = MODEL.txtCerebroVascular;
                             fx.txtCardioVascularOutras = MODEL.txtCardioVascularOutras;
                             fx.txtObstrucao = MODEL.txtObstrucao;
                             fx.txtAsma = MODEL.txtAsma;
                             fx.txtFibrose = MODEL.txtFibrose;
                             fx.txtPulmomarOutras = MODEL.txtPulmomarOutras;
                             fx.txtDiabetes1 = MODEL.txtDiabetes1;
                             fx.txtDiabetes2 = MODEL.txtDiabetes2;
                             fx.txtTiroide = MODEL.txtTiroide;
                             fx.txtRenais = MODEL.txtRenais;
                             fx.txtFigado = MODEL.txtFigado;
                             fx.txtMetabolicaOutras = MODEL.txtMetabolicaOutras;
                             fx.chkDor = MODEL.chkDor;
                             fx.chkRespiracao = MODEL.chkRespiracao;
                             fx.chkTonturas = MODEL.chkTonturas;
                             fx.chkDispeneia = MODEL.chkDispeneia;
                             fx.chkEdema = MODEL.chkEdema;
                             fx.chkPalpitacoes = MODEL.chkPalpitacoes;
                             fx.chkClaudicacao = MODEL.chkClaudicacao;
                             fx.chkMurmurio = MODEL.chkMurmurio;
                             fx.chkfadiga = MODEL.chkfadiga;
                             fx.RESP_DESCRICAO = GetResultQuestCoronaryRisk(MODEL, out sValue);
                             fx.RESP_SUMMARY = Convert.ToInt32(sValue);
                             fx.ACTUALIZADO_POR = int.Parse(User.Identity.GetUserId()); fx.DATA_ACTUALIZACAO = DateTime.Now;
                         });
                        databaseManager.SaveChanges();
                    }
                    else
                    {
                        GT_RespRisco fx = new GT_RespRisco();
                        fx.GT_SOCIOS_ID = databaseManager.GT_SOCIOS.Where(x => x.PES_PESSOAS_ID == MODEL.PEsId).Select(x => x.ID).FirstOrDefault();
                        fx.radIdade = MODEL.q1 != null ? Convert.ToBoolean(MODEL.q1) : (Boolean?)null;
                        fx.radHeredMasc = MODEL.q2 != null ? Convert.ToBoolean(MODEL.q2) : (Boolean?)null;
                        fx.radHeredFem = MODEL.q16 != null ? Convert.ToBoolean(MODEL.q16) : (Boolean?)null;
                        fx.radTabacFuma = MODEL.q3 != null ? Convert.ToBoolean(MODEL.q3) : (Boolean?)null;
                        fx.radTabacFuma6 = MODEL.q4 != null ? Convert.ToBoolean(MODEL.q4) : (Boolean?)null;
                        fx.txtCigarrosMedia = MODEL.txtCigarrosMedia;
                        fx.radTensao = MODEL.q5 != null ? Convert.ToBoolean(MODEL.q5) : (Boolean?)null;
                        fx.txtMaxSistolica = MODEL.txtMaxSistolica;
                        fx.txtMinSistolica = MODEL.txtMinSistolica;
                        fx.txtMaxDistolica = MODEL.txtMaxDistolica;
                        fx.txtMinDistolica = MODEL.txtMinDistolica;
                        fx.radMedicacao = MODEL.q6 != null ? Convert.ToBoolean(MODEL.q6) : (Boolean?)null;
                        fx.txtMedicamento = MODEL.txtMedicamento;
                        fx.radColesterol1 = MODEL.q7 != null ? Convert.ToBoolean(MODEL.q7) : (Boolean?)null;
                        fx.radColesterol2 = MODEL.q8 != null ? Convert.ToBoolean(MODEL.q8) : (Boolean?)null;
                        fx.radColesterol3 = MODEL.q9 != null ? Convert.ToBoolean(MODEL.q9) : (Boolean?)null;
                        fx.radColesterol4 = MODEL.q10 != null ? Convert.ToBoolean(MODEL.q10) : (Boolean?)null;
                        fx.radColesterol5 = MODEL.q11 != null ? Convert.ToBoolean(MODEL.q11) : (Boolean?)null;
                        fx.radGlicose = MODEL.q12 != null ? Convert.ToBoolean(MODEL.q12) : (Boolean?)null;
                        fx.txtGlicose1 = MODEL.txtGlicose1;
                        fx.txtGlicose2 = MODEL.txtGlicose2;
                        fx.radInactividade1 = MODEL.q13 != null ? Convert.ToBoolean(MODEL.q13) : (Boolean?)null;
                        fx.radInactividade2 = MODEL.q14 != null ? Convert.ToBoolean(MODEL.q14) : (Boolean?)null;
                        fx.radInactividade3 = MODEL.q15 != null ? Convert.ToBoolean(MODEL.q15) : (Boolean?)null;
                        fx.txtPerimetro = MODEL.txtPerimetro;
                        fx.txtCardiaca = MODEL.txtCardiaca;
                        fx.txtVascular = MODEL.txtVascular;
                        fx.txtCerebroVascular = MODEL.txtCerebroVascular;
                        fx.txtCardioVascularOutras = MODEL.txtCardioVascularOutras;
                        fx.txtObstrucao = MODEL.txtObstrucao;
                        fx.txtAsma = MODEL.txtAsma;
                        fx.txtFibrose = MODEL.txtFibrose;
                        fx.txtPulmomarOutras = MODEL.txtPulmomarOutras;
                        fx.txtDiabetes1 = MODEL.txtDiabetes1;
                        fx.txtDiabetes2 = MODEL.txtDiabetes2;
                        fx.txtTiroide = MODEL.txtTiroide;
                        fx.txtRenais = MODEL.txtRenais;
                        fx.txtFigado = MODEL.txtFigado;
                        fx.txtMetabolicaOutras = MODEL.txtMetabolicaOutras;
                        fx.chkDor = MODEL.chkDor;
                        fx.chkRespiracao = MODEL.chkRespiracao;
                        fx.chkTonturas = MODEL.chkTonturas;
                        fx.chkDispeneia = MODEL.chkDispeneia;
                        fx.chkEdema = MODEL.chkEdema;
                        fx.chkPalpitacoes = MODEL.chkPalpitacoes;
                        fx.chkClaudicacao = MODEL.chkClaudicacao;
                        fx.chkMurmurio = MODEL.chkMurmurio;
                        fx.chkfadiga = MODEL.chkfadiga;
                        fx.RESP_DESCRICAO = GetResultQuestCoronaryRisk(MODEL, out sValue);
                        fx.RESP_SUMMARY = Convert.ToInt32(sValue);
                        fx.INSERIDO_POR = int.Parse(User.Identity.GetUserId());
                        fx.DATA_INSERCAO = DateTime.Now;
                        databaseManager.GT_RespRisco.Add(fx);
                        databaseManager.SaveChanges();
                    }
                }
                else
                {
                    return Json(new { result = true, success = sValue, risk=true });
                }

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GTQuestTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Outros problemas de saude
        public ActionResult Health(Health MODEL, int? Id)
        {
            MODEL.PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;
           
            if (Id > 0)
            {
                var data = databaseManager.GT_RespProblemasSaude.Where(x => x.ID == Id).ToList();
                if (data.Count() == 0)
                    return RedirectToAction("health", "gtmanagement", new { Id = string.Empty });
                ViewBag.data = data;
                MODEL.ID = Id;
                MODEL.q1 = data.First().radOsteoporose.HasValue ? Convert.ToInt32(data.First().radOsteoporose) : (int?)null;
                MODEL.dtOsteoporoseI = string.IsNullOrEmpty(data.First().dtOsteoporoseI.ToString()) ? null : DateTime.Parse(data.First().dtOsteoporoseI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtOsteoporoseF = string.IsNullOrEmpty(data.First().dtOsteoporoseF.ToString()) ? null : DateTime.Parse(data.First().dtOsteoporoseF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtOsteoporose = data.First().txtOsteoporose;

                MODEL.q2 = data.First().radOsteoartose.HasValue ? Convert.ToInt32(data.First().radOsteoartose) : (int?)null;
                MODEL.dtOsteoartoseI = string.IsNullOrEmpty(data.First().dtOsteoartoseI.ToString()) ? null : DateTime.Parse(data.First().dtOsteoartoseI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtOsteoartoseF = string.IsNullOrEmpty(data.First().dtOsteoartoseF.ToString()) ? null : DateTime.Parse(data.First().dtOsteoartoseF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtOsteoartose = data.First().txtOsteoartose;

                MODEL.q3 = data.First().radArticulares.HasValue ? Convert.ToInt32(data.First().radArticulares) : (int?)null;
                MODEL.dtArticularesI = string.IsNullOrEmpty(data.First().dtArticularesI.ToString()) ? null : DateTime.Parse(data.First().dtArticularesI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtArticularesF = string.IsNullOrEmpty(data.First().dtArticularesF.ToString()) ? null : DateTime.Parse(data.First().dtArticularesF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtArticulares = data.First().txtArticulares;

                MODEL.q4 = data.First().radLesoes.HasValue ? Convert.ToInt32(data.First().radLesoes) : (int?)null;
                MODEL.dtLesoesI = string.IsNullOrEmpty(data.First().dtLesoesI.ToString()) ? null : DateTime.Parse(data.First().dtLesoesI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtLesoesF = string.IsNullOrEmpty(data.First().dtLesoesF.ToString()) ? null : DateTime.Parse(data.First().dtLesoesF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtLesoes = data.First().txtLesoes;

                MODEL.q5 = data.First().radDor.HasValue ? Convert.ToInt32(data.First().radDor) : (int?)null;
                MODEL.dtDorI = string.IsNullOrEmpty(data.First().dtDorI.ToString()) ? null : DateTime.Parse(data.First().dtDorI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtDorF = string.IsNullOrEmpty(data.First().dtDorF.ToString()) ? null : DateTime.Parse(data.First().dtDorF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtDor = data.First().txtDor;
                MODEL.txtCausaDor = data.First().txtCausaDor;

                MODEL.q5_1 = data.First().radEscoliose.HasValue ? Convert.ToInt32(data.First().radEscoliose) : (int?)null;
                MODEL.dtEscolioseI = string.IsNullOrEmpty(data.First().dtEscolioseI.ToString()) ? null : DateTime.Parse(data.First().dtEscolioseI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtEscolioseF = string.IsNullOrEmpty(data.First().dtEscolioseF.ToString()) ? null : DateTime.Parse(data.First().dtEscolioseF.ToString()).ToString("dd-MM-yyyy");

                MODEL.q5_2 = data.First().radHiperlordose.HasValue ? Convert.ToInt32(data.First().radHiperlordose) : (int?)null;
                MODEL.dtHiperlordoseI = string.IsNullOrEmpty(data.First().dtHiperlordoseI.ToString()) ? null : DateTime.Parse(data.First().dtHiperlordoseI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtHiperlordoseF = string.IsNullOrEmpty(data.First().dtHiperlordoseF.ToString()) ? null : DateTime.Parse(data.First().dtHiperlordoseF.ToString()).ToString("dd-MM-yyyy");

                MODEL.q5_3 = data.First().radHipercifose.HasValue ? Convert.ToInt32(data.First().radHipercifose) : (int?)null;
                MODEL.dtHipercifoseI = string.IsNullOrEmpty(data.First().dtHipercifoseI.ToString()) ? null : DateTime.Parse(data.First().dtHipercifoseI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtHipercifoseF = string.IsNullOrEmpty(data.First().dtHipercifoseF.ToString()) ? null : DateTime.Parse(data.First().dtHipercifoseF.ToString()).ToString("dd-MM-yyyy");

                MODEL.q6 = data.First().radJoelho.HasValue ? Convert.ToInt32(data.First().radJoelho) : (int?)null;
                MODEL.dtJoelhoI = string.IsNullOrEmpty(data.First().dtJoelhoI.ToString()) ? null : DateTime.Parse(data.First().dtJoelhoI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtJoelhoF = string.IsNullOrEmpty(data.First().dtJoelhoF.ToString()) ? null : DateTime.Parse(data.First().dtJoelhoF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtJoelho = data.First().txtOmbro;

                MODEL.q7 = data.First().radOmbro.HasValue ? Convert.ToInt32(data.First().radOmbro) : (int?)null;
                MODEL.dtOmbroI = string.IsNullOrEmpty(data.First().dtOmbroI.ToString()) ? null : DateTime.Parse(data.First().dtOmbroI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtOmbroF = string.IsNullOrEmpty(data.First().dtOmbroF.ToString()) ? null : DateTime.Parse(data.First().dtOmbroF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtOmbro = data.First().txtOmbro;

                MODEL.q8 = data.First().radOmbro.HasValue ? Convert.ToInt32(data.First().radOmbro) : (int?)null;
                MODEL.dtPunhoI = string.IsNullOrEmpty(data.First().dtPunhoI.ToString()) ? null : DateTime.Parse(data.First().dtPunhoI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtPunhoF = string.IsNullOrEmpty(data.First().dtPunhoF.ToString()) ? null : DateTime.Parse(data.First().dtPunhoF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtPunho = data.First().txtPunho;

                MODEL.q9 = data.First().radOmbro.HasValue ? Convert.ToInt32(data.First().radOmbro) : (int?)null;
                MODEL.dtTornozeloI = string.IsNullOrEmpty(data.First().dtTornozeloI.ToString()) ? null : DateTime.Parse(data.First().dtTornozeloI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtTornozeloF = string.IsNullOrEmpty(data.First().dtTornozeloF.ToString()) ? null : DateTime.Parse(data.First().dtTornozeloF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtTornozelo = data.First().txtTornozelo;

                MODEL.q10 = data.First().radOutraArtic.HasValue ? Convert.ToInt32(data.First().radOutraArtic) : (int?)null;
                MODEL.dtOutraArticI = string.IsNullOrEmpty(data.First().dtOutraArticI.ToString()) ? null : DateTime.Parse(data.First().dtOutraArticI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtOutraArticF = string.IsNullOrEmpty(data.First().dtOutraArticF.ToString()) ? null : DateTime.Parse(data.First().dtOutraArticF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtOutraArtic1 = data.First().txtOutraArtic1;
                MODEL.txtOutraArtic2 = data.First().txtOutraArtic2;

                MODEL.q11 = data.First().radParkinson.HasValue ? Convert.ToInt32(data.First().radParkinson) : (int?)null;
                MODEL.dtParkinsonI = string.IsNullOrEmpty(data.First().dtParkinsonI.ToString()) ? null : DateTime.Parse(data.First().dtParkinsonI.ToString()).ToString("dd-MM-yyyy");

                MODEL.q12 = data.First().radVisual.HasValue ? Convert.ToInt32(data.First().radVisual) : (int?)null;
                MODEL.dtVisualI = string.IsNullOrEmpty(data.First().dtVisualI.ToString()) ? null : DateTime.Parse(data.First().dtVisualI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtVisualF = string.IsNullOrEmpty(data.First().dtVisualF.ToString()) ? null : DateTime.Parse(data.First().dtVisualF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtVisual = data.First().txtVisual;

                MODEL.q13 = data.First().radVisual.HasValue ? Convert.ToInt32(data.First().radVisual) : (int?)null;
                MODEL.dtAuditivoI = string.IsNullOrEmpty(data.First().dtAuditivoI.ToString()) ? null : DateTime.Parse(data.First().dtAuditivoI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtAuditivoF = string.IsNullOrEmpty(data.First().dtAuditivoF.ToString()) ? null : DateTime.Parse(data.First().dtAuditivoF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtAuditivo = data.First().txtAuditivo;

                MODEL.q14 = data.First().radVisual.HasValue ? Convert.ToInt32(data.First().radVisual) : (int?)null;
                MODEL.dtGastroI = string.IsNullOrEmpty(data.First().dtGastroI.ToString()) ? null : DateTime.Parse(data.First().dtGastroI.ToString()).ToString("dd-MM-yyyy");
                MODEL.dtGastroF = string.IsNullOrEmpty(data.First().dtGastroF.ToString()) ? null : DateTime.Parse(data.First().dtGastroF.ToString()).ToString("dd-MM-yyyy");
                MODEL.txtGastro = data.First().txtGastro;

                MODEL.q15 = data.First().radCirugia.HasValue ? Convert.ToInt32(data.First().radCirugia) : (int?)null;
                MODEL.txtCirugiaIdade1 = data.First().txtCirugiaIdade1;
                MODEL.txtCirugiaOnde1 = data.First().txtCirugiaOnde1;
                MODEL.txtCirugiaCausa1 = data.First().txtCirugiaCausa1;
                MODEL.txtCirugiaRestricao1 = data.First().txtCirugiaRestricao1;
                MODEL.txtCirugiaIdade2 = data.First().txtCirugiaIdade2;
                MODEL.txtCirugiaOnde2 = data.First().txtCirugiaOnde2;
                MODEL.txtCirugiaCausa2 = data.First().txtCirugiaCausa2;
                MODEL.txtCirugiaRestricao2 = data.First().txtCirugiaRestricao2;

                MODEL.q16 = data.First().radProbSaude.HasValue ? Convert.ToInt32(data.First().radProbSaude) : (int?)null;
                MODEL.txtProbSaude = data.First().txtProbSaude;

                MODEL.q17 = data.First().radInactividade.HasValue ? Convert.ToInt32(data.First().radInactividade) : (int?)null;
                MODEL.txtInactividade = data.First().txtInactividade;
            }
            
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Quest_Health;
            return View("Quest/Health", MODEL);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Health(Health MODEL, string returnUrl)
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

                    if (MODEL.ID > 0)
                    {
                        (from c in databaseManager.GT_RespProblemasSaude
                         where c.ID == MODEL.ID
                         select c).ToList().ForEach(fx => {
                             fx.radOsteoporose = MODEL.q1 != null ? Convert.ToBoolean(MODEL.q1) : (Boolean?)null;
                             fx.dtOsteoporoseI = string.IsNullOrWhiteSpace(MODEL.dtOsteoporoseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOsteoporoseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtOsteoporoseF = string.IsNullOrWhiteSpace(MODEL.dtOsteoporoseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOsteoporoseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtOsteoporose = MODEL.txtOsteoporose;

                             fx.radOsteoartose = MODEL.q2 != null ? Convert.ToBoolean(MODEL.q2) : (Boolean?)null;
                             fx.dtOsteoartoseI = string.IsNullOrWhiteSpace(MODEL.dtOsteoartoseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOsteoartoseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtOsteoartoseF = string.IsNullOrWhiteSpace(MODEL.dtOsteoartoseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOsteoartoseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtOsteoartose = MODEL.txtOsteoartose;

                             fx.radArticulares = MODEL.q3 != null ? Convert.ToBoolean(MODEL.q3) : (Boolean?)null;
                             fx.dtArticularesI = string.IsNullOrWhiteSpace(MODEL.dtArticularesI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtArticularesI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtArticularesF = string.IsNullOrWhiteSpace(MODEL.dtArticularesF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtArticularesF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtArticulares = MODEL.txtArticulares;

                             fx.radLesoes = MODEL.q4 != null ? Convert.ToBoolean(MODEL.q4) : (Boolean?)null;
                             fx.dtLesoesI = string.IsNullOrWhiteSpace(MODEL.dtLesoesI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtLesoesI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtLesoesF = string.IsNullOrWhiteSpace(MODEL.dtLesoesF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtLesoesF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtLesoes = MODEL.txtLesoes;

                             fx.radDor = MODEL.q5 != null ? Convert.ToBoolean(MODEL.q5) : (Boolean?)null;
                             fx.dtDorI = string.IsNullOrWhiteSpace(MODEL.dtDorI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtDorI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtDorF = string.IsNullOrWhiteSpace(MODEL.dtDorF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtDorF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtDor = MODEL.txtDor;
                             fx.txtCausaDor = MODEL.txtCausaDor;

                             fx.radEscoliose = MODEL.q5_1 != null ? Convert.ToBoolean(MODEL.q5_1) : (Boolean?)null;
                             fx.dtEscolioseI = string.IsNullOrWhiteSpace(MODEL.dtEscolioseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtEscolioseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtEscolioseF = string.IsNullOrWhiteSpace(MODEL.dtEscolioseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtEscolioseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                             fx.radHiperlordose = MODEL.q5_2 != null ? Convert.ToBoolean(MODEL.q5_2) : (Boolean?)null;
                             fx.dtHiperlordoseI = string.IsNullOrWhiteSpace(MODEL.dtHiperlordoseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtHiperlordoseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtHiperlordoseF = string.IsNullOrWhiteSpace(MODEL.dtHiperlordoseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtHiperlordoseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                             fx.radHipercifose = MODEL.q5_3 != null ? Convert.ToBoolean(MODEL.q5_3) : (Boolean?)null;
                             fx.dtHipercifoseI = string.IsNullOrWhiteSpace(MODEL.dtHipercifoseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtHipercifoseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtHipercifoseF = string.IsNullOrWhiteSpace(MODEL.dtHipercifoseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtHipercifoseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                             fx.radJoelho = MODEL.q6 != null ? Convert.ToBoolean(MODEL.q6) : (Boolean?)null;
                             fx.dtJoelhoI = string.IsNullOrWhiteSpace(MODEL.dtJoelhoI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtJoelhoI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtJoelhoF = string.IsNullOrWhiteSpace(MODEL.dtJoelhoF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtJoelhoF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtJoelho = MODEL.txtJoelho;

                             fx.radOmbro = MODEL.q7 != null ? Convert.ToBoolean(MODEL.q7) : (Boolean?)null;
                             fx.dtOmbroI = string.IsNullOrWhiteSpace(MODEL.dtOmbroI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOmbroI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtOmbroF = string.IsNullOrWhiteSpace(MODEL.dtOmbroF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOmbroF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtOmbro = MODEL.txtOmbro;

                             fx.radPunho = MODEL.q8 != null ? Convert.ToBoolean(MODEL.q8) : (Boolean?)null;
                             fx.dtPunhoI = string.IsNullOrWhiteSpace(MODEL.dtPunhoI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtPunhoI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtPunhoF = string.IsNullOrWhiteSpace(MODEL.dtPunhoF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtPunhoF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtPunho = MODEL.txtPunho;

                             fx.radTornozelo = MODEL.q9 != null ? Convert.ToBoolean(MODEL.q9) : (Boolean?)null;
                             fx.dtTornozeloI = string.IsNullOrWhiteSpace(MODEL.dtTornozeloI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtTornozeloI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtTornozeloF = string.IsNullOrWhiteSpace(MODEL.dtTornozeloF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtTornozeloF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtTornozelo = MODEL.txtTornozelo;

                             fx.radOutraArtic = MODEL.q10 != null ? Convert.ToBoolean(MODEL.q10) : (Boolean?)null;
                             fx.dtOutraArticI = string.IsNullOrWhiteSpace(MODEL.dtOutraArticI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOutraArticI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtOutraArticF = string.IsNullOrWhiteSpace(MODEL.dtOutraArticF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOutraArticF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtOutraArtic1 = MODEL.txtOutraArtic1;
                             fx.txtOutraArtic2 = MODEL.txtOutraArtic2;

                             fx.radParkinson = MODEL.q11 != null ? Convert.ToBoolean(MODEL.q11) : (Boolean?)null;
                             fx.dtParkinsonI = string.IsNullOrWhiteSpace(MODEL.dtParkinsonI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtParkinsonI, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                             fx.radVisual = MODEL.q12 != null ? Convert.ToBoolean(MODEL.q12) : (Boolean?)null;
                             fx.dtVisualI = string.IsNullOrWhiteSpace(MODEL.dtVisualI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtVisualI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtVisualF = string.IsNullOrWhiteSpace(MODEL.dtVisualF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtVisualF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtVisual = MODEL.txtVisual;

                             fx.radAuditivo = MODEL.q13 != null ? Convert.ToBoolean(MODEL.q13) : (Boolean?)null;
                             fx.dtAuditivoI = string.IsNullOrWhiteSpace(MODEL.dtAuditivoI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtAuditivoI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtAuditivoF = string.IsNullOrWhiteSpace(MODEL.dtAuditivoF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtAuditivoF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtAuditivo = MODEL.txtAuditivo;

                             fx.radGastro = MODEL.q14 != null ? Convert.ToBoolean(MODEL.q14) : (Boolean?)null;
                             fx.dtGastroI = string.IsNullOrWhiteSpace(MODEL.dtGastroI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtGastroI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.dtGastroF = string.IsNullOrWhiteSpace(MODEL.dtGastroF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtGastroF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                             fx.txtGastro = MODEL.txtGastro;

                             fx.radCirugia = MODEL.q15 != null ? Convert.ToBoolean(MODEL.q15) : (Boolean?)null;
                             fx.txtCirugiaIdade1 = MODEL.txtCirugiaIdade1;
                             fx.txtCirugiaOnde1 = MODEL.txtCirugiaOnde1;
                             fx.txtCirugiaCausa1 = MODEL.txtCirugiaCausa1;
                             fx.txtCirugiaRestricao1 = MODEL.txtCirugiaRestricao1;
                             fx.txtCirugiaIdade2 = MODEL.txtCirugiaIdade2;
                             fx.txtCirugiaOnde2 = MODEL.txtCirugiaOnde2;
                             fx.txtCirugiaCausa2 = MODEL.txtCirugiaCausa2;
                             fx.txtCirugiaRestricao2 = MODEL.txtCirugiaRestricao2;

                             fx.radProbSaude = MODEL.q16 != null ? Convert.ToBoolean(MODEL.q16) : (Boolean?)null;
                             fx.txtProbSaude = MODEL.txtProbSaude;

                             fx.radInactividade = MODEL.q17 != null ? Convert.ToBoolean(MODEL.q17) : (Boolean?)null;
                             fx.txtInactividade = MODEL.txtInactividade;
                             fx.ACTUALIZADO_POR = int.Parse(User.Identity.GetUserId()); fx.DATA_ACTUALIZACAO = DateTime.Now;
                         });
                        databaseManager.SaveChanges();
                    }
                    else
                    {
                        GT_RespProblemasSaude fx = new GT_RespProblemasSaude();
                        fx.GT_SOCIOS_ID = databaseManager.GT_SOCIOS.Where(x => x.PES_PESSOAS_ID == MODEL.PEsId).Select(x => x.ID).FirstOrDefault();
                        
                        fx.radOsteoporose = MODEL.q1 != null ? Convert.ToBoolean(MODEL.q1) : (Boolean?)null;
                        fx.dtOsteoporoseI = string.IsNullOrWhiteSpace(MODEL.dtOsteoporoseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOsteoporoseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtOsteoporoseF = string.IsNullOrWhiteSpace(MODEL.dtOsteoporoseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOsteoporoseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtOsteoporose = MODEL.txtOsteoporose;

                        fx.radOsteoartose = MODEL.q2 != null ? Convert.ToBoolean(MODEL.q2) : (Boolean?)null;
                        fx.dtOsteoartoseI = string.IsNullOrWhiteSpace(MODEL.dtOsteoartoseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOsteoartoseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtOsteoartoseF = string.IsNullOrWhiteSpace(MODEL.dtOsteoartoseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOsteoartoseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtOsteoartose = MODEL.txtOsteoartose;

                        fx.radArticulares = MODEL.q3 != null ? Convert.ToBoolean(MODEL.q3) : (Boolean?)null;
                        fx.dtArticularesI = string.IsNullOrWhiteSpace(MODEL.dtArticularesI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtArticularesI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtArticularesF = string.IsNullOrWhiteSpace(MODEL.dtArticularesF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtArticularesF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtArticulares = MODEL.txtArticulares;

                        fx.radLesoes = MODEL.q4 != null ? Convert.ToBoolean(MODEL.q4) : (Boolean?)null;
                        fx.dtLesoesI = string.IsNullOrWhiteSpace(MODEL.dtLesoesI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtLesoesI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtLesoesF = string.IsNullOrWhiteSpace(MODEL.dtLesoesF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtLesoesF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtLesoes = MODEL.txtLesoes;

                        fx.radDor = MODEL.q5 != null ? Convert.ToBoolean(MODEL.q5) : (Boolean?)null;
                        fx.dtDorI = string.IsNullOrWhiteSpace(MODEL.dtDorI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtDorI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtDorF = string.IsNullOrWhiteSpace(MODEL.dtDorF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtDorF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtDor = MODEL.txtDor;
                        fx.txtCausaDor = MODEL.txtCausaDor;

                        fx.radEscoliose = MODEL.q5_1 != null ? Convert.ToBoolean(MODEL.q5_1) : (Boolean?)null;
                        fx.dtEscolioseI = string.IsNullOrWhiteSpace(MODEL.dtEscolioseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtEscolioseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtEscolioseF = string.IsNullOrWhiteSpace(MODEL.dtEscolioseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtEscolioseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        
                        fx.radHiperlordose = MODEL.q5_2 != null ? Convert.ToBoolean(MODEL.q5_2) : (Boolean?)null;
                        fx.dtHiperlordoseI = string.IsNullOrWhiteSpace(MODEL.dtHiperlordoseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtHiperlordoseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtHiperlordoseF = string.IsNullOrWhiteSpace(MODEL.dtHiperlordoseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtHiperlordoseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                        fx.radHipercifose = MODEL.q5_3 != null ? Convert.ToBoolean(MODEL.q5_3) : (Boolean?)null;
                        fx.dtHipercifoseI = string.IsNullOrWhiteSpace(MODEL.dtHipercifoseI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtHipercifoseI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtHipercifoseF = string.IsNullOrWhiteSpace(MODEL.dtHipercifoseF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtHipercifoseF, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                        fx.radJoelho = MODEL.q6 != null ? Convert.ToBoolean(MODEL.q6) : (Boolean?)null;
                        fx.dtJoelhoI = string.IsNullOrWhiteSpace(MODEL.dtJoelhoI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtJoelhoI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtJoelhoF = string.IsNullOrWhiteSpace(MODEL.dtJoelhoF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtJoelhoF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtJoelho = MODEL.txtJoelho;

                        fx.radOmbro = MODEL.q7 != null ? Convert.ToBoolean(MODEL.q7) : (Boolean?)null;
                        fx.dtOmbroI = string.IsNullOrWhiteSpace(MODEL.dtOmbroI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOmbroI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtOmbroF = string.IsNullOrWhiteSpace(MODEL.dtOmbroF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOmbroF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtOmbro = MODEL.txtOmbro;

                        fx.radPunho = MODEL.q8 != null ? Convert.ToBoolean(MODEL.q8) : (Boolean?)null;
                        fx.dtPunhoI = string.IsNullOrWhiteSpace(MODEL.dtPunhoI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtPunhoI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtPunhoF = string.IsNullOrWhiteSpace(MODEL.dtPunhoF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtPunhoF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtPunho = MODEL.txtPunho;

                        fx.radTornozelo = MODEL.q9 != null ? Convert.ToBoolean(MODEL.q9) : (Boolean?)null;
                        fx.dtTornozeloI = string.IsNullOrWhiteSpace(MODEL.dtTornozeloI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtTornozeloI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtTornozeloF = string.IsNullOrWhiteSpace(MODEL.dtTornozeloF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtTornozeloF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtTornozelo = MODEL.txtTornozelo;

                        fx.radOutraArtic = MODEL.q10 != null ? Convert.ToBoolean(MODEL.q10) : (Boolean?)null;
                        fx.dtOutraArticI = string.IsNullOrWhiteSpace(MODEL.dtOutraArticI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOutraArticI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtOutraArticF = string.IsNullOrWhiteSpace(MODEL.dtOutraArticF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtOutraArticF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtOutraArtic1 = MODEL.txtOutraArtic1;
                        fx.txtOutraArtic2 = MODEL.txtOutraArtic2;

                        fx.radParkinson = MODEL.q11 != null ? Convert.ToBoolean(MODEL.q11) : (Boolean?)null;
                        fx.dtParkinsonI = string.IsNullOrWhiteSpace(MODEL.dtParkinsonI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtParkinsonI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        
                        fx.radVisual = MODEL.q12 != null ? Convert.ToBoolean(MODEL.q12) : (Boolean?)null;
                        fx.dtVisualI = string.IsNullOrWhiteSpace(MODEL.dtVisualI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtVisualI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtVisualF = string.IsNullOrWhiteSpace(MODEL.dtVisualF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtVisualF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtVisual = MODEL.txtVisual;

                        fx.radAuditivo = MODEL.q13 != null ? Convert.ToBoolean(MODEL.q13) : (Boolean?)null;
                        fx.dtAuditivoI = string.IsNullOrWhiteSpace(MODEL.dtAuditivoI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtAuditivoI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtAuditivoF = string.IsNullOrWhiteSpace(MODEL.dtAuditivoF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtAuditivoF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtAuditivo = MODEL.txtAuditivo;

                        fx.radGastro = MODEL.q14 != null ? Convert.ToBoolean(MODEL.q14) : (Boolean?)null;
                        fx.dtGastroI = string.IsNullOrWhiteSpace(MODEL.dtGastroI) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtGastroI, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.dtGastroF = string.IsNullOrWhiteSpace(MODEL.dtGastroF) ? (DateTime?)null : DateTime.ParseExact(MODEL.dtGastroF, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fx.txtGastro = MODEL.txtGastro;

                        fx.radCirugia = MODEL.q15 != null ? Convert.ToBoolean(MODEL.q15) : (Boolean?)null;
                        fx.txtCirugiaIdade1 = MODEL.txtCirugiaIdade1;
                        fx.txtCirugiaOnde1 = MODEL.txtCirugiaOnde1;
                        fx.txtCirugiaCausa1 = MODEL.txtCirugiaCausa1;
                        fx.txtCirugiaRestricao1 = MODEL.txtCirugiaRestricao1;
                        fx.txtCirugiaIdade2 = MODEL.txtCirugiaIdade2;
                        fx.txtCirugiaOnde2 = MODEL.txtCirugiaOnde2;
                        fx.txtCirugiaCausa2 = MODEL.txtCirugiaCausa2;
                        fx.txtCirugiaRestricao2 = MODEL.txtCirugiaRestricao2;

                        fx.radProbSaude = MODEL.q16 != null ? Convert.ToBoolean(MODEL.q16) : (Boolean?)null;
                        fx.txtProbSaude = MODEL.txtProbSaude;

                        fx.radInactividade = MODEL.q17 != null ? Convert.ToBoolean(MODEL.q17) : (Boolean?)null;
                        fx.txtInactividade = MODEL.txtInactividade;

                        fx.INSERIDO_POR = int.Parse(User.Identity.GetUserId());
                        fx.DATA_INSERCAO = DateTime.Now;
                        databaseManager.GT_RespProblemasSaude.Add(fx);
                        databaseManager.SaveChanges();
                    }
               
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GTQuestTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Flexibilidade
        public ActionResult Flexibility(Flexibility MODEL, int? Id,int? flexiType)
        {
            var tipoList = databaseManager.GT_TipoTesteFlexibilidade.ToList();
            MODEL.TipoList = tipoList.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
            MODEL.TipoId = MODEL.TipoList.Any()? Convert.ToInt32(MODEL.TipoList.FirstOrDefault().Value):0;

            if (flexiType != null && flexiType > 0)
            {
                var data = tipoList.Where(x => x.ID == flexiType).ToList();
                if (data.Count() == 0)
                    return RedirectToAction("", "home", new { Id = string.Empty });
                MODEL.TipoId = flexiType.Value;
            }

            MODEL.PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;
          
            if (Id > 0)
            {
                var data = databaseManager.GT_RespFlexiTeste.Where(x => x.ID == Id).ToList();
                if (data.Count() == 0)
                    return RedirectToAction("flexibility", "gtmanagement", new { Id = string.Empty });
                ViewBag.data = data;
                MODEL.ID = Id;
                MODEL.TipoId = data.First().GT_TipoTesteFlexibilidade_ID;

                if (MODEL.TipoId == 1)
                {
                    MODEL.iFlexiAct = data.First().RESP_SUMMARY;
                    MODEL.lblResActualFlexi = data.First().RESP_DESCRICAO;
                    MODEL.iFlexiAnt = GetFlexiIndiceAnterior(data.First().GT_SOCIOS_ID, Id);
                    MODEL.lblResAnteriorFlexi = MODEL.iFlexiAnt != null ? GetResultadoFlexiIndice(MODEL.iFlexiAnt.Value) : string.Empty;

                    var flexflexNumberArr = data.Select(x => new List<int?>
                {
                    x.RESP_01,
                    x.RESP_02,
                    x.RESP_03,
                    x.RESP_04,
                    x.RESP_05,
                    x.RESP_06,
                    x.RESP_07,
                    x.RESP_08,
                    x.RESP_09,
                    x.RESP_10,
                    x.RESP_11,
                    x.RESP_12,
                    x.RESP_13,
                    x.RESP_14,
                    x.RESP_15,
                    x.RESP_16,
                    x.RESP_17,
                    x.RESP_18,
                    x.RESP_19,
                    x.RESP_20
                }).ToArray();
                    ViewBag.flexflexNumberArr = flexflexNumberArr.First().ToList();
                }
                if (MODEL.TipoId == 2)
                {
                    int iPerc;
                    int iValue;
                    string sRes;
                    MODEL.TENTATIVA1 = data.First().TENTATIVA1;
                    MODEL.TENTATIVA2 = data.First().TENTATIVA2;
                    MODEL.ESPERADO = data.First().ESPERADO;
                    DoLoadValuesPercentilAlcancar();
                    DoGraficoActualSentar(MODEL.TENTATIVA1.Value, MODEL.TENTATIVA2.Value, out iPerc, out iValue, out sRes);
                    MODEL.iFlexiAct = iPerc;
                    MODEL.RESULTADO = iValue;
                    MODEL.lblResActualFlexi = sRes;

                    if (GetFlexiIndiceAnteriorType2(data.First().GT_SOCIOS_ID, MODEL.ID) != null)
                    {
                        MODEL.iFlexiAnt = GetPercentil(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE), GetFlexiIndiceAnteriorType2(data.First().GT_SOCIOS_ID, MODEL.ID).Value);
                        MODEL.lblResAnteriorFlexi = MODEL.iFlexiAnt != null ? GetResultadoSentarAlcancar(MODEL.iFlexiAnt.Value) : string.Empty;
                    }
                }
            }
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Quest_Flex;
            return View("Quest/Flexibility", MODEL);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Flexibility(Flexibility MODEL, int?[] flexflexNumberArr)
        {
            var GT_SOCIOS_ID = 0;

            try
            {
                //  VALIDATE FORM FIRST
                if (!ModelState.IsValid)
                {
                    string errors = string.Empty;
                    ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                    return Json(new { result = false, error = errors });
                }

                DoLoadValuesPercentilAlcancar();
                GT_SOCIOS_ID = databaseManager.GT_SOCIOS.Where(x => x.PES_PESSOAS_ID == MODEL.PEsId).Select(x => x.ID).FirstOrDefault();

                if (MODEL.TipoId == 1)
                {
                    //Type1
                    MODEL.iFlexiAct = GetFlexiIndice(flexflexNumberArr);
                    MODEL.lblResActualFlexi = GetResultadoFlexiIndice(MODEL.iFlexiAct.Value);
                }
                if (MODEL.TipoId == 2)
                {
                    //Type2
                    int iPerc;
                    int iValue;
                    string sRes;
                    MODEL.ESPERADO = DoSetEsperado(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE));
                    DoGraficoActualSentar(MODEL.TENTATIVA1.Value, MODEL.TENTATIVA2.Value, out iPerc, out iValue, out sRes);
                    MODEL.iFlexiAct = iPerc;
                    MODEL.RESULTADO = iValue;
                    MODEL.lblResActualFlexi = sRes;
                }

                if (MODEL.ID > 0)
                {
                    (from c in databaseManager.GT_RespFlexiTeste
                     where c.ID == MODEL.ID
                     select c).ToList().ForEach(fx => {
                         if (flexflexNumberArr != null)
                         {
                             fx.RESP_01 = flexflexNumberArr[0];
                             fx.RESP_02 = flexflexNumberArr[1];
                             fx.RESP_03 = flexflexNumberArr[2];
                             fx.RESP_04 = flexflexNumberArr[3];
                             fx.RESP_05 = flexflexNumberArr[4];
                             fx.RESP_06 = flexflexNumberArr[5];
                             fx.RESP_07 = flexflexNumberArr[6];
                             fx.RESP_08 = flexflexNumberArr[7];
                             fx.RESP_09 = flexflexNumberArr[8];
                             fx.RESP_10 = flexflexNumberArr[9];
                             fx.RESP_11 = flexflexNumberArr[10];
                             fx.RESP_12 = flexflexNumberArr[11];
                             fx.RESP_13 = flexflexNumberArr[12];
                             fx.RESP_14 = flexflexNumberArr[13];
                             fx.RESP_15 = flexflexNumberArr[14];
                             fx.RESP_16 = flexflexNumberArr[15];
                             fx.RESP_17 = flexflexNumberArr[16];
                             fx.RESP_18 = flexflexNumberArr[17];
                             fx.RESP_19 = flexflexNumberArr[18];
                             fx.RESP_20 = flexflexNumberArr[19];
                         }
                         //Type2
                         fx.TENTATIVA1 = MODEL.TENTATIVA1;
                         fx.TENTATIVA2 = MODEL.TENTATIVA2;
                         fx.ESPERADO = MODEL.ESPERADO;
                         fx.RESP_SUMMARY = MODEL.iFlexiAct;
                         fx.RESP_DESCRICAO = MODEL.lblResActualFlexi;
                         fx.PERCENTIL = MODEL.iFlexiAct;
                         fx.ACTUALIZADO_POR = int.Parse(User.Identity.GetUserId()); fx.DATA_ACTUALIZACAO = DateTime.Now;
                     });
                    databaseManager.SaveChanges();
                }
                else
                {
                    GT_RespFlexiTeste fx = new GT_RespFlexiTeste();
                    fx.GT_SOCIOS_ID = GT_SOCIOS_ID;
                    fx.GT_TipoTesteFlexibilidade_ID = MODEL.TipoId;
                    if (flexflexNumberArr != null)
                    {
                        fx.RESP_01 = flexflexNumberArr[0];
                        fx.RESP_02 = flexflexNumberArr[1];
                        fx.RESP_03 = flexflexNumberArr[2];
                        fx.RESP_04 = flexflexNumberArr[3];
                        fx.RESP_05 = flexflexNumberArr[4];
                        fx.RESP_06 = flexflexNumberArr[5];
                        fx.RESP_07 = flexflexNumberArr[6];
                        fx.RESP_08 = flexflexNumberArr[7];
                        fx.RESP_09 = flexflexNumberArr[8];
                        fx.RESP_10 = flexflexNumberArr[9];
                        fx.RESP_11 = flexflexNumberArr[10];
                        fx.RESP_12 = flexflexNumberArr[11];
                        fx.RESP_13 = flexflexNumberArr[12];
                        fx.RESP_14 = flexflexNumberArr[13];
                        fx.RESP_15 = flexflexNumberArr[14];
                        fx.RESP_16 = flexflexNumberArr[15];
                        fx.RESP_17 = flexflexNumberArr[16];
                        fx.RESP_18 = flexflexNumberArr[17];
                        fx.RESP_19 = flexflexNumberArr[18];
                        fx.RESP_20 = flexflexNumberArr[19];
                    }
                    //Type2
                    fx.TENTATIVA1 = MODEL.TENTATIVA1;
                    fx.TENTATIVA2 = MODEL.TENTATIVA2;
                    fx.ESPERADO = MODEL.ESPERADO;
                    fx.RESP_SUMMARY = MODEL.iFlexiAct;
                    fx.RESP_DESCRICAO = MODEL.lblResActualFlexi;
                    fx.PERCENTIL = MODEL.iFlexiAct;
                    fx.INSERIDO_POR = int.Parse(User.Identity.GetUserId());
                    fx.DATA_INSERCAO = DateTime.Now;
                    databaseManager.GT_RespFlexiTeste.Add(fx);
                    databaseManager.SaveChanges();

                    MODEL.ID = fx.ID;
                }

                if (MODEL.TipoId == 1)
                {
                    //Type1
                    MODEL.iFlexiAnt = GetFlexiIndiceAnterior(GT_SOCIOS_ID, MODEL.ID);
                    MODEL.lblResAnteriorFlexi = MODEL.iFlexiAnt != null ? GetResultadoFlexiIndice(MODEL.iFlexiAnt.Value) : string.Empty;
                }
                if (MODEL.TipoId == 2)
                {
                    //Type2
                    if (GetFlexiIndiceAnteriorType2(GT_SOCIOS_ID, MODEL.ID) != null)
                    {
                        MODEL.iFlexiAnt = GetPercentil(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE), GetFlexiIndiceAnteriorType2(GT_SOCIOS_ID, MODEL.ID).Value);
                        MODEL.lblResAnteriorFlexi = MODEL.iFlexiAnt != null ? GetResultadoSentarAlcancar(MODEL.iFlexiAnt.Value) : string.Empty;
                    }
                }

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty,
                flexAct= MODEL.iFlexiAct+"-"+ MODEL.lblResActualFlexi, flexAnt = MODEL.iFlexiAnt + "-" + MODEL.lblResAnteriorFlexi,
                tentativas = MODEL.ESPERADO+"-"+MODEL.RESULTADO,
                table = "GTQuestTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }


        //Composicao Corporal
        public ActionResult BodyComposition(BodyComposition MODEL, int? Id)
        {
            MODEL.PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;

            MODEL.GT_TipoNivelActividade_List = databaseManager.GT_TipoNivelActividade.OrderBy(x => x.ID).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
            MODEL.GT_TipoMetodoComposicao_List = databaseManager.GT_TipoMetodoComposicao.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
            int tipoComp = MODEL.GT_TipoMetodoComposicao_List.Any()?Convert.ToInt32(MODEL.GT_TipoMetodoComposicao_List.Select(X => X.Value).FirstOrDefault()):0;
            MODEL.GT_TipoTesteComposicao_List = databaseManager.GT_TipoTesteComposicao.Where(x=>x.GT_TipoMetodoComposicao_ID== tipoComp).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
            MODEL.Actual = !string.IsNullOrEmpty(Configs.GESTREINO_AVALIDO_PESO)?decimal.Parse(Configs.GESTREINO_AVALIDO_PESO).ToString("G29").Replace(",","."):string.Empty;
          
            if (Id > 0)
            {
                var data = databaseManager.GT_RespComposicao.Where(x => x.ID == Id).ToList();
                if (data.Count() == 0)
                    return RedirectToAction("bodycomposition", "gtmanagement", new { Id = string.Empty });
                ViewBag.data = data;
                MODEL.ID = Id;
                MODEL.GT_TipoTesteComposicao_ID = data.First().GT_TipoTesteComposicao_ID;
                MODEL.Actual = (!string.IsNullOrEmpty(data.First().PESO.ToString())) ? (data.First().PESO ?? 0).ToString("G29").Replace(",", ".") : null;
                MODEL.Desejavel = data.First().PESODESEJAVEL;
                MODEL.Aperder = data.First().PESOPERDER;
                MODEL.Abdominal = data.First().PERIMETRO_ABDOMINAL;
                MODEL.Cintura = data.First().PERIMETRO_CINTURA;
                MODEL.PerimetroUmbigo = data.First().PERIMETRO_UMBIGO;
                MODEL.Tricipital = data.First().TRICIPITAL;
                MODEL.TricipitalFem = data.First().TRICIPITAL;
                MODEL.AbdominalFem = data.First().PREGAS_ABDOMINAL;
                MODEL.Resistencia = data.First().RESISTENCIA;
                MODEL.Pregas = data.First().PREGASTRICIPITAL;
                MODEL.SupraIliacaFem = data.First().PREGASSUPRALLIACA;
                MODEL.Subescapular = data.First().PREGAS_SUBESCAPULAR;
                MODEL.Peitoral = data.First().PREGAS_PEITO;
                MODEL.PercMG = data.First().PERCMG;
                MODEL.MIG = data.First().MIG;
                MODEL.MG = data.First().MG;
                MODEL.PercMGDesejavel = data.First().MGDESEJAVEL;
                MODEL.MetabolismoRepouso = data.First().METABOLISMO;
                MODEL.Estimacao = data.First().ESTIMACAO;
                MODEL.GT_TipoNivelActividade_ID = data.First().GT_TipoNivelActividade_ID.Value;
                MODEL.lblDataInsercao = data.First().DATA_INSERCAO;

                int iPerc;
                decimal iValue;
                string sRes;
                DoLoadValuesPercentilComposicao();
                DoGetActualComposicao(MODEL.PercMG, out iPerc, out iValue, out sRes);

                MODEL.iFlexiAct = iPerc;
                MODEL.lblResActualFlexi = sRes;

                if (GetValorAnteriorComposicao(data.First().GT_SOCIOS_ID, MODEL.ID, MODEL.GT_TipoTesteComposicao_ID) != null)
                {
                    MODEL.iFlexiAnt = GetPercentilComposicao(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE), GetValorAnteriorComposicao(data.First().GT_SOCIOS_ID, MODEL.ID, MODEL.GT_TipoTesteComposicao_ID).Value);
                    MODEL.lblResAnteriorFlexi = MODEL.iFlexiAnt != null ? GetResultadoComposicao(MODEL.iFlexiAnt.Value) : string.Empty;
                }
            }
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Quest_BodyComposition;
            return View("Quest/BodyComposition", MODEL);
        }
        [HttpGet]
        public ActionResult loadGT_TipoTesteComposicao_List(int? Id)
        {
            return Json(databaseManager.GT_TipoTesteComposicao.Where(x => x.GT_TipoMetodoComposicao_ID == Id).Select(x => new
            {  x.ID, x.DESCRICAO
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BodyComposition(BodyComposition MODEL)
        {
            var GT_SOCIOS_ID = 0;

            try
            {
                MODEL.GT_TipoNivelActividade_List = databaseManager.GT_TipoNivelActividade.OrderBy(x => x.ID).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
                MODEL.GT_TipoMetodoComposicao_List = databaseManager.GT_TipoMetodoComposicao.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
                MODEL.GT_TipoTesteComposicao_List = databaseManager.GT_TipoTesteComposicao.Where(x => x.GT_TipoMetodoComposicao_ID == MODEL.GT_TipoMetodoComposicao_ID).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
                MODEL.IMC = databaseManager.PES_PESSOAS_CARACT.Where(x => x.PES_PESSOAS_ID == MODEL.PEsId).Select(x => x.IMC).FirstOrDefault();

                MODEL.PercMG = (MODEL.PercMG != null) ? decimal.Parse(MODEL.PercMG.ToString().Replace(",","."), CultureInfo.InvariantCulture) : (Decimal?)null;

                //  VALIDATE FORM FIRST
                if (!ModelState.IsValid)
                {
                    string errors = string.Empty;
                    ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                    return Json(new { result = false, error = errors });
                }

                GT_SOCIOS_ID = databaseManager.GT_SOCIOS.Where(x => x.PES_PESSOAS_ID == MODEL.PEsId).Select(x => x.ID).FirstOrDefault();
                DoLoadValuesPercentilComposicao();
                DoCalculaValores(MODEL);

                int iPerc;
                decimal iValue;
                string sRes;

                DoGetActualComposicao(MODEL.PercMG,out iPerc, out iValue, out sRes);

                MODEL.iFlexiAct = iPerc;
                MODEL.lblResActualFlexi = sRes;
                MODEL.Tricipital = Configs.GESTREINO_AVALIDO_SEXO == "Masculino" ? MODEL.Tricipital : MODEL.TricipitalFem;
               
                if (MODEL.ID > 0)
                {
                    (from c in databaseManager.GT_RespComposicao
                     where c.ID == MODEL.ID
                     select c).ToList().ForEach(fx => {
                         fx.GT_TipoTesteComposicao_ID = MODEL.GT_TipoTesteComposicao_ID;
                         fx.PESO = (!string.IsNullOrEmpty(MODEL.Actual)) ? decimal.Parse(MODEL.Actual, CultureInfo.InvariantCulture) : (Decimal?)null;
                         fx.PESODESEJAVEL = MODEL.Desejavel;
                         fx.PESOPERDER = MODEL.Aperder;
                         fx.PERIMETRO_ABDOMINAL = MODEL.Abdominal;
                         fx.PERIMETRO_CINTURA = MODEL.Cintura;
                         fx.PERIMETRO_UMBIGO = MODEL.PerimetroUmbigo;
                         fx.ABDOMINAL = MODEL.Abdominal;
                         fx.TRICIPITAL = MODEL.Tricipital;
                         fx.RESISTENCIA = MODEL.Resistencia;
                         fx.PREGASTRICIPITAL = MODEL.Pregas;
                         fx.PREGASSUPRALLIACA = MODEL.SupraIliacaFem;
                         fx.PREGAS_ABDOMINAL = MODEL.AbdominalFem;
                         fx.PREGAS_SUBESCAPULAR = MODEL.Subescapular;
                         fx.PREGAS_PEITO = MODEL.Peitoral;
                         fx.PERCMG = MODEL.PercMG;
                         fx.MIG = MODEL.MIG;
                         fx.MG = MODEL.MG;
                         fx.MGDESEJAVEL = MODEL.PercMGDesejavel;
                         fx.METABOLISMO = MODEL.MetabolismoRepouso;
                         fx.ESTIMACAO = MODEL.Estimacao;
                         fx.GT_TipoNivelActividade_ID = MODEL.GT_TipoNivelActividade_ID;
                         fx.RESP_SUMMARY = iValue;
                         fx.RESP_DESCRICAO = sRes;
                         fx.PERCENTIL = iPerc;
                         fx.ACTUALIZADO_POR = int.Parse(User.Identity.GetUserId()); fx.DATA_ACTUALIZACAO = DateTime.Now;
                     });
                    databaseManager.SaveChanges();
                }
                else
                {
                    GT_RespComposicao fx = new GT_RespComposicao();
                    fx.GT_SOCIOS_ID = GT_SOCIOS_ID;
                    fx.GT_TipoTesteComposicao_ID = MODEL.GT_TipoTesteComposicao_ID;
                    fx.PESO = (!string.IsNullOrEmpty(MODEL.Actual)) ? decimal.Parse(MODEL.Actual, CultureInfo.InvariantCulture) : (Decimal?)null;
                    fx.PESODESEJAVEL = MODEL.Desejavel;
                    fx.PESOPERDER = MODEL.Aperder;
                    fx.PERIMETRO_ABDOMINAL = MODEL.Abdominal;
                    fx.PERIMETRO_CINTURA = MODEL.Cintura;
                    fx.PERIMETRO_UMBIGO = MODEL.PerimetroUmbigo;
                    fx.ABDOMINAL = MODEL.Abdominal;
                    fx.TRICIPITAL = MODEL.Tricipital;
                    fx.RESISTENCIA = MODEL.Resistencia;
                    fx.PREGASTRICIPITAL = MODEL.Pregas;
                    fx.PREGASSUPRALLIACA = MODEL.SupraIliacaFem;
                    fx.PREGAS_ABDOMINAL = MODEL.AbdominalFem;
                    fx.PREGAS_SUBESCAPULAR = MODEL.Subescapular;
                    fx.PREGAS_PEITO = MODEL.Peitoral;
                    fx.PERCMG = MODEL.PercMG;
                    fx.MIG = MODEL.MIG;
                    fx.MG = MODEL.MG;
                    fx.MGDESEJAVEL = MODEL.PercMGDesejavel;
                    fx.METABOLISMO = MODEL.MetabolismoRepouso;
                    fx.ESTIMACAO = MODEL.Estimacao;
                    fx.GT_TipoNivelActividade_ID = MODEL.GT_TipoNivelActividade_ID;
                    fx.RESP_SUMMARY = iValue;
                    fx.RESP_DESCRICAO = sRes;
                    fx.PERCENTIL = iPerc;
                    fx.INSERIDO_POR = int.Parse(User.Identity.GetUserId());
                    fx.DATA_INSERCAO = DateTime.Now;
                    databaseManager.GT_RespComposicao.Add(fx);
                    databaseManager.SaveChanges();

                    MODEL.ID = fx.ID;
                }

                if (GetValorAnteriorComposicao(GT_SOCIOS_ID, MODEL.ID, MODEL.GT_TipoTesteComposicao_ID) != null)
                {
                    MODEL.iFlexiAnt = GetPercentilComposicao(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE), GetValorAnteriorComposicao(GT_SOCIOS_ID, MODEL.ID, MODEL.GT_TipoTesteComposicao_ID).Value);
                    MODEL.lblResAnteriorFlexi = MODEL.iFlexiAnt != null ? GetResultadoComposicao(MODEL.iFlexiAnt.Value) : string.Empty;
                }

                MODEL.lblDataInsercao = databaseManager.GT_RespComposicao.Where(x => x.ID == MODEL.ID).Select(X => X.DATA_INSERCAO).FirstOrDefault();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Quest_BodyComposition;
            return View("Quest/BodyComposition", MODEL);
        }


        //Cardio
        public ActionResult Cardio(Cardio MODEL, int? Id, int? flexiType)
        {
            MODEL.PEsId = !string.IsNullOrEmpty(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) ? int.Parse(Cookies.ReadCookie(Cookies.COOKIES_GESTREINO_AVALIADO)) : 0;

            MODEL.GT_TipoMetodoComposicao_List = databaseManager.GT_TipoMetodoCardio.OrderBy(x => x.ID).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
            int tipoComp = MODEL.GT_TipoMetodoComposicao_List.Any() ? Convert.ToInt32(MODEL.GT_TipoMetodoComposicao_List.Select(X => X.Value).FirstOrDefault()) : 0;
            MODEL.GT_TipoTesteCardio_List = databaseManager.GT_TipoTesteCardio.Where(x=>x.GT_TipoMetodoCardio_ID==tipoComp).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
          
            if (Id > 0)
            {
                var data = databaseManager.GT_RespAptidaoCardio.Where(x => x.ID == Id).ToList();
                if (data.Count() == 0)
                    return RedirectToAction("cardio", "gtmanagement", new { Id = string.Empty });

                ViewBag.data = data;
                MODEL.ID = Id;
                MODEL.GT_TipoTesteCardio_ID = data.First().GT_TipoTesteCardio_ID;
                MODEL.V02max = data.First().VO2MAX;
                MODEL.V02desejavel = data.First().VO2DESEJAVEL;
                MODEL.CustoCalorico = data.First().CUSTOCAL;
                MODEL.TempoRealizacao200 = data.First().TEMPO;
                MODEL.MediaWatts = data.First().MEDIA;
                MODEL.Distancia12m = data.First().DISTANCIA;
                MODEL.Tempo1600m = data.First().TEMPO;
                MODEL.Frequencia400m = data.First().FC400M;
                MODEL.FrequenciaFimTeste = data.First().FCFIMTESTE;
                MODEL.MediaFrequencia = data.First().MEDIA;
                MODEL.FC15sec = data.First().FC15M;
                MODEL.FC3min = data.First().FC3M;
                MODEL.Velocidade = data.First().VELOCIDADE;
                MODEL.Carga = data.First().CARGA;
                MODEL.FC4min = data.First().FC4M;
                MODEL.FC5min = data.First().FC5M;
                MODEL.ValorMedioFC = data.First().MEDIA;
                MODEL.VO2Carga = data.First().V02CARGA;

                int iPerc;
                decimal iValue;
                string sRes;

                //DoLoadValuesPercentilComposicao();
                //DoGetActualComposicao(MODEL.PercMG, out iPerc, out iValue, out sRes);

                //MODEL.iFlexiAct = iPerc;
                //MODEL.lblResActualFlexi = sRes;

                /*
                if (GetValorAnteriorComposicao(data.First().GT_SOCIOS_ID, MODEL.ID, MODEL.GT_TipoTesteComposicao_ID) != null)
                {
                    MODEL.iFlexiAnt = GetPercentilComposicao(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE), GetValorAnteriorComposicao(data.First().GT_SOCIOS_ID, MODEL.ID, MODEL.GT_TipoTesteComposicao_ID).Value);
                    MODEL.lblResAnteriorFlexi = MODEL.iFlexiAnt != null ? GetResultadoComposicao(MODEL.iFlexiAnt.Value) : string.Empty;
                }
                */
            }
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Quest_Cardio;
            return View("Quest/Cardio", MODEL);
        }
        [HttpGet]
        public ActionResult loadGT_TipoMetodoCardio_List(int? Id)
        {
            return Json(databaseManager.GT_TipoTesteCardio.Where(x => x.GT_TipoMetodoCardio_ID == Id).Select(x => new
            {
                x.ID,
                x.DESCRICAO
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cardio(Cardio MODEL)
        {
            var GT_SOCIOS_ID = 0;

            try
            {
                MODEL.GT_TipoMetodoComposicao_List = databaseManager.GT_TipoMetodoCardio.OrderBy(x => x.ID).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });
                MODEL.GT_TipoTesteCardio_List = databaseManager.GT_TipoTesteCardio.Where(x => x.GT_TipoMetodoCardio_ID == MODEL.GT_TipoTesteCardio_ID).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DESCRICAO });

                //  VALIDATE FORM FIRST
                if (!ModelState.IsValid)
                {
                    string errors = string.Empty;
                    ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                    return Json(new { result = false, error = errors });
                }

                GT_SOCIOS_ID = databaseManager.GT_SOCIOS.Where(x => x.PES_PESSOAS_ID == MODEL.PEsId).Select(x => x.ID).FirstOrDefault();
                DoLoadValuesPercentilComposicao();
               

                int iPerc;
                decimal iValue;
                string sRes;


                //Cálculo de valores
                CalculaValoresRemo2000(MODEL);

                //Cálculo do percentil, dá jeito ter na base de dados!!
                ValorAct = Convert.ToDecimal(txtRemo2000Vo2.Text);
                iPercentilAct = GetPercentilCardioresp(Convert.ToInt32(Sexo_), Idade_, ValorAct);

                DoGetActualRemo2000(out iPerc, out iValue, out sRes);





                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Quest_Cardio;
            return View("Quest/Cardio", MODEL);
        }













        //Formulas exported from legacy project
        private string GetResult(GT_Quest_Anxient MODEL)
        {
            string sResultAns;
            string sResultDep;
            int iResultAns;
            int iResultDep;

            sResultAns = String.Empty;
            sResultDep = String.Empty;
            iResultAns = 0;
            iResultDep = 0;

            PropertyInfo[] properties = typeof(GT_Quest_Anxient).GetProperties();
            List<string> f = new List<string> { };
            int countpos = 0;

            foreach (PropertyInfo property in properties)
            {
                countpos++;
                var val = property.GetValue(MODEL);
                if (val != null && property.Name.Contains("q"))
                {

                    if (Math.IEEERemainder(Convert.ToInt32(countpos.ToString()), 2) != 0)
                    {
                        iResultDep += GetResultDepressao(val.ToString());
                    }
                    else
                    {
                        iResultAns += GetResultAnsiedade(val.ToString());
                    }

                }
            }

            /*foreach (DictionaryEntry de in DictRespostas)
            {
                if (Math.IEEERemainder(Convert.ToInt32(de.Key.ToString()), 2) != 0)
                {
                    iResultDep += GetResultDepressao(de.Value.ToString());
                }
                else
                {
                    iResultAns += GetResultAnsiedade(de.Value.ToString());
                }
            }*/

            //Criar as strings de resultados - Ansiedade
            if (iResultAns >= 8)
                sResultAns = "1";
            else
                sResultAns = "0";

            //Criar as strings de resultados - Depressão
            if (iResultDep >= 8)
                sResultDep = "1";
            else
                sResultDep = "0";

            //return Convert.ToString(iResultDep) + " " + Convert.ToString(iResultAns);
            return sResultAns + sResultDep;
        }
        private string GetResultQuest(GT_Quest_Anxient MODEL)
        {
            string sResultAns;
            string sResultDep;
            int iResultAns;
            int iResultDep;

            sResultAns = String.Empty;
            sResultDep = String.Empty;
            iResultAns = 0;
            iResultDep = 0;



            PropertyInfo[] properties = typeof(GT_Quest_Anxient).GetProperties();
            List<string> f = new List<string> { };
            int countpos = 0;

            foreach (PropertyInfo property in properties)
            {
                countpos++;
                var val = property.GetValue(MODEL);
                if (val != null && property.Name.Contains("q"))
                {

                    if (Math.IEEERemainder(Convert.ToInt32(countpos.ToString()), 2) != 0)
                    {
                        iResultDep += GetResultDepressao(val.ToString());
                    }
                    else
                    {
                        iResultAns += GetResultAnsiedade(val.ToString());
                    }

                }
            }


            /*foreach (DictionaryEntry de in DictRespostas)
            {
                if (Math.IEEERemainder(Convert.ToInt32(de.Key.ToString()), 2) != 0)
                {
                    iResultDep += GetResultDepressao(de.Value.ToString());
                }
                else
                {
                    iResultAns += GetResultAnsiedade(de.Value.ToString());
                }
            }*/

            //Criar as strings de resultados - Ansiedade
            if (iResultAns >= 8)
                sResultAns = "O paciente encontra-se num estado de ansiedade.\n";
            else
                sResultAns = "O paciente não se encontra num estado de ansiedade.\n";

            //Criar as strings de resultados - Depressão
            if (iResultDep >= 8)
                sResultDep = "O paciente encontra-se num estado depressivo.\n";
            else
                sResultDep = "O paciente não se encontra num estado depressivo.\n";

            //return Convert.ToString(iResultDep) + " " + Convert.ToString(iResultAns);
            return sResultAns + sResultDep;
        }
        private int GetResultAnsiedade(string sQuestion)
        {

            switch (sQuestion)
            {
                case "1":
                    return 3;
                //break;
                case "2":
                    return 2;
                //break;
                case "3":
                    return 1;
                //break;
                case "4":
                    return 0;
                    //break;

            }
            return 0;
        }
        private int GetResultDepressao(string sQuestion)
        {
            switch (sQuestion)
            {
                case "1":
                    return 0;
                //break;
                case "2":
                    return 1;
                //break;
                case "3":
                    return 2;
                //break;
                case "4":
                    return 3;
                    //break;
            }

            return 0;
        }
        private string GetResultQuestSelfConcept(GT_Quest_SelfConcept MODEL,out int iValue)
        {
            string sResultQuest;
            int iResultQuest;

            sResultQuest = String.Empty;
            iResultQuest = 0;

            PropertyInfo[] properties = typeof(GT_Quest_SelfConcept).GetProperties();
            List<string> f = new List<string> { };
            int countpos = 0;

            foreach (PropertyInfo property in properties)
            {
                countpos++;
                var val = property.GetValue(MODEL);
                if (val != null && property.Name.Contains("q"))
                {

                    //Para as perguntas 3,12 e 18 o resultado das perguntas é invertido
                    if (countpos.ToString() == "3" || countpos.ToString() == "12" || countpos.ToString() == "18")
                    {
                        iResultQuest += GetResultInvertidoSelfConcept(val.ToString());
                    }
                    else
                    {
                        iResultQuest += GetResultSelfConcept(val.ToString());
                    }

                }
            }
            /*foreach (DictionaryEntry de in DictRespostas)
            {
                //Para as perguntas 3,12 e 18 o resultado das perguntas é invertido
                if (de.Key.ToString() == "3" || de.Key.ToString() == "12" || de.Key.ToString() == "18")
                {
                    iResultQuest += GetResultInvertidoSelfConcept(de.Value.ToString());
                }
                else
                {
                    iResultQuest += GetResultSelfConcept(de.Value.ToString());
                }
            }*/

            //Criar as strings de resultados - Auto Conceito
            iValue = iResultQuest;
            if (iResultQuest <= 67)
                sResultQuest = "Baixo auto-conceito.\n";
            else
                sResultQuest = "Bom auto-conceito.\n";

            return sResultQuest;
        }
        private int GetResultInvertidoSelfConcept(string sQuestion)
        {
            switch (sQuestion)
            {
                case "1":
                    return 5;
                //break;
                case "2":
                    return 4;
                //break;
                case "3":
                    return 3;
                //break;
                case "4":
                    return 2;
                //break;
                case "5":
                    return 1;
                    //break;
            }
            return 0;
        }
        private int GetResultSelfConcept(string sQuestion)
        {
            switch (sQuestion)
            {
                case "1":
                    return 1;
                //break;
                case "2":
                    return 2;
                //break;
                case "3":
                    return 3;
                //break;
                case "4":
                    return 4;
                //break;
                case "5":
                    return 5;
            }
            return 0;
        }
        //
        private string GetResultQuestCoronaryRisk(CoronaryRisk MODEL, out string sValue)
        {
            string sResultRisco = string.Empty;
            string sRisco = string.Empty;
            int iFactores = 0;
            bool bSintomas = false;

            //FACTORES POSITIVOS
            //Historia Familiar
            if (MODEL.q2 == 1 || MODEL.q16==1)
                iFactores += 1;

            //Hábitos Tabágicos
            if ((MODEL.q3==1) || (MODEL.q4==1))
                iFactores += 1;

            //Hipertensão
            if ((MODEL.q5==1) || (MODEL.q6==1))
                iFactores += 1;

            //HiperColesterolemia
            //if ((radColesterol1S.Checked) || (radColesterol3S.Checked) || (radColesterol5S.Checked))
            if (MODEL.q7==1 || MODEL.q9==1 || MODEL.q11==1)
                iFactores += 1;

            //Glicose
            if ((MODEL.q12==1))
                iFactores += 1;

            //Obesidade
            if (MODEL.txtIMC >= 30)
                iFactores += 1;

            //Estilo de Vida/Inactividade Física
            if (MODEL.q13==1)
                iFactores += 1;

            //FACTORES NEGATIVOS
            if (MODEL.q10==1)
                iFactores -= 1;

            bSintomas = (MODEL.chkDor || MODEL.chkRespiracao
                        || MODEL.chkTonturas || MODEL.chkDispeneia
                        || MODEL.chkEdema || MODEL.chkPalpitacoes
                        || MODEL.chkClaudicacao || MODEL.chkMurmurio
                        || MODEL.chkfadiga
                        //Doenças conhecidas associadas ao risco coronário
                        || MODEL.chkCardiaca || MODEL.chkVascular || MODEL.chkCerebroVascular
                        || MODEL.chkCardioVascularOutras || MODEL.chkObstrucao || MODEL.chkAsma
                        || MODEL.chkFibrose || MODEL.chkPulmomarOutras || MODEL.chkDiabetes1
                        || MODEL.chkDiabetes2 || MODEL.chkRenais || MODEL.chkFigado || MODEL.chkMetabolicaOutras);

            if (bSintomas)
            {
                sResultRisco = "Risco Elevado";
                sRisco = "2";
            }
            else if (MODEL.q1==1 || iFactores >= 2)
            {
                sResultRisco = "Risco Moderado";
                sRisco = "1";
            }
            else
            {
                sResultRisco = "Risco Baixo";
                sRisco = "0";
            }

            sValue = sRisco;
            return sResultRisco;
        }

        //Flexibility
        private int GetFlexiIndice(int?[] flexflexNumberArr)
        {
            int iFlexi = 0;

            for (int x = 0; x <= 19; x++)
            {
                if (flexflexNumberArr[x] != null)
                    iFlexi = iFlexi + Convert.ToInt32(flexflexNumberArr[x]);
            }
            return iFlexi;
        }
        private string GetResultadoFlexiIndice(int iFlexi)
        {
            string retValue = string.Empty;
            if (iFlexi <= 20)
                retValue = "Muito Fraco";
            else if (iFlexi <= 30 && iFlexi >= 21)
                retValue = "Fraco";
            else if (iFlexi <= 50 && iFlexi >= 31)
                retValue = "Médio";
            else if (iFlexi <= 60 && iFlexi >= 51)
                retValue = "Bom";
            else if (iFlexi > 60)
                retValue = "Excelente";

            return retValue;
        }
        private int? GetFlexiIndiceAnterior(int GT_SOCIOS_ID,int? Id)
        {
            int? iFlexi = 0;
            var data = databaseManager.GT_RespFlexiTeste.Where(x => x.GT_SOCIOS_ID == GT_SOCIOS_ID && x.ID<Id && x.GT_TipoTesteFlexibilidade_ID==1).OrderByDescending(x => x.DATA_INSERCAO).Take(1).ToList();

            var flexflexNumberArr = data.Select(x => new List<int?>
                {
                    x.RESP_01,
                    x.RESP_02,
                    x.RESP_03,
                    x.RESP_04,
                    x.RESP_05,
                    x.RESP_06,
                    x.RESP_07,
                    x.RESP_08,
                    x.RESP_09,
                    x.RESP_10,
                    x.RESP_11,
                    x.RESP_12,
                    x.RESP_13,
                    x.RESP_14,
                    x.RESP_15,
                    x.RESP_16,
                    x.RESP_17,
                    x.RESP_18,
                    x.RESP_19,
                    x.RESP_20
                }).ToArray();

            if (flexflexNumberArr.Any())
            {
                var flexflexNumberArrList = flexflexNumberArr.First().ToList();

                if (flexflexNumberArrList.Any())
                {
                    foreach (var x in flexflexNumberArrList)
                    {
                        if (x != null)
                            iFlexi = iFlexi + Convert.ToInt32(x);
                    }
                }
                else
                    iFlexi = null;
            }
            else
                iFlexi = null;
            return iFlexi;
        }
        //
        private ArrayList a20_29M = new ArrayList(9);
        private ArrayList a20_29F = new ArrayList(9);

        private ArrayList a30_39M = new ArrayList(9);
        private ArrayList a30_39F = new ArrayList(9);

        private ArrayList a40_49M = new ArrayList(9);
        private ArrayList a40_49F = new ArrayList(9);

        private ArrayList a50_59M = new ArrayList(9);
        private ArrayList a50_59F = new ArrayList(9);

        private ArrayList a60_69M = new ArrayList(9);
        private ArrayList a60_69F = new ArrayList(9);

        private ArrayList aPercentil = new ArrayList(10);
        private ArrayList aEscolhido = new ArrayList(9);

        private void DoLoadValuesPercentilAlcancar()
        {
            a20_29M.Clear();
            a20_29F.Clear();
            a30_39M.Clear();
            a30_39F.Clear();
            a40_49M.Clear();
            a40_49F.Clear();
            a50_59M.Clear();
            a50_59F.Clear();
            a60_69M.Clear();
            a60_69F.Clear();
            aPercentil.Clear();
            aEscolhido.Clear();

            //Carregamento de Valores
            a20_29M.Add(new Object[9] { 42, 38, 36, 33, 31, 29, 26, 23, 18 });
            a20_29F.Add(new Object[9] { 43, 40, 38, 36, 34, 32, 29, 26, 22 });

            a30_39M.Add(new Object[9] { 40, 37, 34, 32, 29, 27, 24, 21, 17 });
            a30_39F.Add(new Object[9] { 42, 39, 37, 35, 33, 31, 28, 25, 21 });

            a40_49M.Add(new Object[9] { 37, 34, 30, 28, 25, 23, 20, 16, 12 });
            a40_49F.Add(new Object[9] { 40, 37, 35, 33, 31, 29, 26, 14, 19 });

            a50_59M.Add(new Object[9] { 38, 32, 29, 27, 25, 22, 18, 15, 12 });
            a50_59F.Add(new Object[9] { 40, 37, 35, 32, 30, 29, 26, 13, 19 });

            a60_69M.Add(new Object[9] { 35, 30, 26, 24, 22, 18, 16, 14, 11 });
            a60_69F.Add(new Object[9] { 37, 34, 31, 30, 28, 26, 24, 23, 18 });

            aPercentil.Add(100);
            aPercentil.Add(90);
            aPercentil.Add(80);
            aPercentil.Add(70);
            aPercentil.Add(60);
            aPercentil.Add(50);
            aPercentil.Add(40);
            aPercentil.Add(30);
            aPercentil.Add(20);
            aPercentil.Add(10);
        }
        private int GetPercentil(string Sexo, int Idade, int valor)
        {

            switch (Sexo)
            {
                case "Masculino":
                    if (Idade >= 17 && Idade <= 29)
                        aEscolhido = a20_29M;
                    else if (Idade >= 30 && Idade <= 39)
                        aEscolhido = a30_39M;
                    else if (Idade >= 40 && Idade <= 49)
                        aEscolhido = a40_49M;
                    else if (Idade >= 50 && Idade <= 59)
                        aEscolhido = a50_59M;
                    else if (Idade >= 60 && Idade <= 69)
                        aEscolhido = a60_69M;
                    break;
                case "Feminino":
                    if (Idade >= 17 && Idade <= 29)
                        aEscolhido = a20_29F;
                    else if (Idade >= 30 && Idade <= 39)
                        aEscolhido = a30_39F;
                    else if (Idade >= 40 && Idade <= 49)
                        aEscolhido = a40_49F;
                    else if (Idade >= 50 && Idade <= 59)
                        aEscolhido = a50_59F;
                    else if (Idade >= 60 && Idade <= 69)
                        aEscolhido = a60_69F;
                    break;
            }

            Array arrTemp;
            arrTemp = (Array)aEscolhido[0];
            int indice = 0;

            foreach (Object i in arrTemp)
            {
                if (valor > Convert.ToInt32(i))
                {
                    break;
                }
                indice += 1;

            }
            return Convert.ToInt32(aPercentil[indice]);
        }
        private int DoSetEsperado(string Sexo, int Idade)
        {
            int y = 0;
            switch (Sexo)
            {
                case "Masculino":
                    if (Idade >= 17 && Idade <= 29)
                        aEscolhido = a20_29M;
                    else if (Idade >= 30 && Idade <= 39)
                        aEscolhido = a30_39M;
                    else if (Idade >= 40 && Idade <= 49)
                        aEscolhido = a40_49M;
                    else if (Idade >= 50 && Idade <= 59)
                        aEscolhido = a50_59M;
                    else if (Idade >= 60 && Idade <= 69)
                        aEscolhido = a60_69M;
                    break;
                case "Feminino":
                    if (Idade >= 17 && Idade <= 29)
                        aEscolhido = a20_29F;
                    else if (Idade >= 30 && Idade <= 39)
                        aEscolhido = a30_39F;
                    else if (Idade >= 40 && Idade <= 49)
                        aEscolhido = a40_49F;
                    else if (Idade >= 50 && Idade <= 59)
                        aEscolhido = a50_59F;
                    else if (Idade >= 60 && Idade <= 69)
                        aEscolhido = a60_69F;
                    break;
            }

            Array arrTemp;
            arrTemp = (Array)aEscolhido[0];
            y = Convert.ToInt32(Convert.ToString(arrTemp.GetValue(2)));
            return y;
        }
        private void DoGraficoActualSentar(int txtTentativa1, int txtTentativa2,  out int iPerc, out int iValue, out string sRes)
        {
            int iPercentilAct;
            int ValorAct = 0;
            
            ValorAct = Convert.ToInt32((txtTentativa1 + txtTentativa2) / 2);
            iPercentilAct = GetPercentil(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE), ValorAct);
            iPerc = iPercentilAct;
            iValue = ValorAct;
            sRes = GetResultadoSentarAlcancar(iPercentilAct);
        }
        private string GetResultadoSentarAlcancar(int iSentar)
        {
            string retValue = string.Empty;
            if (iSentar < 30)
                retValue = "Muito Fraco";
            else if (iSentar < 50 && iSentar >= 30)
                retValue = "Fraco";
            else if (iSentar <= 70 && iSentar >= 50)
                retValue = "Médio";
            else if (iSentar <= 90 && iSentar >= 71)
                retValue = "Bom";
            else if (iSentar > 90)
                retValue = "Excelente";
            return retValue;
        }
        private int? GetFlexiIndiceAnteriorType2(int GT_SOCIOS_ID, int? Id)
        {
            int? iFlexi = 0;
            var data = databaseManager.GT_RespFlexiTeste.Where(x => x.GT_SOCIOS_ID == GT_SOCIOS_ID && x.ID < Id && x.GT_TipoTesteFlexibilidade_ID==2).OrderByDescending(x => x.DATA_INSERCAO).Take(1).ToList();

            var flexflexNumberArr = data.Select(x => new List<int?>
                { 
                    x.TENTATIVA1,
                    x.TENTATIVA2
                }).ToArray();

            if (flexflexNumberArr.Any())
            {
                var flexflexNumberArrList = flexflexNumberArr.First().ToList();

                if (flexflexNumberArrList.Any())
                {
                    foreach (var x in flexflexNumberArrList)
                    {
                        if (x != null)
                            iFlexi = iFlexi + Convert.ToInt32(x);
                    }
                    iFlexi = iFlexi / 2;
                }
                else
                    iFlexi = null;
            }
            else
                iFlexi = null;
            return iFlexi;
        }

        //Bodycomposition
        private void DoCalculaValores(BodyComposition MODEL)
        {
            Decimal DC;
            Decimal SumPregas;
            string sTempPesoDesejado1 = string.Empty;
            string sTempPesoDesejado2 = string.Empty;

            if (MODEL.GT_TipoTesteComposicao_ID == 1)
            {
                //Cálculo de Densidade Corporal (DC)
                if (Configs.GESTREINO_AVALIDO_SEXO == "Masculino")
                {
                    SumPregas = Convert.ToDecimal(MODEL.Peitoral) + Convert.ToDecimal(MODEL.Tricipital) + Convert.ToDecimal(MODEL.Subescapular);
                    DC = Convert.ToDecimal(1.1125025) - (Convert.ToDecimal(0.0013125) * (SumPregas)) + (Convert.ToDecimal(0.0000055) * (SumPregas * SumPregas)) - Convert.ToDecimal(0.000244) * Convert.ToDecimal(Configs.GESTREINO_AVALIDO_IDADE);

                    //Cálculo de %MG (txtPercMG)
                    MODEL.PercMG = ((Convert.ToDecimal(495) / DC) - 450);
                }
                else
                {
                    SumPregas = Convert.ToDecimal(MODEL.TricipitalFem) + Convert.ToDecimal(MODEL.SupraIliacaFem) + Convert.ToDecimal(MODEL.AbdominalFem);
                    DC = Convert.ToDecimal(1.089733) - (Convert.ToDecimal(0.0009245) * (SumPregas)) + (Convert.ToDecimal(0.0000025) * (SumPregas * SumPregas)) - Convert.ToDecimal(0.000979) * Convert.ToDecimal(Configs.GESTREINO_AVALIDO_IDADE);

                    //Cálculo de %MG (txtPercMG)
                    MODEL.PercMG = ((Convert.ToDecimal(501) / DC) - 457);
                }
                MODEL.PercMG = MODEL.PercMG.ToString().Length > 5 ? Convert.ToDecimal(MODEL.PercMG.ToString().Substring(0, 5)) : MODEL.PercMG;
            }
            //WELTMAN ET AL
            else if  (MODEL.GT_TipoTesteComposicao_ID == 2)
            {
                //Cálculo de Densidade Corporal (DC)
                if (Configs.GESTREINO_AVALIDO_SEXO == "Masculino")
                    MODEL.PercMG = Convert.ToDecimal(0.31457) * ((Convert.ToDecimal(MODEL.PerimetroUmbigo) + Convert.ToDecimal(MODEL.Abdominal)) / 2) - (Convert.ToDecimal(0.10969) * (Convert.ToDecimal(Configs.GESTREINO_AVALIDO_PESO))) + Convert.ToDecimal(10.8336);
                else
                    MODEL.PercMG = Convert.ToDecimal(0.11077) * ((Convert.ToDecimal(MODEL.PerimetroUmbigo) + Convert.ToDecimal(MODEL.Abdominal)) / 2) - (Convert.ToDecimal(0.11077) * (Convert.ToDecimal(Configs.GESTREINO_AVALIDO_ALTURA))) + (Convert.ToDecimal(0.11077) * Convert.ToDecimal(Configs.GESTREINO_AVALIDO_PESO));
                
                MODEL.PercMG = MODEL.PercMG.ToString().Length > 5 ? Convert.ToDecimal(MODEL.PercMG.ToString().Substring(0, 5)) : MODEL.PercMG;
            }
            //Gray e Col passou a Deurenberg et al
            else if (MODEL.GT_TipoTesteComposicao_ID == 3)
            {
                decimal dMIG = Convert.ToDecimal(-12.44) + Convert.ToDecimal(0.340) * (Convert.ToDecimal(Configs.GESTREINO_AVALIDO_ALTURA) * Convert.ToDecimal(Configs.GESTREINO_AVALIDO_ALTURA)) / Convert.ToDecimal(MODEL.Resistencia);
                dMIG = dMIG + Convert.ToDecimal(0.273) * Convert.ToDecimal(Configs.GESTREINO_AVALIDO_PESO);
                dMIG = dMIG + (Convert.ToDecimal(15.34) * Convert.ToDecimal(Configs.GESTREINO_AVALIDO_ALTURA)) * Convert.ToDecimal(0.01);
                dMIG = dMIG - (Convert.ToDecimal(0.127) * Convert.ToDecimal(Configs.GESTREINO_AVALIDO_IDADE));
                MODEL.MIG = dMIG;
                MODEL.MIG= MODEL.MIG.ToString().Length>5? Convert.ToDecimal(MODEL.MIG.ToString().Substring(0, 5)): MODEL.MIG;
                MODEL.MG =Convert.ToDecimal(Configs.GESTREINO_AVALIDO_PESO) - dMIG;
                MODEL.PercMG =(Convert.ToDecimal(MODEL.MG) * Convert.ToDecimal(100)) / Convert.ToDecimal(Configs.GESTREINO_AVALIDO_PESO);
                MODEL.PercMG = MODEL.PercMG.ToString().Length > 5 ? Convert.ToDecimal(MODEL.PercMG.ToString().Substring(0, 5)) : MODEL.PercMG;
            }
            //Guo e Col
            //			else if (cmbTipoTeste.SelectedValue.ToString()=="4")
            //			{
            //				txtPercMG.Text = "25";	//TODO
            //				if (txtPercMG.Text.Length > 5) txtPercMG.Text = txtPercMG.Text.Substring(0,5);
            //			}

            //%MG Desejável
            MODEL.PercMGDesejavel=  DoSetEsperadoComposicao(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE));


            //Apenas para != de  Deurenberg et al
            if(MODEL.GT_TipoTesteComposicao_ID != 3)
            {
                //Calculo do MG (txtMG)
                MODEL.MG = (Convert.ToDecimal(MODEL.PercMG) / 100) * Convert.ToDecimal(Configs.GESTREINO_AVALIDO_PESO);
                MODEL.MG = MODEL.MG.ToString().Length > 5 ? Convert.ToDecimal(MODEL.MG.ToString().Substring(0, 5)) : MODEL.MG;

                //Cálculo de MIG (txtMIG)
                MODEL.MIG = Convert.ToDecimal(Configs.GESTREINO_AVALIDO_PESO) - Convert.ToDecimal(MODEL.MG);
                MODEL.MIG = MODEL.MIG.ToString().Length > 5 ? Convert.ToDecimal(MODEL.MIG.ToString().Substring(0, 5)) : MODEL.MIG;
            }
            //Cálculo do Peso desejável (txtPesoDesejado)
            Array arrTemp;
            arrTemp = (Array)aCompEscolhido[0];

            sTempPesoDesejado1 = Convert.ToString(Convert.ToDecimal(MODEL.MIG) / (1 - (Convert.ToDecimal(0.01) * Convert.ToDecimal(arrTemp.GetValue(2)))));
            if (sTempPesoDesejado1.Length > 6) sTempPesoDesejado1 = sTempPesoDesejado1.Substring(0, 6);

            sTempPesoDesejado2 = Convert.ToString(Convert.ToDecimal(MODEL.MIG) / (1 - (Convert.ToDecimal(0.01) * Convert.ToDecimal(arrTemp.GetValue(4)))));
            if (sTempPesoDesejado2.Length > 6) sTempPesoDesejado2 = sTempPesoDesejado2.Substring(0, 6);

            MODEL.Desejavel = sTempPesoDesejado1 + " a " + sTempPesoDesejado2;

            //Cálculo do Peso a Perder (txtPesoPerder)
            if (Convert.ToDecimal(decimal.Parse(MODEL.Actual, CultureInfo.InvariantCulture)) > Convert.ToDecimal(sTempPesoDesejado2))
                MODEL.Aperder = Convert.ToDecimal(MODEL.Actual) - Convert.ToDecimal(sTempPesoDesejado2);
            else
                MODEL.Aperder =0;

            MODEL.Aperder = MODEL.Aperder.ToString().Length > 5 ? Convert.ToDecimal(MODEL.Aperder.ToString().Substring(0, 5)) : MODEL.Aperder;
          
            //Se o peso a perder for negativo então é pq tem peso a menos, Logo atribuo "0"
            if (Convert.ToDecimal(MODEL.Aperder) < 0)
                MODEL.Aperder =0;

            //Cálculo do Metabolismo de repouso (txtRepouso)
            MODEL.MetabolismoRepouso = Convert.ToDecimal(638) + (Convert.ToDecimal(MODEL.MIG) * Convert.ToDecimal(15.9));
            MODEL.MetabolismoRepouso = MODEL.MetabolismoRepouso.ToString().Length > 8 ? Convert.ToDecimal(MODEL.MetabolismoRepouso.ToString().Substring(0, 8)) : MODEL.MetabolismoRepouso;

            //Cálculo do Estimação (txtEstimacao)
            if (MODEL.GT_TipoNivelActividade_ID == 1)
               MODEL.Estimacao= Convert.ToDecimal(MODEL.MetabolismoRepouso) * Convert.ToDecimal(1.2);
            else if (MODEL.GT_TipoNivelActividade_ID == 3)
                MODEL.Estimacao = Convert.ToDecimal(MODEL.MetabolismoRepouso) * Convert.ToDecimal(1.375);
            else if (MODEL.GT_TipoNivelActividade_ID == 3)
                MODEL.Estimacao = Convert.ToDecimal(MODEL.MetabolismoRepouso) * Convert.ToDecimal(1.55);
            else if (MODEL.GT_TipoNivelActividade_ID == 4)
                MODEL.Estimacao = Convert.ToDecimal(MODEL.MetabolismoRepouso * Convert.ToDecimal(1.725));
            else if (MODEL.GT_TipoNivelActividade_ID == 5)
                MODEL.Estimacao = Convert.ToDecimal(MODEL.MetabolismoRepouso) * Convert.ToDecimal(1.9);

            MODEL.Estimacao = MODEL.Estimacao.ToString().Length > 8 ? Convert.ToDecimal(MODEL.Estimacao.ToString().Substring(0, 8)) : MODEL.Estimacao;
}

        private ArrayList aComp20_29M = new ArrayList(9);
        private ArrayList aComp20_29F = new ArrayList(9);
        private ArrayList aComp30_39M = new ArrayList(9);
        private ArrayList aComp30_39F = new ArrayList(9);
        private ArrayList aComp40_49M = new ArrayList(9);
        private ArrayList aComp40_49F = new ArrayList(9);
        private ArrayList aComp50_59M = new ArrayList(9);
        private ArrayList aComp50_59F = new ArrayList(9);
        private ArrayList aComp60_69M = new ArrayList(9);
        private ArrayList aComp60_69F = new ArrayList(9);
        private ArrayList aCompPercentil = new ArrayList(9);
        private ArrayList aCompEscolhido = new ArrayList(9);

        private void DoLoadValuesPercentilComposicao()
        {
            aComp20_29M.Clear();
            aComp20_29F.Clear();
            aComp30_39M.Clear();
            aComp30_39F.Clear();
            aComp40_49M.Clear();
            aComp40_49F.Clear();
            aComp50_59M.Clear();
            aComp50_59F.Clear();
            aComp60_69M.Clear();
            aComp60_69F.Clear();
            aCompPercentil.Clear();
            aCompEscolhido.Clear();

            //Carregamento de Valores
            aComp20_29M.Add(new Object[9] { 7.1, 9.4, 11.8, 14.1, 15.9, 17.4, 19.5, 22.4, 25.9 });
            aComp20_29F.Add(new Object[9] { 14.5, 17.1, 19.0, 20.6, 22.1, 23.7, 25.4, 27.7, 32.1 });

            aComp30_39M.Add(new Object[9] { 11.3, 13.9, 15.9, 17.5, 19, 20.5, 22.3, 24.2, 27.3 });
            aComp30_39F.Add(new Object[9] { 15.5, 18, 20, 21.6, 23.1, 24.9, 27, 29.3, 32.8 });

            aComp40_49M.Add(new Object[9] { 13.6, 16.3, 18.1, 19.6, 21.1, 22.5, 24.1, 26.1, 28.9 });
            aComp40_49F.Add(new Object[9] { 18.5, 21.3, 23.5, 24.9, 26.4, 28.1, 30.1, 32.1, 35 });

            aComp50_59M.Add(new Object[9] { 15.3, 17.9, 19.8, 21.3, 22.7, 24.1, 25.7, 27.5, 30.3 });
            aComp50_59F.Add(new Object[9] { 21.6, 25, 26.6, 28.5, 30.1, 31.6, 33.5, 35.6, 37.9 });

            aComp60_69M.Add(new Object[9] { 15.3, 18.4, 20.3, 22, 23.5, 25, 26.7, 28.5, 31.2 });
            aComp60_69F.Add(new Object[9] { 21.1, 25.1, 27.5, 29.3, 30.9, 32.5, 34.3, 36.6, 39.3 });

            aCompPercentil.Add(100);
            aCompPercentil.Add(90);
            aCompPercentil.Add(80);
            aCompPercentil.Add(70);
            aCompPercentil.Add(60);
            aCompPercentil.Add(50);
            aCompPercentil.Add(40);
            aCompPercentil.Add(30);
            aCompPercentil.Add(20);
            aCompPercentil.Add(10);
        }
        private string DoSetEsperadoComposicao(string Sexo, int Idade)
        {
            switch (Sexo)
            {
                case "Masculino":
                    if (Idade >= 17 && Idade <= 29)
                        aCompEscolhido = aComp20_29M;
                    else if (Idade >= 30 && Idade <= 39)
                        aCompEscolhido = aComp30_39M;
                    else if (Idade >= 40 && Idade <= 49)
                        aCompEscolhido = aComp40_49M;
                    else if (Idade >= 50 && Idade <= 59)
                        aCompEscolhido = aComp50_59M;
                    else if (Idade >= 60 && Idade <= 69)
                        aCompEscolhido = aComp60_69M;
                    break;
                case "Feminino":
                    if (Idade >= 17 && Idade <= 29)
                        aCompEscolhido = aComp20_29F;
                    else if (Idade >= 30 && Idade <= 39)
                        aCompEscolhido = aComp30_39F;
                    else if (Idade >= 40 && Idade <= 49)
                        aCompEscolhido = aComp40_49F;
                    else if (Idade >= 50 && Idade <= 59)
                        aCompEscolhido = aComp50_59F;
                    else if (Idade >= 60 && Idade <= 69)
                        aCompEscolhido = aComp60_69F;
                    break;
            }

            Array arrTemp;
            arrTemp = (Array)aCompEscolhido[0];
            return Convert.ToString(arrTemp.GetValue(2)) + " a " + Convert.ToString(arrTemp.GetValue(4));

        }
        private int GetPercentilComposicao(string Sexo, int Idade, decimal valor)
        {
            switch (Sexo)
            {
                case "Masculino":
                    if (Idade >= 17 && Idade <= 29)
                        aCompEscolhido = aComp20_29M;
                    else if (Idade >= 30 && Idade <= 39)
                        aCompEscolhido = aComp30_39M;
                    else if (Idade >= 40 && Idade <= 49)
                        aCompEscolhido = aComp40_49M;
                    else if (Idade >= 50 && Idade <= 59)
                        aCompEscolhido = aComp50_59M;
                    else if (Idade >= 60 && Idade <= 69)
                        aCompEscolhido = aComp60_69M;
                    break;
                case "Feminino":
                    if (Idade >= 17 && Idade <= 29)
                        aCompEscolhido = aComp20_29F;
                    else if (Idade >= 30 && Idade <= 39)
                        aCompEscolhido = aComp30_39F;
                    else if (Idade >= 40 && Idade <= 49)
                        aCompEscolhido = aComp40_49F;
                    else if (Idade >= 50 && Idade <= 59)
                        aCompEscolhido = aComp50_59F;
                    else if (Idade >= 60 && Idade <= 69)
                        aCompEscolhido = aComp60_69F;
                    break;
            }

            Array arrTemp;
            arrTemp = (Array)aCompEscolhido[0];
            int indice = 0;
            //Detectar o valor
            foreach (Object i in arrTemp)
            {
                if (valor < Convert.ToDecimal(i))
                {
                    break;
                }
                indice += 1;

            }
            //			if (indice == 9) 
            //				indice = (indice -1);

            return Convert.ToInt32(aCompPercentil[indice]);
            //return 0;
        }
        private void DoGraficoActualComposicao(decimal? PercMG)
        {
            int iPercentilAct;
            decimal ValorAct = 0;
            ValorAct = PercMG.Value;
            iPercentilAct = GetPercentilComposicao(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE), ValorAct);

        }
        private void DoGetActualComposicao(decimal? PercMG,out int iPerc, out decimal iValue, out string sRes)
        {
            int iPercentilAct;
            decimal ValorAct = 0;
            ValorAct = PercMG.Value;
            iPercentilAct = GetPercentilComposicao(Configs.GESTREINO_AVALIDO_SEXO, Convert.ToInt32(Configs.GESTREINO_AVALIDO_IDADE), ValorAct);
            sRes = GetResultadoComposicao(iPercentilAct);
            iPerc = iPercentilAct;
            iValue = ValorAct;
        }
        private string GetResultadoComposicao(int dRes)
        {
            string retValue = string.Empty;

            if (dRes < 30)
                retValue = "Muito Fraco";
            else if (dRes < 50 && dRes >= 30)
                retValue = "Fraco";
            else if (dRes <= 70 && dRes >= 50)
                retValue = "Médio";
            else if (dRes <= 90 && dRes >= 71)
                retValue = "Bom";
            else if (dRes > 90)
                retValue = "Excelente";
            return retValue;
        }
        private decimal? GetValorAnteriorComposicao(int GT_SOCIOS_ID, int? Id,int? Type)
        {
            decimal? iFlexi = 0;
            var data = databaseManager.GT_RespComposicao.Where(x => x.GT_SOCIOS_ID == GT_SOCIOS_ID && x.ID < Id && x.GT_TipoTesteComposicao_ID == Type).OrderByDescending(x => x.DATA_INSERCAO).Take(1).ToList();

            var flexflexNumberArr = data.Select(x => new List<decimal?>
                {
                    x.PERCMG
                }).ToArray();

            if (flexflexNumberArr.Any())
            {
                var flexflexNumberArrList = flexflexNumberArr.First().ToList();

                if (flexflexNumberArrList.Any())
                {
                    foreach (var x in flexflexNumberArrList)
                    {
                        if (x != null)
                            iFlexi = x;
                    }
                }
                else
                    iFlexi = null;
            }
            else
                iFlexi = null;
            return iFlexi;
        }


        //Cardio

        private string DoGetDesejavel(string Sexo, int Idade)
        {
            switch (Sexo)
            {
                case "Masculino":
                    if (Idade >= 17 && Idade <= 29)
                        aCardioEscolhido = aCardio20_29M;
                    else if (Idade >= 30 && Idade <= 39)
                        aCardioEscolhido = aCardio30_39M;
                    else if (Idade >= 40 && Idade <= 49)
                        aCardioEscolhido = aCardio40_49M;
                    else if (Idade >= 50 && Idade <= 59)
                        aCardioEscolhido = aCardio50_59M;
                    else if (Idade >= 60 && Idade <= 69)
                        aCardioEscolhido = aCardio60_69M;
                    break;
                case "Feminino":
                    if (Idade >= 17 && Idade <= 29)
                        aCardioEscolhido = aCardio20_29F;
                    else if (Idade >= 30 && Idade <= 39)
                        aCardioEscolhido = aCardio30_39F;
                    else if (Idade >= 40 && Idade <= 49)
                        aCardioEscolhido = aCardio40_49F;
                    else if (Idade >= 50 && Idade <= 59)
                        aCardioEscolhido = aCardio50_59F;
                    else if (Idade >= 60 && Idade <= 69)
                        aCardioEscolhido = aCardio60_69F;
                    break;
            }

            Array arrTemp;
            arrTemp = (Array)aCardioEscolhido[0];
            return Convert.ToString(arrTemp.GetValue(4)) + " a " + Convert.ToString(arrTemp.GetValue(2));
        }

        //2000 Metros
        private void CalculaValoresRemo2000(Cardio MODEL)
        {

            MODEL.V02max= DoGetVo2MaxRemo2000();
            MODEL.V02max = MODEL.V02max.ToString().Length > 5 ? Convert.ToDecimal(MODEL.V02max.ToString().Substring(0, 5)) : MODEL.V02max;

            MODEL.V02Mets = DoGetVo2MetsRemo2000();
            MODEL.V02Mets = MODEL.V02Mets.ToString().Length > 5 ? Convert.ToDecimal(MODEL.V02Mets.ToString().Substring(0, 5)) : MODEL.V02Mets;

            MODEL.V02desejavel = DoGetDesejavel(Convert.ToInt32(Sexo_), Idade_);

            MODEL.V02CustoCalMin = DoGetCustoCaloricoRemo2000()
            MODEL.V02CustoCalMin = MODEL.V02CustoCalMin.ToString().Length > 5 ? Convert.ToDecimal(MODEL.V02CustoCalMin.ToString().Substring(0, 5)) : MODEL.V02CustoCalMin;

        }


        private decimal DoGetVo2MaxRemo2000()
        {
            decimal retValue;

            retValue = ((Convert.ToDecimal(txtRemo2000Watts.Text) * Convert.ToDecimal(14.4) + 65) / Convert.ToDecimal(Peso_));

            return retValue;

        }


        private decimal DoGetVo2MetsRemo2000()
        {
            decimal retValue;

            retValue = DoGetVo2MaxRemo2000() / Convert.ToDecimal(3.5);

            return retValue;
        }


        private decimal DoGetCustoCaloricoRemo2000()
        {
            decimal retValue;

            retValue = (DoGetVo2MetsRemo2000() * Convert.ToDecimal(txtRemo2000Custo.Text)) / Convert.ToDecimal(100);

            retValue = retValue - 1;

            retValue = (retValue * Convert.ToDecimal(3.5) * Convert.ToDecimal(Peso_)) / Convert.ToDecimal(200);

            return retValue;
        }


        private void DoGetActualRemo2000(out int iPerc, out decimal iValue, out string sRes)
        {
            int iPercentilAct;
            decimal ValorAct = 0;

            if (txtRemo2000Vo2.Text == string.Empty) txtRemo2000Vo2.Text = "0";
            ValorAct = Convert.ToDecimal(txtRemo2000Vo2.Text);
            iPercentilAct = GetPercentilCardioresp(Convert.ToInt32(Sexo_), Idade_, ValorAct);

            sRes = GetResultadocardio(iPercentilAct);
            iPerc = iPercentilAct;
            iValue = ValorAct;
        }

        private void DoGraficoActualRemo2000()
        {
            int iPercentilAct;
            decimal ValorAct = 0;

            if (txtRemo2000Vo2.Text == string.Empty) return;
            ValorAct = Convert.ToDecimal(txtRemo2000Vo2.Text);

            iPercentilAct = GetPercentilCardioresp(Convert.ToInt32(Sexo_), Idade_, ValorAct);
            CriaGraficoRemo2000Actual(iPercentilAct);

            //Descrição do Resultado Actual
            lblResActualRemo2000.Text = GetResultadocardio(iPercentilAct);

        }


        private void DoGraficoAnteriorRemo2000()
        {
            int iPercentilAnt = 0;
            decimal ValorAnt = 0;

            ValorAnt = GetValorAnteriorRemo2000();

            if (ValorAnt == 0) return;

            iPercentilAnt = GetPercentilCardioresp(Convert.ToInt32(Sexo_), Idade_, ValorAnt);
            CriaGraficoremo2000Anterior(iPercentilAnt);

            //Descrição do Resultado Anterior
            lblResAnteriorRemo2000.Text = GetResultadocardio(iPercentilAnt);
        }


        private void CriaGraficoRemo2000Actual(int iPercentil)
        {
            //int iPercentil = GetPercentil(1,25,Convert.ToInt32(textBox1.Text));
            int SizeX = (iPercentil * 500) / 100;

            LabelGradient.LabelGradient labelGradient1;
            labelGradient1 = new LabelGradient.LabelGradient();

            labelGradient1.BorderStyle = System.Windows.Forms.Border3DStyle.Adjust;
            labelGradient1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            //labelGradient1.Font = new System.Drawing.Font("Arial Black", 13.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            labelGradient1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            labelGradient1.GradientColorTwo = System.Drawing.Color.White;
            labelGradient1.GradientColorOne = System.Drawing.Color.FromArgb(27, 78, 169);
            labelGradient1.Location = new System.Drawing.Point(0, 0);
            labelGradient1.ForeColor = System.Drawing.Color.White;
            labelGradient1.Name = "labelGradient1";
            labelGradient1.Size = new System.Drawing.Size(SizeX, 24);
            labelGradient1.TabIndex = 0;
            labelGradient1.Text = Convert.ToString(iPercentil) + "%";
            labelGradient1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            if (pnlGraficoRemo2000Act.Controls.Count > 0)
                pnlGraficoRemo2000Act.Controls.RemoveAt(0);
            pnlGraficoRemo2000Act.Controls.Add(labelGradient1);
            pnlGraficoRemo2000Act.Refresh();
        }


        private void CriaGraficoremo2000Anterior(int iPercentil)
        {
            int SizeX = (iPercentil * 500) / 100;

            LabelGradient.LabelGradient labelGradient1;
            labelGradient1 = new LabelGradient.LabelGradient();

            labelGradient1.BorderStyle = System.Windows.Forms.Border3DStyle.Adjust;
            labelGradient1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            //labelGradient1.Font = new System.Drawing.Font("Arial Black", 13.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            labelGradient1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            labelGradient1.GradientColorTwo = System.Drawing.Color.White;
            labelGradient1.GradientColorOne = System.Drawing.Color.Red;  //System.Drawing.Color.FromArgb(27,78,169);
            labelGradient1.Location = new System.Drawing.Point(0, 0);
            labelGradient1.ForeColor = System.Drawing.Color.White;
            labelGradient1.Name = "labelGradient1";
            labelGradient1.Size = new System.Drawing.Size(SizeX, 24);
            labelGradient1.TabIndex = 0;
            labelGradient1.Text = Convert.ToString(iPercentil) + "%";
            labelGradient1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            if (pnlGraficoRemo2000Ant.Controls.Count > 0)
                pnlGraficoRemo2000Ant.Controls.RemoveAt(0);
            pnlGraficoRemo2000Ant.Controls.Add(labelGradient1);
            pnlGraficoRemo2000Ant.Refresh();
        }


        private decimal GetValorAnteriorRemo2000()
        {
            try
            {
                string strSQL;
                string strConn;

                OleDbDataReader oDataReader;
                OleDbDataAdapter DBcommand;

                strConn = General.getConnectionString();
                OleDbConnection myConnection = new OleDbConnection(strConn);

                strSQL = "SELECT TOP 1 Vo2Max FROM tbl_RespAptidaoCardio ";
                strSQL += " WHERE IDSocio=" + Atleta_ + " AND CDate(Data) < #" + General.GetFormatData(Data_) + "# AND IDTipoTesteCardio= " + cmbTipoTeste.SelectedValue.ToString() + " ORDER BY CDate(Data) DESC";

                General.TrataConnection(myConnection);

                DBcommand = new OleDbDataAdapter();
                DBcommand.SelectCommand = new OleDbCommand();
                DBcommand.SelectCommand.Connection = myConnection;
                DBcommand.SelectCommand.CommandText = strSQL;
                DBcommand.SelectCommand.CommandType = CommandType.Text;
                oDataReader = DBcommand.SelectCommand.ExecuteReader();

                while (oDataReader.Read())
                {
                    return Convert.ToDecimal(oDataReader["Vo2Max"]);
                }

                return 0;

            }
            catch
            {
                return 0;
            }
        }









    }
}