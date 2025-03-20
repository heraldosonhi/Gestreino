using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Gestreino.Classes;
using Gestreino.Models;
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

    public class AdministrationController : Controller
    {
        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();
        ExportEmail Mailer = new ExportEmail();
        // _MenuLeftBarLink
        int _MenuLeftBarLink_User = 101;
        int _MenuLeftBarLink_Access = 102;
        int _MenuLeftBarLink_LoginLogs = 103;
        int _MenuLeftBarLink_Parameters = 104;
        int _MenuLeftBarLink_Settings = 105;
        int _MenuLeftBarLink_Tokens = 106;
        public ActionResult Index()
        {
            return View();
        }

        //Users
        public ActionResult Users()
        {
            if (!AcessControl.Authorized(AcessControl.ADM_USERS_USERS_LIST_VIEW_SEARCH)) return View("Lockout");
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_User;
            return View("Users/Index");
        }
        [HttpGet]
        public ActionResult ViewUsers(int? Id)
        {
            if (!AcessControl.Authorized(AcessControl.ADM_USERS_USERS_LIST_VIEW_SEARCH)) return View("Lockout");
            if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }
            var data = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(Id, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList();
            if (!data.Any()) { return RedirectToAction("", "home"); }
            ViewBag.imgSrc = (string.IsNullOrEmpty(data.First().FOTOGRAFIA)) ? "/Assets/images/user-avatar.jpg" : "/" + data.First().FOTOGRAFIA;

            ViewBag.data = data;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_User;
            return View("Users/ViewUsers");
        }
        [HttpPost]
        public ActionResult GetUser(int? GroupId, int? ProfileId)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Login = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Grupos = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Perfis = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Estado = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[8][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            GroupId = GroupId == null ? null : GroupId;
            ProfileId = ProfileId == null ? null : ProfileId;
            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(null, GroupId, ProfileId, null, null, null, null, null, null, null, null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Login)) v = v.Where(a => a.LOGIN != null && a.LOGIN.ToUpper().Contains(Login.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
            if (!string.IsNullOrEmpty(Grupos)) v = v.Where(a => a.TOTALGROUPS != null && a.TOTALGROUPS.ToString() == Grupos);
            if (!string.IsNullOrEmpty(Perfis)) v = v.Where(a => a.TOTALPERFIS != null && a.TOTALPERFIS.ToString() == Perfis);
            if (!string.IsNullOrEmpty(Estado)) v = v.Where(a => a.ACTIVO != null && a.ACTIVO == (Estado == "1" ? "Activo" : "Inactivo"));
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
                        case "LOGIN": v = v.OrderBy(s => s.LOGIN); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
                        case "GRUPOS": v = v.OrderBy(s => s.TOTALGROUPS); break;
                        case "PERFIS": v = v.OrderBy(s => s.TOTALPERFIS); break;
                        case "ESTADO": v = v.OrderBy(s => s.ACTIVO); break;
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
                        case "LOGIN": v = v.OrderByDescending(s => s.LOGIN); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
                        case "GRUPOS": v = v.OrderByDescending(s => s.TOTALGROUPS); break;
                        case "PERFIS": v = v.OrderByDescending(s => s.TOTALPERFIS); break;
                        case "ESTADO": v = v.OrderByDescending(s => s.ACTIVO); break;
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
                    Id = x.ID,
                    LOGIN = x.LOGIN,
                    NOME = x.NOME_PROPIO + " " + x.APELIDO,
                    GRUPOS = x.TOTALGROUPS,
                    PERFIS = x.TOTALPERFIS,
                    ESTADO = x.ACTIVO,
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
        public ActionResult NewUser(Gestreino.Models.Users MODEL)
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
                    return Json(new { result = false, error = "Data Inicial deve ser inferior a Data Final!" });
                }
                if (databaseManager.UTILIZADORES.Where(x => x.LOGIN == MODEL.Login).Any())
                    return Json(new { result = false, error = "Utilizador já se encontra registado, por favor verifique a seleção!" });

                if (databaseManager.PES_CONTACTOS.Where(x => x.EMAIL == MODEL.Email).Any())
                    return Json(new { result = false, error = "Endereço de email já se encontra registado, por favor verifique a seleção!" });


                var Status = MODEL.Status == 1 ? true : false;
                var DateIni = string.IsNullOrWhiteSpace(MODEL.DateAct) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateAct, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DateEnd = string.IsNullOrWhiteSpace(MODEL.DateDisact) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateDisact, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                // Create Salted Password
                var Salt = Crypto.GenerateSalt(64);
                var Password = Crypto.Hash(MODEL.Password.Trim() + Salt);
                // Remove whitespaces and parse datetime strings //TrimStart() //Trim()

                // Create
                var create = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(null, null, null, MODEL.Login, MODEL.Name, Convert.ToDecimal(MODEL.Phone), MODEL.Email.Trim(), Password, Salt, Status, DateIni, DateEnd, true, int.Parse(User.Identity.GetUserId()), "C").ToList();

                // Send Email
                string url = "http://gestreino.pt/";
                Mailer.SendEmailMVC(1, MODEL.Email, MODEL.Name, MODEL.Login, MODEL.Password.Trim(), url, null); // Email template - 3

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(Gestreino.Models.Users MODEL)
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

                if (databaseManager.UTILIZADORES.Where(x => x.LOGIN == MODEL.Login && x.ID != MODEL.Id).Any())
                    return Json(new { result = false, error = "Utilizador já se encontra registado, por favor verifique a seleção!" });

                var Status = MODEL.Status == 1 ? true : false;

                if (databaseManager.PES_CONTACTOS.Where(a => a.EMAIL == MODEL.Email && a.PES_PESSOAS_ID != MODEL.PesId).ToList().Count() > 0)
                {
                    if (!string.IsNullOrEmpty(MODEL.Email)) return Json(new { result = false, error = "Este endereço de email já encontra-se em uso!" });

                }
                // Update
                var update = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(MODEL.Id, null, null, MODEL.Login, null, Convert.ToDecimal(MODEL.Phone), MODEL.Email.Trim(), null, null, Status, null, null, true, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CleanUserLogs(Gestreino.Models.Users MODEL)
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

                var _cartid = (from c in databaseManager.UTILIZADORES_LOGIN_PASSWORD_TENT
                               where c.UTILIZADORES_ID == MODEL.Id //&& c.DATA.ToString("dd/MM/yyyy") == DateTime.Today.ToString("dd/MM/yyyy")
                               select c);
                databaseManager.UTILIZADORES_LOGIN_PASSWORD_TENT.RemoveRange(_cartid);
                databaseManager.SaveChanges();

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(Gestreino.Models.Users MODEL)
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

                // Create Salted Password
                var Salt = Crypto.GenerateSalt(64);
                var Password = Crypto.Hash(MODEL.Password.Trim() + Salt);
                // Remove whitespaces and parse datetime strings //TrimStart() //Trim()
                var update = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(MODEL.Id, null, null, null, null, null, null, Password, Salt, null, null, null, null, int.Parse(User.Identity.GetUserId()), Convert.ToChar('P').ToString()).ToArray();

                // Send Email
                string url = "http://gestreino.pt/";
                Mailer.SendEmailMVC(2, MODEL.Email, MODEL.Login, MODEL.Password.Trim(), url, null, null); // Email template - 3

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Access
        [HttpGet]
        public ActionResult Access()
        {
            if (AcessControl.Authorized(AcessControl.ADM_USERS_ATOMS_LIST_VIEW_SEARCH) || AcessControl.Authorized(AcessControl.ADM_USERS_PROFILES_LIST_VIEW_SEARCH) || AcessControl.Authorized(AcessControl.ADM_USERS_GROUPS_LIST_VIEW_SEARCH)) { }
            else return View("Lockout");

            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Access;
            return View("Access/Index");
        }

        //Groups
        [HttpGet]
        public ActionResult ViewGroups(int? Id)
        {
            if (!AcessControl.Authorized(AcessControl.ADM_USERS_GROUPS_LIST_VIEW_SEARCH)) return View("Lockout");

            if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }
            var data = databaseManager.SP_UTILIZADORES_ENT_GRUPOS(Id, null, null, null, null, "R").ToList();
            if (!data.Any()) { return RedirectToAction("", "home"); }
            ViewBag.data = data;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Access;
            return View("Access/ViewGroups");
        }
        [HttpPost]
        public ActionResult GetGroups()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_GRUPOS(null, null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                    AccessControlEdit = !AcessControl.Authorized(AcessControl.ADM_USERS_GROUPS_EDIT) ? "none" : "",
                    AccessControlDelete = !AcessControl.Authorized(AcessControl.ADM_USERS_GROUPS_DELETE) ? "none" : "",
                    Id = x.ID,
                    SIGLA = x.SIGLA,
                    NOME = x.NOME,
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
        public ActionResult NewGroup(UTILIZADORES_ACESSO_GRUPOS MODEL)
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

                if (databaseManager.UTILIZADORES_ACESSO_GRUPOS.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_UTILIZADORES_ENT_GRUPOS(null, MODEL.SIGLA, MODEL.NOME, MODEL.DESCRICAO, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GroupTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGroup(UTILIZADORES_ACESSO_GRUPOS MODEL)
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
                if (databaseManager.UTILIZADORES_ACESSO_GRUPOS.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_UTILIZADORES_ENT_GRUPOS(MODEL.ID, MODEL.SIGLA, MODEL.NOME, MODEL.DESCRICAO, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GroupTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGroup(int?[] ids)
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
                    databaseManager.SP_UTILIZADORES_ENT_GRUPOS(i, null, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GroupTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Profiles
        [HttpGet]
        public ActionResult ViewProfiles(int? Id)
        {
            if (!AcessControl.Authorized(AcessControl.ADM_USERS_PROFILES_LIST_VIEW_SEARCH)) return View("Lockout");

            if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }
            var data = databaseManager.SP_UTILIZADORES_ENT_PERFIS(Id, null, null, null, "R").ToList();
            if (!data.Any()) { return RedirectToAction("", "home"); }
            ViewBag.data = data;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Access;
            return View("Access/ViewProfiles");
        }
        [HttpPost]
        public ActionResult GetProfiles()
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
            var Desc = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_PERFIS(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
            if (!string.IsNullOrEmpty(Desc)) v = v.Where(a => a.DESCRICAO != null && a.DESCRICAO.ToUpper().Contains(Desc.ToUpper()));
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
                    AccessControlEdit = !AcessControl.Authorized(AcessControl.ADM_USERS_PROFILES_EDIT) ? "none" : "",
                    AccessControlDelete = !AcessControl.Authorized(AcessControl.ADM_USERS_PROFILES_DELETE) ? "none" : "",
                    Id = x.ID,
                    NOME = x.NOME,
                    DESCRICAO = x.DESCRICAO,
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
        public ActionResult NewProfile(UTILIZADORES_ACESSO_PERFIS MODEL)
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
                if (databaseManager.UTILIZADORES_ACESSO_PERFIS.Where(x => x.NOME == MODEL.NOME || x.DESCRICAO == MODEL.DESCRICAO).Any())
                    return Json(new { result = false, error = "Nome e/ou Descrição desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_UTILIZADORES_ENT_PERFIS(null, MODEL.NOME, MODEL.DESCRICAO, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "ProfileTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(UTILIZADORES_ACESSO_PERFIS MODEL)
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
                if (databaseManager.UTILIZADORES_ACESSO_PERFIS.Where(x => x.NOME == MODEL.NOME && x.ID != MODEL.ID || x.DESCRICAO == MODEL.DESCRICAO && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Nome e/ou Descrição desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_UTILIZADORES_ENT_PERFIS(MODEL.ID, MODEL.NOME, MODEL.DESCRICAO, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "ProfileTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProfile(int?[] ids)
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
                    databaseManager.SP_UTILIZADORES_ENT_PERFIS(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "ProfileTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Atoms
        [HttpGet]
        public ActionResult ViewAtoms(int? Id)
        {
            if (!AcessControl.Authorized(AcessControl.ADM_USERS_ATOMS_LIST_VIEW_SEARCH)) return View("Lockout");

            if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }
            var data = databaseManager.SP_UTILIZADORES_ENT_ATOMOS(Id, null, null, null, "R").ToList();
            if (!data.Any()) { return RedirectToAction("", "home"); }
            ViewBag.data = data;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Access;
            return View("Access/ViewAtoms");
        }
        [HttpPost]
        public ActionResult GetAtoms()
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
            var Desc = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_ATOMOS(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
            if (!string.IsNullOrEmpty(Desc)) v = v.Where(a => a.DESCRICAO != null && a.DESCRICAO.ToUpper().Contains(Desc.ToUpper()));
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
                    AccessControlEdit = !AcessControl.Authorized(AcessControl.ADM_USERS_ATOMS_EDIT) ? "none" : "",
                    AccessControlDelete = !AcessControl.Authorized(AcessControl.ADM_USERS_ATOMS_DELETE) ? "none" : "",
                    Id = x.ID,
                    NOME = x.NOME,
                    DESCRICAO = x.DESCRICAO,
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
        public ActionResult NewAtom(UTILIZADORES_ACESSO_ATOMOS MODEL)
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
                if (databaseManager.UTILIZADORES_ACESSO_ATOMOS.Where(x => x.NOME == MODEL.NOME || x.DESCRICAO == MODEL.DESCRICAO).Any())
                    return Json(new { result = false, error = "Nome e/ou Descrição desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_UTILIZADORES_ENT_ATOMOS(null, MODEL.NOME, MODEL.DESCRICAO, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "AtomTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAtom(UTILIZADORES_ACESSO_ATOMOS MODEL)
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
                if (databaseManager.UTILIZADORES_ACESSO_ATOMOS.Where(x => x.NOME == MODEL.NOME && x.ID != MODEL.ID || x.DESCRICAO == MODEL.DESCRICAO && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Nome e/ou Descrição desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_UTILIZADORES_ENT_ATOMOS(MODEL.ID, MODEL.NOME, MODEL.DESCRICAO, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "AtomTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAtom(int?[] ids)
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
                    databaseManager.SP_UTILIZADORES_ENT_ATOMOS(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "AtomTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Users/Groups/Atoms/Profiles 
        [HttpPost]
        public ActionResult GetUserGroup(int? GroupId, int? UserId)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Grupo = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var User = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            GroupId = GroupId == null ? null : GroupId;
            UserId = UserId == null ? null : UserId;
            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_GRUPOS_UTILIZADORES(null, GroupId, UserId, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Grupo)) v = v.Where(a => a.GRUPO_NOME != null && a.GRUPO_NOME.ToUpper().Contains(Grupo.ToUpper()));
            if (!string.IsNullOrEmpty(User)) v = v.Where(a => a.LOGIN != null && a.LOGIN.ToUpper().Contains(User.ToUpper()));
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
                        case "GRUPO": v = v.OrderBy(s => s.GRUPO_NOME); break;
                        case "UTILIZADOR": v = v.OrderBy(s => s.LOGIN); break;
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
                        case "GRUPO": v = v.OrderByDescending(s => s.GRUPO_NOME); break;
                        case "UTILIZADOR": v = v.OrderByDescending(s => s.LOGIN); break;
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
                    AccessControlDelete = !AcessControl.Authorized(AcessControl.ADM_USERS_GROUP_USERS_DELETE) ? "none" : "",
                    Id = x.ID,
                    GRUPO = x.GRUPO_NOME + " (" + x.SIGLA + ")",
                    UTILIZADOR = x.NOME_PROPIO + " " + x.APELIDO + " (" + x.LOGIN + ")",
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
        public ActionResult NewUserGroup(AccessAppendItems MODEL, int?[] items)
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
                if(items ==null)
                    return Json(new { result = false, error = "Deve selecionar um item para o registo!" });

                // Validate
                foreach (var i in items)
                {
                    if (MODEL.UserId == null)
                    {
                        if (databaseManager.UTILIZADORES_ACESSO_UTIL_GRUPOS.Where(x => x.UTILIZADORES_ACESSO_GRUPOS_ID == MODEL.GroupId && x.UTILIZADORES_ID == i).Any())
                            return Json(new { result = false, error = "Relação já se encontra associada, por favor verifique a seleção!" });
                    }
                    if (MODEL.GroupId == null)
                    {
                        if (databaseManager.UTILIZADORES_ACESSO_UTIL_GRUPOS.Where(x => x.UTILIZADORES_ACESSO_GRUPOS_ID == i && x.UTILIZADORES_ID == MODEL.UserId).Any())
                            return Json(new { result = false, error = "Relação já se encontra associada, por favor verifique a seleção!" });
                    }
                }

                // Create
                foreach (var i in items)
                {
                    if (MODEL.UserId == null)
                        databaseManager.SP_UTILIZADORES_ENT_GRUPOS_UTILIZADORES(null, MODEL.GroupId, i, int.Parse(User.Identity.GetUserId()), "C").ToList();
                    if (MODEL.GroupId == null)
                        databaseManager.SP_UTILIZADORES_ENT_GRUPOS_UTILIZADORES(null, i, MODEL.UserId, int.Parse(User.Identity.GetUserId()), "C").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserGroupTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserGroup(int?[] ids)
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
                    databaseManager.SP_UTILIZADORES_ENT_GRUPOS_UTILIZADORES(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserGroupTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        [HttpPost]
        public ActionResult GetAtomGroup(int? GroupId, int? AtomId)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Grupo = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Atom = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            GroupId = GroupId == null ? null : GroupId;
            AtomId = AtomId == null ? null : AtomId;
            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_ATOMOS_GRUPOS(null, GroupId, AtomId, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Grupo)) v = v.Where(a => a.GRUPO_NOME != null && a.GRUPO_NOME.ToUpper().Contains(Grupo.ToUpper()));
            if (!string.IsNullOrEmpty(Atom)) v = v.Where(a => a.ATOMO_NOME != null && a.ATOMO_NOME.ToUpper().Contains(Atom.ToUpper()));
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
                        case "GRUPO": v = v.OrderBy(s => s.GRUPO_NOME); break;
                        case "ATOMO": v = v.OrderBy(s => s.ATOMO_NOME); break;
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
                        case "GRUPO": v = v.OrderByDescending(s => s.GRUPO_NOME); break;
                        case "ATOMO": v = v.OrderByDescending(s => s.ATOMO_NOME); break;
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
                    AccessControlDelete = !AcessControl.Authorized(AcessControl.ADM_USERS_ATOMS_GROUPS_DELETE) ? "none" : "",
                    Id = x.ID,
                    GRUPO = x.GRUPO_NOME + " (" + x.GRUPO_SIGLA + ")",
                    ATOMO = x.ATOMO_NOME,
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
        public ActionResult NewAtomGroup(AccessAppendItems MODEL, int?[] items)
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
                // Validate
                foreach (var i in items)
                {
                    if (MODEL.AtomId == null)
                    {
                        if (databaseManager.UTILIZADORES_ACESSO_ATOMOS_GRUPOS.Where(x => x.UTILIZADORES_ACESSO_GRUPOS_ID == MODEL.GroupId && x.UTILIZADORES_ACESSO_ATOMOS_ID == i).Any())
                            return Json(new { result = false, error = "Relação já se encontra associada, por favor verifique a seleção!" });
                    }
                    if (MODEL.GroupId == null)
                    {
                        if (databaseManager.UTILIZADORES_ACESSO_ATOMOS_GRUPOS.Where(x => x.UTILIZADORES_ACESSO_GRUPOS_ID == i && x.UTILIZADORES_ACESSO_ATOMOS_ID == MODEL.AtomId).Any())
                            return Json(new { result = false, error = "Relação já se encontra associada, por favor verifique a seleção!" });
                    }
                }
                // Create
                foreach (var i in items)
                {
                    if (MODEL.AtomId == null)
                        databaseManager.SP_UTILIZADORES_ENT_ATOMOS_GRUPOS(null, MODEL.GroupId, i, int.Parse(User.Identity.GetUserId()), "C").ToList();
                    if (MODEL.GroupId == null)
                        databaseManager.SP_UTILIZADORES_ENT_ATOMOS_GRUPOS(null, i, MODEL.AtomId, int.Parse(User.Identity.GetUserId()), "C").ToList();

                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "AtomGroupTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAtomGroup(int?[] ids)
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
                    databaseManager.SP_UTILIZADORES_ENT_ATOMOS_GRUPOS(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "AtomGroupTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        [HttpPost]
        public ActionResult GetProfileAtom(int? ProfileId, int? AtomId)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Perfil = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Atom = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            ProfileId = ProfileId == null ? null : ProfileId;
            AtomId = AtomId == null ? null : AtomId;
            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_PERFIS_ATOMOS(null, ProfileId, AtomId, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Perfil)) v = v.Where(a => a.PERFIL_NOME != null && a.PERFIL_NOME.ToUpper().Contains(Perfil.ToUpper()));
            if (!string.IsNullOrEmpty(Atom)) v = v.Where(a => a.ATOMO_NOME != null && a.ATOMO_NOME.ToUpper().Contains(Atom.ToUpper()));
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
                        case "PERFIL": v = v.OrderBy(s => s.PERFIL_NOME); break;
                        case "ATOMO": v = v.OrderBy(s => s.ATOMO_NOME); break;
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
                        case "PERFIL": v = v.OrderByDescending(s => s.PERFIL_NOME); break;
                        case "ATOMO": v = v.OrderByDescending(s => s.ATOMO_NOME); break;
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
                    AccessControlDelete = !AcessControl.Authorized(AcessControl.ADM_USERS_ATOMS_PROFILES_DELETE) ? "none" : "",
                    Id = x.ID,
                    PERFIL = x.PERFIL_NOME,
                    ATOMO = x.ATOMO_NOME,
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
        public ActionResult NewAtomProfile(AccessAppendItems MODEL, int?[] items)
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
                // Validate
                foreach (var i in items)
                {
                    if (MODEL.AtomId == null)
                    {
                        if (databaseManager.UTILIZADORES_ACESSO_PERFIS_ATOMOS.Where(x => x.UTILIZADORES_ACESSO_PERFIS_ID == MODEL.ProfileId && x.UTILIZADORES_ACESSO_ATOMOS_ID == i).Any())
                            return Json(new { result = false, error = "Relação já se encontra associada, por favor verifique a seleção!" });
                    }
                    if (MODEL.ProfileId == null)
                    {
                        if (databaseManager.UTILIZADORES_ACESSO_PERFIS_ATOMOS.Where(x => x.UTILIZADORES_ACESSO_PERFIS_ID == i && x.UTILIZADORES_ACESSO_ATOMOS_ID == MODEL.AtomId).Any())
                            return Json(new { result = false, error = "Relação já se encontra associada, por favor verifique a seleção!" });
                    }
                }
                // Create
                foreach (var i in items)
                {
                    if (MODEL.AtomId == null)
                        databaseManager.SP_UTILIZADORES_ENT_PERFIS_ATOMOS(null, MODEL.ProfileId, i, int.Parse(User.Identity.GetUserId()), "C").ToList();
                    if (MODEL.ProfileId == null)
                        databaseManager.SP_UTILIZADORES_ENT_PERFIS_ATOMOS(null, i, MODEL.AtomId, int.Parse(User.Identity.GetUserId()), "C").ToList();

                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "ProfileAtomTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAtomProfile(int?[] ids)
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
                    databaseManager.SP_UTILIZADORES_ENT_PERFIS_ATOMOS(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "ProfileAtomTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        [HttpPost]
        public ActionResult GetProfileUtil(int? ProfileId, int? UserId)
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
            var Perfil = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            ProfileId = ProfileId == null ? null : ProfileId;
            UserId = UserId == null ? null : UserId;
            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES_PERFIS(null, ProfileId, UserId, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Perfil)) v = v.Where(a => a.PERFIL_NOME != null && a.PERFIL_NOME.ToUpper().Contains(Perfil.ToUpper()));
            if (!string.IsNullOrEmpty(User)) v = v.Where(a => a.LOGIN != null && a.LOGIN.ToUpper().Contains(User.ToUpper()));
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
                        case "UTILIZADOR": v = v.OrderBy(s => s.LOGIN); break;
                        case "PERFIL": v = v.OrderBy(s => s.PERFIL_NOME); break;
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
                        case "UTILIZADOR": v = v.OrderByDescending(s => s.LOGIN); break;
                        case "PERFIL": v = v.OrderByDescending(s => s.PERFIL_NOME); break;
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
                    AccessControlDelete = !AcessControl.Authorized(AcessControl.ADM_USERS_PROFILE_USERS_DELETE) ? "none" : "",
                    Id = x.ID,
                    PERFIL = x.PERFIL_NOME,
                    UTILIZADOR = x.NOME_PROPIO + " " + x.APELIDO + " (" + x.LOGIN + ")",
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
        public ActionResult NewProfileUtil(AccessAppendItems MODEL, int?[] items)
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
                // Validate
                foreach (var i in items)
                {
                    if (MODEL.UserId == null)
                    {
                        if (databaseManager.UTILIZADORES_ACESSO_UTIL_PERFIS.Where(x => x.UTILIZADORES_ACESSO_PERFIS_ID == MODEL.ProfileId && x.UTILIZADORES_ID == i).Any())
                            return Json(new { result = false, error = "Relação já se encontra associada, por favor verifique a seleção!" });
                    }
                    if (MODEL.ProfileId == null)
                    {
                        if (databaseManager.UTILIZADORES_ACESSO_UTIL_PERFIS.Where(x => x.UTILIZADORES_ACESSO_PERFIS_ID == i && x.UTILIZADORES_ID == MODEL.UserId).Any())
                            return Json(new { result = false, error = "Relação já se encontra associada, por favor verifique a seleção!" });
                    }
                }
                // Create
                foreach (var i in items)
                {
                    if (MODEL.UserId == null)
                        databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES_PERFIS(null, MODEL.ProfileId, i, int.Parse(User.Identity.GetUserId()), "C").ToList();
                    if (MODEL.ProfileId == null)
                        databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES_PERFIS(null, i, MODEL.UserId, int.Parse(User.Identity.GetUserId()), "C").ToList();

                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.ToString() });
            }
            return Json(new { result = true, error = string.Empty, table = "ProfileUtilTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUtilProfile(int?[] ids)
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
                    databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES_PERFIS(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "ProfileUtilTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        [HttpPost]
        public ActionResult GetUserAtoms(int? UserId)
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
            var Insercao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            UserId = UserId == null ? null : UserId;
            var atoms = databaseManager.SP_UTILIZADORES_LOGIN(UserId, null, null, null, null, null, null, null, null, "A").Select(x => x).ToList();

            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_ATOMOS(null, null, null, null, "R").
                     //FIX
                     Where(x => atoms.Contains(x.ID)).ToList()
                     select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                    Id = x.ID,
                    NOME = x.NOME,
                    INSERCAO = x.INSERCAO,
                    DATAINSERCAO = x.DATA_INSERCAO,
                    ACTUALIZACAO = x.ACTUALIZACAO,
                    DATAACTUALIZACAO = x.DATA_ACTUALIZACAO
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }

        //UserLogs
        public ActionResult LoginLogs()
        {
            if (!AcessControl.Authorized(AcessControl.ADM_USERS_LOGIN_LOGS_LIST_VIEW_SEARCH)) return View("Lockout");

            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_LoginLogs;
            return View("Access/LoginLogs");
        }
        [HttpPost]
        public ActionResult GetUserLogs(int? UserId)
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Login = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var IP = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Mac = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Host = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Device = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var URL = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var Data = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            UserId = UserId == null ? null : UserId;
            var v = (from a in databaseManager.SP_UTILIZADORES_LOGIN_LOGS(UserId, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Login)) v = v.Where(a => a.LOGIN != null && a.LOGIN.ToUpper().Contains(Login.ToUpper()));
            if (!string.IsNullOrEmpty(IP)) v = v.Where(a => a.ENDERECO_IP != null && a.ENDERECO_IP.ToUpper().Contains(IP.ToUpper()));
            if (!string.IsNullOrEmpty(Mac)) v = v.Where(a => a.ENDERECO_MAC != null && a.ENDERECO_MAC.ToUpper().Contains(Mac.ToUpper()));
            if (!string.IsNullOrEmpty(Host)) v = v.Where(a => a.HOSPEDEIRO_HOST != null && a.HOSPEDEIRO_HOST.ToUpper().Contains(Host.ToUpper()));
            if (!string.IsNullOrEmpty(Device)) v = v.Where(a => a.DISPOSITIVO != null && a.DISPOSITIVO.ToUpper().Contains(Device.ToUpper()));
            if (!string.IsNullOrEmpty(URL)) v = v.Where(a => a.URL != null && a.URL.ToUpper().Contains(URL.ToUpper()));
            if (!string.IsNullOrEmpty(Data)) v = v.Where(a => a.DATA != null && a.DATA.ToUpper().Contains(Data.Replace("-", "/").ToUpper())); // Simply replace no need for DateTime Parse


            //ORDER RESULT SET
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                if (sortColumnDir == "asc")
                {
                    switch (sortColumn)
                    {
                        case "LOGIN": v = v.OrderBy(s => s.LOGIN); break;
                        case "IP": v = v.OrderBy(s => s.ENDERECO_IP); break;
                        case "MAC": v = v.OrderBy(s => s.ENDERECO_MAC); break;
                        case "HOST": v = v.OrderBy(s => s.HOSPEDEIRO_HOST); break;
                        case "DEVICE": v = v.OrderBy(s => s.DISPOSITIVO); break;
                        case "URL": v = v.OrderBy(s => s.URL); break;
                        case "DATA": v = v.OrderBy(s => s.DATA); break;
                    }
                }
                else
                {
                    switch (sortColumn)
                    {
                        case "LOGIN": v = v.OrderByDescending(s => s.LOGIN); break;
                        case "IP": v = v.OrderByDescending(s => s.ENDERECO_IP); break;
                        case "MAC": v = v.OrderByDescending(s => s.ENDERECO_MAC); break;
                        case "HOST": v = v.OrderByDescending(s => s.HOSPEDEIRO_HOST); break;
                        case "DEVICE": v = v.OrderByDescending(s => s.DISPOSITIVO); break;
                        case "URL": v = v.OrderByDescending(s => s.URL); break;
                        case "DATA": v = v.OrderByDescending(s => s.DATA); break;
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
                    Id = x.ID,
                    LOGIN = x.LOGIN,
                    IP = x.ENDERECO_IP,
                    MAC = x.ENDERECO_MAC,
                    HOST = x.HOSPEDEIRO_HOST,
                    DEVICE = x.DISPOSITIVO,
                    URL = x.URL,
                    DATA = x.DATA
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }

        //Tokens
        public ActionResult Tokens()
        {
            if (!AcessControl.Authorized(AcessControl.ADM_SEC_TOKENS_LIST_VIEW_SEARCH)) return View("Lockout");

            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Tokens;
            return View("Access/Tokens");
        }
        [HttpPost]
        public ActionResult GetTokenTable()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Tipo = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Token = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Conteudo = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Data = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GRL_ENT_TOKENS(null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Tipo)) v = v.Where(a => a.TOKEN_TIPO != null && a.TOKEN_TIPO.ToUpper().Contains(Tipo.ToUpper()));
            if (!string.IsNullOrEmpty(Token)) v = v.Where(a => a.TOKEN != null && a.TOKEN.ToUpper().Contains(Token.ToUpper()));
            if (!string.IsNullOrEmpty(Conteudo)) v = v.Where(a => a.CONTEUDO != null && a.CONTEUDO.ToUpper().Contains(Conteudo.ToUpper()));
            if (!string.IsNullOrEmpty(Data)) v = v.Where(a => a.DATA != null && a.DATA.ToUpper().Contains(Data.Replace("-", "/").ToUpper()));
            if (!string.IsNullOrEmpty(Insercao)) v = v.Where(a => a.DATA_INSERCAO != null && a.DATA_INSERCAO.ToUpper().Contains(Insercao.Replace("-", "/").ToUpper()));

            //ORDER RESULT SET
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                if (sortColumnDir == "asc")
                {
                    switch (sortColumn)
                    {
                        case "TIPO": v = v.OrderBy(s => s.TOKEN_TIPO); break;
                        case "TOKEN": v = v.OrderBy(s => s.TOKEN); break;
                        case "CONTEUDO": v = v.OrderBy(s => s.CONTEUDO); break;
                        case "DATA": v = v.OrderBy(s => s.DATA); break;
                        case "INSERCAO": v = v.OrderBy(s => s.DATA_INSERCAO); break;
                    }
                }
                else
                {
                    switch (sortColumn)
                    {
                        case "TIPO": v = v.OrderByDescending(s => s.TOKEN_TIPO); break;
                        case "TOKEN": v = v.OrderByDescending(s => s.TOKEN); break;
                        case "CONTEUDO": v = v.OrderByDescending(s => s.CONTEUDO); break;
                        case "DATA": v = v.OrderByDescending(s => s.DATA); break;
                        case "INSERCAO": v = v.OrderByDescending(s => s.DATA_INSERCAO); break;
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
                    Id = x.ID,
                    TIPO = x.TOKEN_TIPO,
                    TOKEN = x.TOKEN,
                    CONTEUDO = x.CONTEUDO,
                    DATA = x.DATA,
                    INSERCAO = x.DATA_INSERCAO
                }),
                sortColumn = sortColumn,
                sortColumnDir = sortColumnDir,
            }, JsonRequestBehavior.AllowGet);
        }


        //Parameters
        [HttpGet]
        public ActionResult Parameters()
        {
            if (AcessControl.Authorized(AcessControl.ADM_CONFIG_PARAM_ADM) || AcessControl.Authorized(AcessControl.ADM_CONFIG_PARAM_GT) || AcessControl.Authorized(AcessControl.ADM_CONFIG_PARAM_PES)) { }else return View("Lockout");

            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Parameters;
            return View("Parameters/Index");
        }
        [HttpGet]
        public ActionResult ParametersGRL()
        {
            if (!AcessControl.Authorized(AcessControl.ADM_CONFIG_PARAM_ADM)) return View("Lockout");

            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Parameters;
            return View("Parameters/ParametersGRL");
        }
        [HttpGet]
        public ActionResult ParametersPES()
        {
            if (!AcessControl.Authorized(AcessControl.ADM_CONFIG_PARAM_PES)) return View("Lockout");

            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Parameters;
            return View("Parameters/ParametersPES");
        }
        [HttpGet]
        public ActionResult ParametersGT(Gestreino.Models.GTExercicio MODEL)
        {
            if (!AcessControl.Authorized(AcessControl.ADM_CONFIG_PARAM_GT)) return View("Lockout");

            MODEL.TipoList = databaseManager.GT_TipoTreino.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Parameters;
            return View("Parameters/ParametersGT",MODEL);
        }


        //Exercicios
        public ActionResult ViewExercises(int? Id, Gestreino.Models.GTExercicio MODEL)
        {
            if (!AcessControl.Authorized(AcessControl.ADM_CONFIG_PARAM_GT)) return View("Lockout");
            
            if (Id == null || Id <= 0) { return RedirectToAction("", "home"); }

            var data = databaseManager.SP_GT_ENT_EXERCICIO(Id, null, null, null, null, null, Convert.ToChar('R').ToString()).ToList();
            if (!data.Any()) return RedirectToAction("", "home");
            MODEL.ID = Id;

            ViewBag.data = data;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Parameters;
            return View("Parameters/ViewExercise", MODEL);
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
                    Id = x.ID,
                    TREINO = x.tr_nome,
                    NOME = x.nome,
                    ALONGAMENTO = x.ALONGAMENTO,
                    SEQUENCIA = x.SEQUENCIA,
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
                var create = databaseManager.SP_GT_ENT_EXERCICIO(null, MODEL.TipoTreinoId, MODEL.Nome, MODEL.Alongamento, MODEL.Sequencia, int.Parse(User.Identity.GetUserId()), "C").ToList();
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



        //Parameters tipodoc
        [HttpPost]
        public ActionResult GetGRLDocTypes()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GRL_ENT_TIPODOC(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
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
        public ActionResult NewGRLDocTypes(GRL_ARQUIVOS_TIPO_DOCS MODEL)
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

                if (databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_GRL_ENT_TIPODOC(null, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoDocTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLDocTypes(GRL_ARQUIVOS_TIPO_DOCS MODEL)
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
                if (databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_GRL_ENT_TIPODOC(MODEL.ID, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoDocTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLDocTypes(int?[] ids)
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
                    databaseManager.SP_GRL_ENT_TIPODOC(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoDocTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters tipotoken
        [HttpPost]
        public ActionResult GetGRLTokenTypes()
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
            var Insercao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GRL_ENT_TIPOTOKEN(null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                    Id = x.ID,
                    NOME = x.NOME,
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
        public ActionResult NewGRLTokenTypes(GRL_TOKENS_TIPOS MODEL)
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

                if (databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_GRL_ENT_TIPOTOKEN(null, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoTokenTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLTokenTypes(GRL_TOKENS_TIPOS MODEL)
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
                if (databaseManager.GRL_ARQUIVOS_TIPO_DOCS.Where(x => x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_GRL_ENT_TIPOTOKEN(MODEL.ID, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoTokenTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLTokenTypes(int?[] ids)
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
                    databaseManager.SP_GRL_ENT_TIPOTOKEN(i, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoTokenTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters paises
        [HttpPost]
        public ActionResult GetGRLEndPais()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Codigo = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GRL_ENT_END_PAIS(null, null, null, null, null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
            if (!string.IsNullOrEmpty(Codigo)) v = v.Where(a => a.CODIGO != null && a.CODIGO.ToString() == Codigo);
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
                        case "CODIGO": v = v.OrderBy(s => s.CODIGO); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
                        case "CODIGO": v = v.OrderByDescending(s => s.CODIGO); break;
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
                      Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
                    CODIGO = x.CODIGO,
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
        public ActionResult NewGRLEndPais(GRL_ENDERECO_PAIS MODEL)
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

                if (databaseManager.GRL_ENDERECO_PAIS.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_GRL_ENT_END_PAIS(null, MODEL.SIGLA, MODEL.NOME, MODEL.CODIGO, MODEL.INDICATIVO, MODEL.NACIONALIDADE_MAS, MODEL.NACIONALIDADE_FEM, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndPaisTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLEndPais(GRL_ENDERECO_PAIS MODEL)
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
                if (databaseManager.GRL_ENDERECO_PAIS.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_GRL_ENT_END_PAIS(MODEL.ID, MODEL.SIGLA, MODEL.NOME, MODEL.CODIGO, MODEL.INDICATIVO, MODEL.NACIONALIDADE_MAS, MODEL.NACIONALIDADE_FEM, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndPaisTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLEndPais(int?[] ids)
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
                    databaseManager.SP_GRL_ENT_END_PAIS(i, null, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndPaisTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters cidades
        [HttpPost]
        public ActionResult GetGRLEndCidade()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Pais = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GRL_ENT_END_CIDADE(null, null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
            if (!string.IsNullOrEmpty(Pais)) v = v.Where(a => a.PAIS_NOME != null && a.PAIS_NOME.ToUpper().Contains(Pais.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
                        case "PAIS": v = v.OrderBy(s => s.PAIS_NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
                        case "PAIS": v = v.OrderByDescending(s => s.PAIS_NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
                    PAIS = x.PAIS_NOME,
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
        public ActionResult NewGRLEndCidade(GRLEndCidade MODEL)
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
                var create = databaseManager.SP_GRL_ENT_END_CIDADE(null, MODEL.SIGLA, MODEL.NOME, MODEL.ENDERECO_PAIS_ID, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndCidadeTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLEndCidade(GRLEndCidade MODEL)
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
                var update = databaseManager.SP_GRL_ENT_END_CIDADE(MODEL.ID, MODEL.SIGLA, MODEL.NOME, MODEL.ENDERECO_PAIS_ID, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndCidadeTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLEndCidade(int?[] ids)
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
                    databaseManager.SP_GRL_ENT_END_CIDADE(i, null, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndCidadeTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters distritos
        [HttpPost]
        public ActionResult GetGRLEndDistr()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Cidade = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GRL_ENT_END_DISTR(null, null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
            if (!string.IsNullOrEmpty(Cidade)) v = v.Where(a => a.CIDADE_NOME != null && a.CIDADE_NOME.ToUpper().Contains(Cidade.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
                        case "CIDADE": v = v.OrderBy(s => s.CIDADE_NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
                        case "CIDADE": v = v.OrderByDescending(s => s.CIDADE_NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
                    CIDADE = x.CIDADE_NOME,
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
        public ActionResult NewGRLEndDistr(GRLEndDistr MODEL)
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
                var create = databaseManager.SP_GRL_ENT_END_DISTR(null, MODEL.SIGLA, MODEL.NOME, MODEL.ENDERECO_CIDADE_ID, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndDistrTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLEndDistr(GRLEndDistr MODEL)
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
                var update = databaseManager.SP_GRL_ENT_END_DISTR(MODEL.ID, MODEL.SIGLA, MODEL.NOME, MODEL.ENDERECO_CIDADE_ID, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndDistrTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLEndDistr(int?[] ids)
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
                    databaseManager.SP_GRL_ENT_END_DISTR(i, null, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndDistrTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters tipo ident
        [HttpPost]
        public ActionResult GetGRLIdentType()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_PES_ENT_TIPO_IDENTIFICACAO(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
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
        public ActionResult NewGRLIdentType(PES_TIPO_IDENTIFICACAO MODEL)
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
                if (databaseManager.PES_TIPO_IDENTIFICACAO.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_PES_ENT_TIPO_IDENTIFICACAO(null, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLIdentTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLIdentType(PES_TIPO_IDENTIFICACAO MODEL)
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
                if (databaseManager.PES_TIPO_IDENTIFICACAO.Where(x => x.SIGLA == MODEL.SIGLA && x.ID!=MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_PES_ENT_TIPO_IDENTIFICACAO(MODEL.ID, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLIdentTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLIdentType(int?[] ids)
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
                    databaseManager.SP_PES_ENT_TIPO_IDENTIFICACAO(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLIdentTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters estado civil
        [HttpPost]
        public ActionResult GetGRLEstadoCivil()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_PES_ENT_ESTADOCIVIL(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
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
        public ActionResult NewGRLEstadoCivil(PES_ESTADO_CIVIL MODEL)
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
                if (databaseManager.PES_ESTADO_CIVIL.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_PES_ENT_ESTADOCIVIL(null, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEstadoCivilTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLEstadoCivil(PES_ESTADO_CIVIL MODEL)
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
                if (databaseManager.PES_ESTADO_CIVIL.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_PES_ENT_ESTADOCIVIL(MODEL.ID, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEstadoCivilTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLEstadoCivil(int?[] ids)
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
                    databaseManager.SP_PES_ENT_ESTADOCIVIL(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEstadoCivilTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters tipo endereco
        [HttpPost]
        public ActionResult GetGRLEndType()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_PES_ENT_TIPO_ENDERECO(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
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
        public ActionResult NewGRLEndType(PES_TIPO_ENDERECOS MODEL)
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
                if (databaseManager.PES_TIPO_ENDERECOS.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_PES_ENT_TIPO_ENDERECO(null, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndTipoEndTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLEndType(PES_TIPO_ENDERECOS MODEL)
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
                if (databaseManager.PES_TIPO_ENDERECOS.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_PES_ENT_TIPO_ENDERECO(MODEL.ID, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndTipoEndTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLEndType(int?[] ids)
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
                    databaseManager.SP_PES_ENT_TIPO_ENDERECO(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLEndTipoEndTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters profissoes
        [HttpPost]
        public ActionResult GetGRLProfissao()
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
            var Insercao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_PES_ENT_PROFISSOES(null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                    Id = x.ID,
                    NOME = x.NOME,
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
        public ActionResult NewGRLProfissao(PES_PROFISSOES MODEL)
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
                if (databaseManager.PES_PROFISSOES.Where(x => x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_PES_ENT_PROFISSOES(null, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLProfissaoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLProfissao(PES_PROFISSOES MODEL)
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
                if (databaseManager.PES_PROFISSOES.Where(x => x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_PES_ENT_PROFISSOES(MODEL.ID, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLProfissaoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLProfissao(int?[] ids)
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
                    databaseManager.SP_PES_ENT_PROFISSOES(i, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLProfissaoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters tipo de contractos
        [HttpPost]
        public ActionResult GetGRLProfContract()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_PES_ENT_TIPO_CONTRACTO(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
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
        public ActionResult NewGRLProfContract(PES_PROFISSOES_TIPO_CONTRACTO MODEL)
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
                if (databaseManager.PES_PROFISSOES_TIPO_CONTRACTO.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_PES_ENT_TIPO_CONTRACTO(null, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLProfissaoTipoContractoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLProfContract(PES_PROFISSOES_TIPO_CONTRACTO MODEL)
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
                if (databaseManager.PES_PROFISSOES_TIPO_CONTRACTO.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_PES_ENT_TIPO_CONTRACTO(MODEL.ID, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLProfissaoTipoContractoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLProfContract(int?[] ids)
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
                    databaseManager.SP_PES_ENT_TIPO_CONTRACTO(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLProfissaoTipoContractoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters tipo de regimes
        [HttpPost]
        public ActionResult GetGRLProfRegime()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_PES_ENT_TIPO_REGIME(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
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
        public ActionResult NewGRLProfRegime(PES_PROFISSOES_REGIME_TRABALHO MODEL)
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
                if (databaseManager.PES_PROFISSOES_REGIME_TRABALHO.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_PES_ENT_TIPO_REGIME(null, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLProfissaoTipoRegimeTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLProfRegime(PES_PROFISSOES_REGIME_TRABALHO MODEL)
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
                if (databaseManager.PES_PROFISSOES_REGIME_TRABALHO.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_PES_ENT_TIPO_REGIME(MODEL.ID, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLProfissaoTipoRegimeTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLProfRegime(int?[] ids)
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
                    databaseManager.SP_PES_ENT_TIPO_REGIME(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLProfissaoTipoRegimeTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters tipo de agregado
        [HttpPost]
        public ActionResult GetGRLPESAgregado()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_PES_ENT_FAMILIARES_GRUPO(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
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
        public ActionResult NewGRLPESAgregado(PES_FAMILIARES_GRUPOS MODEL)
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
                if (databaseManager.PES_FAMILIARES_GRUPOS.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_PES_ENT_FAMILIARES_GRUPO(null, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLAgregadoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLPESAgregado(PES_FAMILIARES_GRUPOS MODEL)
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
                if (databaseManager.PES_FAMILIARES_GRUPOS.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_PES_ENT_FAMILIARES_GRUPO(MODEL.ID, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLAgregadoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLPESAgregado(int?[] ids)
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
                    databaseManager.SP_PES_ENT_FAMILIARES_GRUPO(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLAgregadoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters grupo sanguineo
        [HttpPost]
        public ActionResult GetGRLPESGrupoSang()
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
            var Insercao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_PES_ENT_CARACT_TIPO_SANG(null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                    Id = x.ID,
                    NOME = x.NOME,
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
        public ActionResult NewGRLPESGrupoSang(PES_PESSOAS_CARACT_TIPO_SANG MODEL)
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
                if (databaseManager.PES_PESSOAS_CARACT_TIPO_SANG.Where(x => x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_PES_ENT_CARACT_TIPO_SANG(null, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLGroupSangTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLPESGrupoSang(PES_PESSOAS_CARACT_TIPO_SANG MODEL)
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
                if (databaseManager.PES_PESSOAS_CARACT_TIPO_SANG.Where(x => x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_PES_ENT_CARACT_TIPO_SANG(MODEL.ID, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLGroupSangTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLPESGrupoSang(int?[] ids)
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
                    databaseManager.SP_PES_ENT_CARACT_TIPO_SANG(i, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLGroupSangTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters tipo de deficiencia
        [HttpPost]
        public ActionResult GetGRLPESDef()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_PES_ENT_CARACT_TIPO_DEF(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper().Contains(Sigla.ToUpper()));
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper().Contains(Nome.ToUpper()));
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                    Id = x.ID,
                    NOME = x.NOME,
                    SIGLA = x.SIGLA,
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
        public ActionResult NewGRLPESDef(PES_PESSOAS_CARACT_TIPO_DEF MODEL)
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
                if (databaseManager.PES_PESSOAS_CARACT_TIPO_DEF.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME == MODEL.NOME).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_PES_ENT_CARACT_TIPO_DEF(null, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoDefTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UdpateGRLPESDef(PES_PESSOAS_CARACT_TIPO_DEF MODEL)
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
                if (databaseManager.PES_PESSOAS_CARACT_TIPO_DEF.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla e/ou Nome desta entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_PES_ENT_CARACT_TIPO_DEF(MODEL.ID, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoDefTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLPESDef(int?[] ids)
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
                    databaseManager.SP_PES_ENT_CARACT_TIPO_DEF(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoDefTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters GT duracao de planos
        [HttpPost]
        public ActionResult GetGRLGTDuracaoPlano()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Duracao = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GT_ENT_DURACAO_TIPO(null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Duracao)) v = v.Where(a => a.DURACAO != null && a.DURACAO.ToString()==Duracao);
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
                        case "DURACAO": v = v.OrderBy(s => s.DURACAO); break;
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
                        case "DURACAO": v = v.OrderByDescending(s => s.DURACAO); break;
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
                     Id = x.ID,
                    DURACAO = x.DURACAO,
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
        public ActionResult NewGRLGTDuracaoPlano(GT_DuracaoPlano MODEL)
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
                if (databaseManager.GT_DuracaoPlano.Where(x => x.DURACAO == MODEL.DURACAO).Any())
                    return Json(new { result = false, error = "Entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_GT_ENT_DURACAO_TIPO(null, MODEL.DURACAO, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLGTDuracaoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLGTDuracaoPlano(GT_DuracaoPlano MODEL)
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
                if (databaseManager.GT_DuracaoPlano.Where(x => x.DURACAO == MODEL.DURACAO && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_GT_ENT_DURACAO_TIPO(MODEL.ID, MODEL.DURACAO, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLGTDuracaoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLGTDuracaoPlano(int?[] ids)
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
                    databaseManager.SP_GT_ENT_DURACAO_TIPO(i, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLGTDuracaoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters GT Fase de treino
        [HttpPost]
        public ActionResult GetGRLGTFaseTreinoTable()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Serie = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Reps = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var RM = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Descanso = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[8][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GT_ENT_FaseTreino(null, null, null, null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.SIGLA != null && a.SIGLA.ToUpper() == Sigla.ToUpper());
            if (!string.IsNullOrEmpty(Serie)) v = v.Where(a => a.GT_Series_ID != null && a.GT_Series_ID.ToString() == Serie);
            if (!string.IsNullOrEmpty(Reps)) v = v.Where(a => a.GT_Repeticoes_ID != null && a.GT_Repeticoes_ID.ToString() == Reps);
            if (!string.IsNullOrEmpty(RM)) v = v.Where(a => a.GT_Carga_ID != null && a.GT_Carga_ID.ToString() == RM);
            if (!string.IsNullOrEmpty(Descanso)) v = v.Where(a => a.GT_TempoDescanso_ID != null && a.GT_TempoDescanso_ID.ToString() == Descanso);
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
                        case "SIGLA": v = v.OrderBy(s => s.SIGLA); break;
                        case "SERIE": v = v.OrderBy(s => s.GT_Series_ID); break;
                        case "REPS": v = v.OrderBy(s => s.GT_Repeticoes_ID); break;
                        case "RM": v = v.OrderBy(s => s.GT_Carga_ID); break;
                        case "DESCANSO": v = v.OrderBy(s => s.TEMPO_DESCANSO); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.SIGLA); break;
                        case "SERIE": v = v.OrderByDescending(s => s.GT_Series_ID); break;
                        case "REPS": v = v.OrderByDescending(s => s.GT_Repeticoes_ID); break;
                        case "RM": v = v.OrderByDescending(s => s.GT_Carga_ID); break;
                        case "DESCANSO": v = v.OrderByDescending(s => s.TEMPO_DESCANSO); break;
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
                    Id = x.ID,
                    SIGLA = x.SIGLA,
                    SERIE = x.SERIES,
                    REPS = x.REPETICOES,
                    RM = x.CARGA,
                    DESCANSO = x.TEMPO_DESCANSO,
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
        public ActionResult NewGRLGTFaseTreino(Gestreino.Models.GT_FaseTreino MODEL)
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
                if (databaseManager.GT_FaseTreino.Where(x => x.SIGLA == MODEL.SIGLA).Any())
                    return Json(new { result = false, error = "Sigla da entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_GT_ENT_FaseTreino(null, MODEL.SIGLA, MODEL.GT_Series_ID, MODEL.GT_Repeticoes_ID, MODEL.GT_Carga_ID, MODEL.GT_TempoDescanso_ID, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLGTFaseTreinoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLGTFaseTreino(Gestreino.Models.GT_FaseTreino MODEL)
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
                if (databaseManager.GT_FaseTreino.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID).Any())
                    return Json(new { result = false, error = "Sigla da entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var update = databaseManager.SP_GT_ENT_FaseTreino(MODEL.ID, MODEL.SIGLA,MODEL.GT_Series_ID,MODEL.GT_Repeticoes_ID,MODEL.GT_Carga_ID,MODEL.GT_TempoDescanso_ID, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLGTFaseTreinoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLGTFaseTreino(int?[] ids)
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
                    databaseManager.SP_GT_ENT_FaseTreino(i, null, null, null, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLGTFaseTreinoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

        //Parameters GT Tipos de Treino
        [HttpPost]
        public ActionResult GetGRLGTTipoTreino()
        {
            //UI DATATABLE PAGINATION BUTTONS
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();

            //UI DATATABLE COLUMN ORDERING
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            //UI DATATABLE SEARCH INPUTS
            var Sigla = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var Nome = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var Insercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_GT_ENT_TIPOTREINO(null, null, null, null, "R").ToList() select a);
            TempData["QUERYRESULT_ALL"] = v.ToList();

            //SEARCH RESULT SET
            if (!string.IsNullOrEmpty(Sigla)) v = v.Where(a => a.sigla != null && a.sigla.ToUpper() == Sigla.ToUpper());
            if (!string.IsNullOrEmpty(Nome)) v = v.Where(a => a.NOME != null && a.NOME.ToUpper() == Nome.ToUpper());
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
                        case "SIGLA": v = v.OrderBy(s => s.sigla); break;
                        case "NOME": v = v.OrderBy(s => s.NOME); break;
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
                        case "SIGLA": v = v.OrderByDescending(s => s.sigla); break;
                        case "NOME": v = v.OrderByDescending(s => s.NOME); break;
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
                     Id = x.ID,
                    SIGLA = x.sigla,
                    NOME = x.NOME,
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
        public ActionResult NewGRLGTTipoTreino(GT_TipoTreino MODEL)
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
                if (databaseManager.GT_TipoTreino.Where(x => x.SIGLA == MODEL.SIGLA || x.NOME==MODEL.NOME).Any())
                    return Json(new { result = false, error = "Nome e/ou Sigla da entidade já se encontra registada, por favor verifique a seleção!" });

                // Create
                var create = databaseManager.SP_GT_ENT_TIPOTREINO(null, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoTreinoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGRLGTTipoTreino(GT_TipoTreino MODEL)
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
                 if (databaseManager.GT_TipoTreino.Where(x => x.SIGLA == MODEL.SIGLA && x.ID != MODEL.ID || x.NOME == MODEL.NOME && x.ID!=MODEL.ID).Any())
                    return Json(new { result = false, error = "Nome e/ou Sigla da entidade já se encontra registada, por favor verifique a seleção!" });

                // Update
                var create = databaseManager.SP_GT_ENT_TIPOTREINO(MODEL.ID, MODEL.SIGLA, MODEL.NOME, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoTreinoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGRLGTTipoTreino(int?[] ids)
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
                    databaseManager.SP_GT_ENT_TIPOTREINO(i, null, null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GRLTipoTreinoTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }




        //Settings 
        [HttpGet]
        public ActionResult Settings(SettingsDef MODEL)
        {
           if (AcessControl.Authorized(AcessControl.ADM_CONFIG_FILEMGR) || AcessControl.Authorized(AcessControl.ADM_CONFIG_INST_EDIT) || AcessControl.Authorized(AcessControl.ADM_CONFIG_SETINGS_EDIT)) { }else
           return View("Lockout");

            var data = databaseManager.SP_INST_APLICACAO(Configs.INST_INSTITUICAO_ID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList();
            var setts = databaseManager.GRL_DEFINICOES.Where(x => x.INST_APLICACAO_ID == Configs.INST_INSTITUICAO_ID).ToList();

            var path = (from j1 in databaseManager.INST_APLICACAO
                                             join j2 in databaseManager.INST_APLICACAO_ARQUIVOS on j1.ID equals j2.INST_APLICACAO_ID
                                             join j3 in databaseManager.GRL_ARQUIVOS on j2.ARQUIVOS_ID equals j3.ID
                                             where j1.ID == Configs.INST_INSTITUICAO_ID && j3.GRL_ARQUIVOS_TIPO_DOCS_ID == Configs.INST_MDL_ADM_VLRID_ARQUIVO_LOGOTIPO
                                             orderby j2.ID
                                             select new { j3.CAMINHO_URL });

            Configs.INST_INSTITUICAO_LOGO=path.Any()?path.FirstOrDefault().CAMINHO_URL:string.Empty;
            ViewBag.imgSrc = (string.IsNullOrEmpty(Configs.INST_INSTITUICAO_LOGO)) ? "/Assets/images/user-avatar.jpg" : "/" + Configs.INST_INSTITUICAO_LOGO;


            MODEL.MOEDA_LIST = databaseManager.GRL_ENDERECO_PAIS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME + " - " + x.CODIGO.ToString() });
            MODEL.INST_PER_TEMA_1 = setts.First().INST_PER_TEMA_1;
            MODEL.INST_PER_TEMA_1_SIDEBAR = setts.First().INST_PER_TEMA_1_SIDEBAR;
            MODEL.INST_PER_TEMA_2 = setts.First().INST_PER_TEMA_2;
            MODEL.INST_PER_LOGOTIPO_WIDTH = setts.First().INST_PER_LOGOTIPO_WIDTH;
            MODEL.INST_MDL_GPAG_MOEDA_PADRAO = setts.First().INST_MDL_GPAG_MOEDA_PADRAO;
            MODEL.INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS = setts.First().INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS;
            MODEL.INST_MDL_GPAG_NOTA_DECIMAL = setts.First().INST_MDL_GPAG_NOTA_DECIMAL;
            MODEL.SEC_SENHA_TENT_BLOQUEIO = setts.First().SEC_SENHA_TENT_BLOQUEIO;
            MODEL.SEC_SENHA_TENT_BLOQUEIO_TEMPO = setts.First().SEC_SENHA_TENT_BLOQUEIO_TEMPO;
            MODEL.SEC_SENHA_RECU_LIMITE_EMAIL = setts.First().SEC_SENHA_RECU_LIMITE_EMAIL;
            MODEL.SEC_SESSAO_TIMEOUT_TEMPO = setts.First().SEC_SESSAO_TIMEOUT_TEMPO;
            MODEL.NET_STMP_HOST = setts.First().NET_STMP_HOST;
            MODEL.NET_STMP_PORT = setts.First().NET_STMP_PORT;
            MODEL.NET_SMTP_USERNAME = setts.First().NET_SMTP_USERNAME;
            MODEL.NET_SMTP_SENHA = setts.First().NET_SMTP_SENHA;
            MODEL.NET_SMTP_FROM = setts.First().NET_SMTP_FROM;
            ViewBag.data = data;
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Settings;
            return View("Settings/Index", MODEL);
        }
        [HttpGet]
        public ActionResult UpdateInst(SettingsInst MODEL)
        {
            if (!AcessControl.Authorized(AcessControl.ADM_CONFIG_INST_EDIT)) return View("Lockout");

            var data = databaseManager.SP_INST_APLICACAO(Configs.INST_INSTITUICAO_ID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList();

            MODEL.ID = data.First().ID;
            MODEL.Sigla = data.First().SIGLA;
            MODEL.Nome = data.First().NOME;
            MODEL.NIF = data.First().NIF;

            MODEL.Telephone = (!string.IsNullOrEmpty(data.First().TELEFONE.ToString())) ? data.First().TELEFONE.ToString() : null;
            MODEL.TelephoneAlternativo = (!string.IsNullOrEmpty(data.First().TELEFONE_ALTERNATIVO.ToString())) ? data.First().TELEFONE_ALTERNATIVO.ToString() : null;
            MODEL.Fax = (!string.IsNullOrEmpty(data.First().FAX.ToString())) ? data.First().FAX.ToString() : null;
            MODEL.Email = data.First().EMAIL;
            MODEL.CodigoPostal = data.First().CODIGO_POSTAL;
            MODEL.URL = data.First().URL;
            MODEL.Numero = data.First().NUMERO;
            MODEL.Rua = data.First().RUA;
            MODEL.Morada = data.First().MORADA;
            MODEL.ENDERECO_PAIS_ID = data.First().ENDERECO_PAIS_ID;
            MODEL.ENDERECO_CIDADE_ID = data.First().GRL_ENDERECO_CIDADE_ID;
            MODEL.ENDERECO_MUN_ID = data.First().ENDERECO_MUN_DISTR_ID;
            MODEL.PAIS_LIST = databaseManager.GRL_ENDERECO_PAIS.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.CIDADE_LIST = databaseManager.GRL_ENDERECO_CIDADE.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            MODEL.MUN_LIST = databaseManager.GRL_ENDERECO_MUN_DISTR.Where(x => x.DATA_REMOCAO == null).OrderBy(x => x.NOME).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.NOME });
            ViewBag.LeftBarLinkActive = _MenuLeftBarLink_Settings;
            return View("Settings/UpdateInst", MODEL);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInst(SettingsInst MODEL, string returnUrl)
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

                Decimal Telephone = (!string.IsNullOrEmpty(MODEL.Telephone)) ? Convert.ToDecimal(MODEL.Telephone) : 0;
                Decimal TelephoneAlternativo = (!string.IsNullOrEmpty(MODEL.TelephoneAlternativo)) ? Convert.ToDecimal(MODEL.TelephoneAlternativo) : 0;
                Decimal Fax = (!string.IsNullOrEmpty(MODEL.Fax)) ? Convert.ToDecimal(MODEL.Fax) : 0;

                // Update
                var update = databaseManager.SP_INST_APLICACAO(MODEL.ID, MODEL.Sigla, MODEL.Nome, MODEL.NIF, Telephone, TelephoneAlternativo, Fax, MODEL.Email, MODEL.CodigoPostal, MODEL.URL, MODEL.Numero, MODEL.Rua, MODEL.Morada, MODEL.ENDERECO_PAIS_ID, MODEL.ENDERECO_CIDADE_ID, MODEL.ENDERECO_MUN_ID, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();

                //Update Config files
                Configs.INST_INSTITUICAO_SIGLA = string.Empty;
                Configs c = new Configs();
                c.BeginConfig();

                returnUrl = "/administration/settings";
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, url = returnUrl, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInstPer(SettingsDef MODEL)
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
                using (var db = databaseManager)
                {
                    var row = db.GRL_DEFINICOES.FirstOrDefault(x => x.INST_APLICACAO_ID == Configs.INST_INSTITUICAO_ID);

                    // this variable is tracked by the db context
                    row.INST_PER_TEMA_1 = MODEL.INST_PER_TEMA_1;
                    row.INST_PER_TEMA_1_SIDEBAR = MODEL.INST_PER_TEMA_1_SIDEBAR;
                    row.INST_PER_TEMA_2 = MODEL.INST_PER_TEMA_2;
                    row.INST_PER_LOGOTIPO_WIDTH = MODEL.INST_PER_LOGOTIPO_WIDTH;
                    db.SaveChanges();
                }
                ModelState.Clear();

                //Update Config files
                Configs.INST_INSTITUICAO_SIGLA = string.Empty;
                Configs c = new Configs();
                c.BeginConfig();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInstPag(SettingsDef MODEL)
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
                using (var db = databaseManager)
                {
                    var row = db.GRL_DEFINICOES.FirstOrDefault(x => x.INST_APLICACAO_ID == Configs.INST_INSTITUICAO_ID);

                    // this variable is tracked by the db context
                    row.INST_MDL_GPAG_MOEDA_PADRAO = MODEL.INST_MDL_GPAG_MOEDA_PADRAO;
                    row.INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS = MODEL.INST_MDL_GPAG_N_DIGITOS_VALORES_PAGAMENTOS;
                    row.INST_MDL_GPAG_NOTA_DECIMAL = MODEL.INST_MDL_GPAG_NOTA_DECIMAL;
                    db.SaveChanges();
                }
                ModelState.Clear();

                //Update Config files
                Configs.INST_INSTITUICAO_SIGLA = string.Empty;
                Configs c = new Configs();
                c.BeginConfig();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInstSec(SettingsDef MODEL)
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
                using (var db = databaseManager)
                {
                    var row = db.GRL_DEFINICOES.FirstOrDefault(x => x.INST_APLICACAO_ID == Configs.INST_INSTITUICAO_ID);

                    // this variable is tracked by the db context
                    row.SEC_SENHA_TENT_BLOQUEIO = MODEL.SEC_SENHA_TENT_BLOQUEIO;
                    row.SEC_SENHA_TENT_BLOQUEIO_TEMPO = MODEL.SEC_SENHA_TENT_BLOQUEIO_TEMPO;
                    row.SEC_SENHA_RECU_LIMITE_EMAIL = MODEL.SEC_SENHA_RECU_LIMITE_EMAIL;
                    row.SEC_SESSAO_TIMEOUT_TEMPO = MODEL.SEC_SESSAO_TIMEOUT_TEMPO;
                    db.SaveChanges();
                }
                ModelState.Clear();

                //Update Config files
                Configs.INST_INSTITUICAO_SIGLA = string.Empty;
                Configs c = new Configs();
                c.BeginConfig();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInstNet(SettingsDef MODEL)
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
                using (var db = databaseManager)
                {
                    var row = db.GRL_DEFINICOES.FirstOrDefault(x => x.INST_APLICACAO_ID == Configs.INST_INSTITUICAO_ID);

                    // this variable is tracked by the db context
                    row.NET_STMP_HOST = MODEL.NET_STMP_HOST;
                    row.NET_STMP_PORT = MODEL.NET_STMP_PORT;
                    row.NET_SMTP_USERNAME = MODEL.NET_SMTP_USERNAME;
                    row.NET_SMTP_SENHA = MODEL.NET_SMTP_SENHA;
                    row.NET_SMTP_FROM = MODEL.NET_SMTP_FROM;
                    db.SaveChanges();
                }
                ModelState.Clear();

                //Update Config files
                Configs.INST_INSTITUICAO_SIGLA = string.Empty;
                Configs c = new Configs();
                c.BeginConfig();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }

    }
}