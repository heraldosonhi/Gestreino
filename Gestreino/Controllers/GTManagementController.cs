using DocumentFormat.OpenXml.EMMA;
using Gestreino.Classes;
using Gestreino.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Gestreino.Classes.SelectValues;

namespace Gestreino.Controllers
{
    [Authorize]
    public class GTManagementController : Controller
    {
        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();


        // GET: GTManagement
        public ActionResult Index()
        {
            return View();
        }

        // GET: GTManagement
        public ActionResult Athletes()
        {
            return View("Athletes/Index");
        }









        // GET: GTManagement
        public ActionResult Users()
        {
            return View("Users/Index");
        }
        // GET: GTManagement
        public ActionResult ViewUsers(int? Id)
        {
            var data = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(Id, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList();
            ViewBag.data = data;

            return View("Users/ViewUsers");
        }
        // Ajax Table
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
            if (!string.IsNullOrEmpty(Grupos)) v = v.Where(a => a.TOTALGROUPS != null && a.TOTALGROUPS.ToString()==Grupos);
            if (!string.IsNullOrEmpty(Perfis)) v = v.Where(a => a.TOTALPERFIS != null && a.TOTALPERFIS.ToString()==Perfis);
            if (!string.IsNullOrEmpty(Estado)) v = v.Where(a => a.ACTIVO != null && a.ACTIVO.Contains(Estado));
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_DELETE) ? "none" : "",
                    Id = x.ID,
                    LOGIN = x.LOGIN,
                    NOME = x.NOME_PROPIO+" "+x.APELIDO,
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




        // Create
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

                var Status = true;
                var DateIni = string.IsNullOrWhiteSpace(MODEL.DateAct) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateAct, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var DateEnd = string.IsNullOrWhiteSpace(MODEL.DateDisact) ? (DateTime?)null : DateTime.ParseExact(MODEL.DateDisact, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                // Create Salted Password
                var Salt = Crypto.GenerateSalt(64);
                var Password = Crypto.Hash(MODEL.Password.Trim() + Salt);
                // Remove whitespaces and parse datetime strings //TrimStart() //Trim()

                // Create
                var create = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(null, null, null, MODEL.Login, MODEL.Name, Convert.ToDecimal(MODEL.Phone), MODEL.Email.Trim(), Password, Salt, Status, DateIni,DateEnd,true, int.Parse(User.Identity.GetUserId()), "C").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserAddressTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }





        // GET: GTManagement
        public ActionResult Access()
        {
            return View("Access/Index");
        }

        // GET: GTManagement
        public ActionResult ViewGroups(int? Id)
        {
            var data = databaseManager.SP_UTILIZADORES_ENT_GRUPOS(Id, null, null, null, null, "R").ToList();
            ViewBag.data = data;

            return View("Access/ViewGroups");
        }
        // GET: GTManagement
        public ActionResult ViewProfiles(int? Id)
        {
            var data = databaseManager.SP_UTILIZADORES_ENT_PERFIS(Id, null, null, null, "R").ToList();
            ViewBag.data = data;

            return View("Access/ViewProfiles");
        }
        // GET: GTManagement
        public ActionResult ViewAtoms(int? Id)
        {
            var data = databaseManager.SP_UTILIZADORES_ENT_ATOMOS(Id, null, null, null, "R").ToList();
            ViewBag.data = data;

            return View("Access/ViewAtoms");
        }

        // Ajax Table
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_DELETE) ? "none" : "",
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

        // Ajax Table
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
            var Insercao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_PERFIS(null, null, null, null, "R").ToList() select a);
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_DELETE) ? "none" : "",
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

        // Ajax Table
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
            var Insercao = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var DataInsercao = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var Actualizacao = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var DataActualizacao = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();

            //DECLARE PAGINATION VARIABLES
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = (from a in databaseManager.SP_UTILIZADORES_ENT_ATOMOS(null, null, null, null, "R").ToList() select a);
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_DELETE) ? "none" : "",
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





        // Create
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
        // Update
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

                // Update
                var update = databaseManager.SP_UTILIZADORES_ENT_GRUPOS(MODEL.ID,MODEL.SIGLA, MODEL.NOME, MODEL.DESCRICAO, int.Parse(User.Identity.GetUserId()), "U").ToList();
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "GroupTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Delete
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









        // Create
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
        // Update
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
        // Delete
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


        // Create
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
        // Update
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
        // Delete
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
                    databaseManager.SP_UTILIZADORES_ENT_ATOMOS(i, null,null, int.Parse(User.Identity.GetUserId()), "D").ToList();
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "AtomTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }








        // Ajax Table
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_DELETE) ? "none" : "",
                    Id = x.ID,
                    GRUPO = x.GRUPO_NOME+ " (" + x.SIGLA + ")",
                    UTILIZADOR = x.NOME_PROPIO+" "+x.APELIDO+" ("+x.LOGIN+")",
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
                // Validate
                foreach (var i in items)
                {
                    if(databaseManager.UTILIZADORES_ACESSO_UTIL_GRUPOS.Where(x=>x.UTILIZADORES_ACESSO_GRUPOS_ID==MODEL.GroupId && x.UTILIZADORES_ID==i).Any())
                      return Json(new { result = false, error = "Relação já se encontra associada, por favor verifique a seleção!" });
                }
                // Create
                foreach (var i in items)
                {
                    var create = databaseManager.SP_UTILIZADORES_ENT_GRUPOS_UTILIZADORES(null, MODEL.GroupId, i, int.Parse(User.Identity.GetUserId()), "C").ToList();
                }
                    ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "UserGroupTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Delete
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





        // Ajax Table
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_DELETE) ? "none" : "",
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
        // Create
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
                    if(MODEL.AtomId==null)
                       databaseManager.SP_UTILIZADORES_ENT_ATOMOS_GRUPOS(null,MODEL.GroupId, i, int.Parse(User.Identity.GetUserId()), "C").ToList();
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
        // Delete
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




        // Ajax Table
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_DELETE) ? "none" : "",
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
        // Create
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
        // Delete
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




        // Ajax Table
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
                    //AccessControlEdit = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_EDIT) ? "none" : "",
                    //AccessControlDelete = !AcessControl.Authorized(AcessControl.GP_USERS_ACADEMIC_DELETE) ? "none" : "",
                    Id = x.ID,
                    PERFIL = x.PERFIL_NOME,
                    UTILIZADOR = x.NOME_PROPIO+" "+x.APELIDO+" ("+x.LOGIN+")",
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
                        databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES_PERFIS(null, i, MODEL.AtomId, int.Parse(User.Identity.GetUserId()), "C").ToList();

                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = ex.Message });
            }
            return Json(new { result = true, error = string.Empty, table = "ProfileUtilTable", showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // Delete
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
    }
}