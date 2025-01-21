using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Gestreino.Models;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Mail;
using System.Xml;
using Gestreino.Classes;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Drawing.Charts;
using Gestreino;

namespace JeanPiagetSGA.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();
        ExportEmail Mailer = new ExportEmail();

        public AccountController()
        {
        }
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // GET: /Account/Profile
        [Authorize]
        public ActionResult Profile(ProfileViewModel MODEL)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var existingClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
            ViewBag.imgSrc = (existingClaim != null && !string.IsNullOrEmpty(claimsIdentity.FindFirst(ClaimTypes.UserData).Value) ? "/" + claimsIdentity.FindFirst(ClaimTypes.UserData).Value : "/Assets/images/user-avatar.jpg");
            MODEL.Login = User.Identity.GetUserName();
            var PesId = (from j1 in databaseManager.UTILIZADORES
                         join j2 in databaseManager.PES_PESSOAS on j1.ID equals j2.UTILIZADORES_ID
                         where j1.LOGIN == MODEL.Login && j2.DATA_REMOCAO == null
                         select new { j2.ID, j1.DATA_INSERCAO }).FirstOrDefault();
            var item = databaseManager.SP_PES_ENT_PESSOAS(PesId.ID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, Convert.ToChar('R').ToString()).OrderByDescending(x => x.NOME).Select(x => new { x.NOME, x.TELEFONE, x.TELEFONE_ALTERNATIVO, x.EMAIL, x.UTILIZADORES_ID, x.GRUPO_UTILIZADORES, x.APRESENTACAO_PESSOAL }).FirstOrDefault();
            MODEL.UserID = item.UTILIZADORES_ID;
            MODEL.Nome = item.NOME;
            MODEL.Email = item.EMAIL;
            MODEL.DataCriacao = PesId.DATA_INSERCAO.ToString("dd/MM/yyyy HH:mm");
            MODEL.Telefone = item.TELEFONE.ToString();
            MODEL.TelefoneAlternativo = item.TELEFONE_ALTERNATIVO.ToString();
            ViewBag.Grupos = (string.IsNullOrEmpty(item.GRUPO_UTILIZADORES)) ? "Não tem grupos associados" : item.GRUPO_UTILIZADORES;
            ViewBag.Apresentacao = (!string.IsNullOrEmpty(item.APRESENTACAO_PESSOAL)) ? Converters.StripHTML(item.APRESENTACAO_PESSOAL) : "Não tem definido uma apresentação pessoal";

            //var PessoaEndereco = databaseManager.SP_PES_ENT_PESSOAS_ENDERECOS(PesId.ID, null, null, null, null, null, null, null, null, null, null, null, "R").Where(x => x.ENDERECO_PRINCIAL == "Sim").ToList();
            //ViewBag.PessoaEndereco = PessoaEndereco.Count() > 0 ? PessoaEndereco[0].MUN + ", " + PessoaEndereco[0].CIDADE : String.Empty;
            //ViewBag.MORADA = PessoaEndereco.Count() > 0 ? PessoaEndereco[0].MORADA : String.Empty;
            //ViewBag.PessoaIdent = databaseManager.SP_PES_ENT_PESSOAS_IDENTIFICACAO(PesId.ID, null, null, null, null, null, null, null, null, null, null, "R").ToList();

            /*
             // Get claims after login
             var claimsIdentity = User.Identity as ClaimsIdentity;
             // Fetch grupos
             var grupoClaim = claimsIdentity.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarygroupsid").ToList();
             // Fetch subgrupos
             var subgrupoClaim = claimsIdentity.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/groupsid").ToList();
             // Fetch atomos
             var atomoClaim = claimsIdentity.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").ToList();

             List<string> grupos = new List<string>();
             List<string> subgrupos = new List<string>();
             List<string> atomos = new List<string>();

             foreach (var i in subgrupoClaim)
             {
                 subgrupos.Add(i.Value);
             }
             foreach (var i in grupoClaim)
             {
                 grupos.Add(i.Value);
             }
             foreach (var i in atomoClaim)
             {
                 atomos.Add(i.Value);
             }
             //string combindedStringG = string.Join(",", grupos.ToArray());
             //string combindedStringS = string.Join(",", subgrupos.ToArray());
             //string combindedStringA = string.Join(",", atomos.ToArray());
             ViewBag.Grupos = grupos;
             ViewBag.SubGrupos = subgrupos;
             ViewBag.Atomos = atomos;
            */
            ViewBag.LeftBarLinkActive = 0;
            return View(MODEL);
        }
        // POST: Reset Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(string returnUrl, ProfileViewModel MODEL)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string errors = string.Empty;
                    //var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                    ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                    return Json(new { result = false, error = errors });
                }

                var loginInfo = databaseManager.UTILIZADORES.Where(a => a.ID == MODEL.UserID && a.DATA_REMOCAO == null).FirstOrDefault();
                if (string.Compare(Crypto.Hash(MODEL.OldPassword + loginInfo.SALT), loginInfo.SENHA_PASSWORD) != 0)
                {
                    return Json(new { result = false, error = "Senha de acesso inválida!" });
                }

                // Create Salted Password
                var Salt = Crypto.GenerateSalt(64);
                var Password = Crypto.Hash(MODEL.Password.Trim() + Salt);
                // Remove whitespaces and parse datetime strings //TrimStart() //Trim()
                var update = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(MODEL.UserID, null, null, null, null, null, null, Password, Salt, null, null, null, null, int.Parse(User.Identity.GetUserId()), Convert.ToChar('P').ToString()).ToArray();

                ModelState.Clear();
            }
            catch (Exception ex)
            {
                return Json(new { result = false, error = "Ocorreu um erro ao processar o seu pedido, por favor tente novamente mais tarde!"/* ex.Message */});
            }

            return Json(new { result = true, error = string.Empty, resetForm = true, showToastr = true, toastrMessage = "Submetido com sucesso!" });
        }
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult PasswordRecovery()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordRecovery(PasswordResetViewModel MODEL)
        {
            //  VALIDATE FORM FIRST
            if (!ModelState.IsValid)
            {
                string errors = string.Empty;
                ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                return Json(new { result = false, error = errors });
            }
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            int tokenid = Configs.TOKENS[0]; // Recuperar senha 
            /*
            if (databaseManager.PES_CONTACTOS.Where(a => a.EMAIL == MODEL.Email && a.DATA_REMOCAO == null).ToList().Count() == 0)
                return Json(new { result = false, error = "Este email não está associado a uma conta de utilizador!" });

            if (databaseManager.GRL_TOKENS.Where(a => a.CONTEUDO == MODEL.Email && a.C_TOKENS_TIPOS_ID == tokenid && a.DATA_REMOCAO == null).ToList().Count() > 0)
            {
                // Get last timestamp from token
                var DateAfter = databaseManager.GRL_TOKENS.Where(a => a.CONTEUDO == MODEL.Email && a.C_TOKENS_TIPOS_ID == tokenid && a.DATA_REMOCAO == null).OrderByDescending(x => x.ID).Select(x => x.DATA).First().AddMinutes(Convert.ToDouble(Configs.SEC_SENHA_RECU_LIMITE_EMAIL));
                TimeSpan ts = DateAfter - DateTime.Now;
                var minutes = ts.Minutes + 1;
                if (DateAfter > DateTime.Now)
                    return Json(new { result = false, error = "Aguarde " + minutes + " minuto(s) antes de requisitar um outro link, se não recebeu o nosso email na sua caixa de entrada por favor verifique a sua pasta de Spam 'Email de Lixo'!" });
            }

            string pesname = databaseManager.PES_PESSOAS.Join(databaseManager.PES_CONTACTOS,
                              x => x.ID,
                              y => y.PES_PESSOAS_ID,
                             (x, y) => new { y.EMAIL, x.NOME }).Where(x => x.EMAIL == MODEL.Email).Select(x => x.NOME).SingleOrDefault();

            // Insert Token
            var token = Crypto.GenerateToken();
            var tokenUrlEncode = HttpUtility.UrlEncode(token);

            // Send Email
            string url = "https://unipiaget-angola.org/api/redirect?token=" + token + "&campus=" + Configs.INST_INSTITUICAO_SIGLA;
            Mailer.SendEmailMVC(3, MODEL.Email, pesname, tokenUrlEncode, url, null, null); // Email template - 3
            */
            // Try Catch
            //if (!string.IsNullOrEmpty(ExportEmail.StatusReport.result) && ExportEmail.StatusReport.result != "Success")
            //    return Json(new { result = false, error = "Erro ao requisitar link para recuperar acesso, por favor tente mais tarde!" /*Mailer.StatusReport.result*/ });
            //else
                // If successfull email sent insert token into database
            //    databaseManager.SP_GRL_ENT_TOKENS(null, null, tokenid, token, MODEL.Email, DateTime.Now, null, "CT").ToList();  // Recuperar senha de acesso - 1

            //return View(model);
            return Json(new { result = true, success = "Link enviado com successo, se não recebeu o nosso email na sua caixa de entrada por favor verifique a sua pasta de Spam 'Email de Lixo', O link é valído por " + Configs.SEC_SENHA_RECU_LIMITE_EMAIL + " minutos apenas.", toastrMessage = "Email enviado com successo!", resetForm = true });
        }
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordRecoveryOthers(PasswordResetViewModel MODEL)
        {
            //  VALIDATE FORM FIRST
            if (!ModelState.IsValid)
            {
                string errors = string.Empty;
                ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                return Json(new { result = false, error = errors });
            }
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var User = (from j1 in databaseManager.UTILIZADORES
                        join j2 in databaseManager.PES_PESSOAS on j1.ID equals j2.UTILIZADORES_ID
                        join j3 in databaseManager.PES_IDENTIFICACAO on j2.ID equals j3.PES_PESSOAS_ID
                        where j1.DATA_REMOCAO == null && j1.LOGIN == MODEL.Login && j3.NUMERO == MODEL.BI
                        select new { j1.ID }).ToList();
            // Exist
            if (User.Count() == 0)
                return Json(new { result = false, error = "Dados inválidos, Por favor verificar o seu número de estudante e/ou identificação pessoal!" });

            var UserId = User[0].ID;

            // Exist groupId
            /*
            if ((from j1 in databaseManager.UTILIZADORES
                 join j2 in databaseManager.UTILIZADORES_UTILI_GRUPOS_SUB on j1.ID equals j2.UTILIZADORES_ID
                 where j1.DATA_REMOCAO == null && j1.ID == UserId && j2.UTILIZADORES_GRUPOS_SUB_ID == Configs.INST_MDL_ADM_VLRID_GRUPO_UTILIZADOR_TMP && j2.ACTIVO == true
                 || j1.DATA_REMOCAO == null && j1.ID == UserId && j2.UTILIZADORES_GRUPOS_SUB_ID == Configs.INST_MDL_ADM_VLRID_GRUPO_UTILIZADOR_ALUNO && j2.ACTIVO == true
                 select new { j2.ID }).ToList().Count() == 0)
                return Json(new { result = false, error = "Não foi permitido a alteração da senha usando os seus dados biográficos!" });

            // Create Salted Password
            var Salt = Crypto.GenerateSalt(64);
            var Password = Crypto.Hash(MODEL.Password.Trim() + Salt);
            // Remove whitespaces and parse datetime strings //TrimStart() //Trim()
            var update = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(UserId, null, null, null, null, null, null, Password, Salt, null, null, null, null, null, null, null, null, null, null, 1, Convert.ToChar('P').ToString()).ToArray();
            */
            //return View(model);
            return Json(new { result = true, success = "Senha de acesso alterada com successo.", resetForm = true });
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult PasswordRecoveryToken(string token, PasswordResetTokenViewModel MODEL)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }
            /*
            string message = string.Empty;
            token = HttpUtility.UrlDecode(token).Replace(" ", "+");
            int tokenid = Configs.TOKENS[0]; // Recuperar senha 
            var tokeninfo = databaseManager.GRL_TOKENS.Where(a => a.TOKEN == token && a.C_TOKENS_TIPOS_ID == tokenid && a.DATA_REMOCAO == null).ToList();
            if (tokeninfo.Count > 0)
            {
                // Get last timestamp from token
                var DateAfter = databaseManager.GRL_TOKENS.Where(a => a.TOKEN == token && a.C_TOKENS_TIPOS_ID == tokenid && a.DATA_REMOCAO == null).OrderByDescending(x => x.ID).Select(x => x.DATA).First().AddMinutes(Convert.ToDouble(Configs.SEC_SENHA_RECU_LIMITE_EMAIL));
                TimeSpan ts = DateAfter - DateTime.Now;
                var minutes = ts.Minutes + 1;
                if (DateAfter > DateTime.Now)
                {
                    MODEL.Email = tokeninfo[0].CONTEUDO;

                    // Get userid
                    int userid = databaseManager.PES_PESSOAS.Join(databaseManager.PES_CONTACTOS,
                                x => x.ID,
                                y => y.PES_PESSOAS_ID,
                               (x, y) => new { y.EMAIL, x.UTILIZADORES_ID }).Where(x => x.EMAIL == MODEL.Email).Select(x => x.UTILIZADORES_ID).SingleOrDefault();
                    // Get user login name
                    string username = databaseManager.UTILIZADORES.Where(x => x.ID == userid).Select(x => x.LOGIN).SingleOrDefault();

                    MODEL.Status = 1; // Token valido
                    MODEL.TOKENID = tokeninfo[0].ID;
                    MODEL.TOKEN = token;
                    MODEL.Login = username;
                }
                else
                {
                    MODEL.Status = 2; // Token expirado
                    message = "Atenção: O link que recebeu no seu email expirou ou você já pode tê-lo usado!";
                }
            }
            else
            {
                MODEL.Status = 0; // Token invalido
                message = "Atenção: link de recuperação de senha inválido ou você já pode tê-lo usado!";
            }
            ViewBag.Message = message;*/
            return View(MODEL);
        }


        // POST: /Account/PasswordRecoveryToken
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordRecoveryToken(PasswordResetTokenViewModel MODEL)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            /*
            string message = string.Empty;
            string token = MODEL.TOKEN.Replace(" ", "+");
            int tokenid = Configs.TOKENS[0]; // Recuperar senha 
            var tokeninfo = databaseManager.GRL_TOKENS.Where(a => a.TOKEN == token && a.C_TOKENS_TIPOS_ID == tokenid && a.DATA_REMOCAO == null).ToList();
            if (tokeninfo.Count > 0)
            {
                if (!ModelState.IsValid)
                {
                    string errors = string.Empty;
                    //var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                    ModelState.Values.SelectMany(v => v.Errors).ToList().ForEach(x => errors = x.ErrorMessage + "\n");
                    return Json(new { result = false, error = errors });
                }

                // Get last timestamp from token
                var DateAfter = databaseManager.GRL_TOKENS.Where(a => a.TOKEN == token && a.C_TOKENS_TIPOS_ID == tokenid && a.DATA_REMOCAO == null).OrderByDescending(x => x.ID).Select(x => x.DATA).First().AddMinutes(Convert.ToDouble(Configs.SEC_SENHA_RECU_LIMITE_EMAIL));
                TimeSpan ts = DateAfter - DateTime.Now;
                var minutes = ts.Minutes + 1;
                if (DateAfter > DateTime.Now)
                {
                    // Token valido

                    int userid = databaseManager.PES_PESSOAS.Join(databaseManager.PES_CONTACTOS,
                             x => x.ID,
                             y => y.PES_PESSOAS_ID,
                            (x, y) => new { y.EMAIL, x.UTILIZADORES_ID }).Where(x => x.EMAIL == MODEL.Email).Select(x => x.UTILIZADORES_ID).SingleOrDefault();

                    string pesname = databaseManager.PES_PESSOAS.Join(databaseManager.PES_CONTACTOS,
                             x => x.ID,
                             y => y.PES_PESSOAS_ID,
                            (x, y) => new { y.EMAIL, x.NOME }).Where(x => x.EMAIL == MODEL.Email).Select(x => x.NOME).SingleOrDefault();

                    // Create Salted Password
                    var Salt = Crypto.GenerateSalt(64);
                    var Password = Crypto.Hash(MODEL.Password.Trim() + Salt);
                    // Remove whitespaces and parse datetime strings //TrimStart() //Trim()
                    var update = databaseManager.SP_UTILIZADORES_ENT_UTILIZADORES(userid, null, null, null, null, null, null, Password, Salt, null, null, null, null, null, null, null, null, null, null, 1, Convert.ToChar('P').ToString()).ToArray();
                    // Get UserId
                    int Id = int.Parse(update[0].ID.ToString());
                    if (Id > 0)
                    {
                        // Remove token
                        var delete = databaseManager.SP_GRL_ENT_TOKENS(MODEL.TOKENID, null, null, null, null, null, null, "DT").ToList();
                        // Send Email
                        string url = "https://unipiaget-angola.org/api/redirect?login=" + "&campus=" + Configs.INST_INSTITUICAO_SIGLA; ;
                        Mailer.SendEmailMVC(4, MODEL.Email, pesname, MODEL.Password, url, null, null); // Email template - 4

                        // Try Catch
                        if (!string.IsNullOrEmpty(ExportEmail.StatusReport.result) && ExportEmail.StatusReport.result != "Success")
                            return Json(new { result = false, error = ExportEmail.StatusReport.result });
                    }
                }
                else
                {
                    return Json(new { result = false, error = "Atenção: O link que recebeu no seu email expirou ou você já pode tê-lo usado!" });
                }
            }
            else
            {
                return Json(new { result = false, error = "Atenção: link de recuperação de senha inválido!" });
            }
            */
            return Json(new { result = true, success = "Senha atualizada com successo, deve iniciar a sua sessão!", resetForm = true });
        }



        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Verification.    
            if (ModelState.IsValid)
            {
                // Initialization. 
                var loginInfo = databaseManager.UTILIZADORES.Where(a => a.LOGIN == model.Email && a.DATA_REMOCAO == null).ToList();
                // Verification.  
                if (loginInfo != null && loginInfo.Count() > 0)
                {
                    // Initialization.
                    var logindetails = loginInfo.First();

                    if (logindetails.VALIDA == false)
                    {
                        // Invalid Account
                        ModelState.AddModelError(string.Empty, "Por favor precisa validar a sua conta!");
                        return View(model);
                    }
                    if (logindetails.ACTIVO == false)
                    {
                        // Inactive Account
                        ModelState.AddModelError(string.Empty, "Conta de utilizador encontra-se inactiva!");
                        return View(model);
                    }
                    if (checkbrute(logindetails.ID))
                    {
                        // Max tent password tryouts
                        var DateAfter = databaseManager.UTILIZADORES_LOGIN_PASSWORD_TENT.Where(a => a.UTILIZADORES_ID == logindetails.ID).OrderByDescending(x => x.ID).Select(x => x.DATA).First().AddMinutes(Convert.ToDouble(Configs.SEC_SENHA_TENT_BLOQUEIO_TEMPO));
                        TimeSpan ts = DateAfter - DateTime.Now;
                        var minutes = ts.Minutes + 1;
                        ModelState.AddModelError(string.Empty, "Tentativa de início de sessão excedida, por favor tente novamente em " + minutes + " minuto(s)!");
                    }
                    else if (string.Compare(Crypto.Hash(model.Password + logindetails.SALT), logindetails.SENHA_PASSWORD) == 0)
                    {
                        // Fetch Groups
                        var grupos = databaseManager.SP_UTILIZADORES_LOGIN(logindetails.ID, null, null, null, null, null, null, null, null, Convert.ToChar('G').ToString()).ToArray();
                        // Fetch SubGroups
                        //var subgrupos = databaseManager.SP_UTILIZADORES_LOGIN(logindetails.ID, null, null, null, null, null, null, null, null, Convert.ToChar('S').ToString()).ToArray();
                        // Fetch Atoms
                        var atomos = databaseManager.SP_UTILIZADORES_LOGIN(logindetails.ID, null, null, null, null, null, null, null, null, Convert.ToChar('A').ToString()).ToArray();
                        // Convert them to list
                        List<int> lst_grupos = grupos.OfType<int>().ToList(); // this isn't going to be fast.
                        List<int> lst_subgrupos = null;//subgrupos.OfType<int>().ToList(); // this isn't going to be fast.
                        List<int> lst_atomos = atomos.OfType<int>().ToList(); // this isn't going to be fast.
                        // Fetch User Details
                        var ProfilePhoto = databaseManager.PES_PESSOAS.Where(x => x.UTILIZADORES_ID == logindetails.ID).Select(x => x.FOTOGRAFIA).SingleOrDefault();
                        // SignInUser   
                        this.SignInUser(logindetails.LOGIN, logindetails.ID.ToString(), true, lst_grupos, lst_subgrupos, lst_atomos, logindetails.ID, ProfilePhoto, returnUrl);
                        // Redirect if Needed    
                        return this.RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        // Invalid Password
                        ModelState.AddModelError(string.Empty, "Dados de credenciais de acesso inválidos!");
                        // Log Password Failure
                        LogSignIn(logindetails.ID, "P");
                    }
                }
                else
                {
                    // Invalid Username
                    ModelState.AddModelError(string.Empty, "Dados de credenciais de acesso inválidos!");
                }
            }
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        // Private Methods
        private void SignInUser(string username, string id, bool isPersistent, List<int> grupos, List<int> subgrupos, List<int> atomos, int UserId, string ProfilePhoto, string returnUrl)
        {
            // Initialization.    
            var claims = new List<Claim>();
            try
            {
                foreach (var g in grupos)
                {
                    claims.Add(new Claim(ClaimTypes.PrimaryGroupSid, g.ToString()));
                }
                //foreach (var s in subgrupos)
                //{
                //    claims.Add(new Claim(ClaimTypes.GroupSid, s.ToString()));
                //}
                foreach (var a in atomos)
                {
                    claims.Add(new Claim(ClaimTypes.Role, a.ToString()));
                }

                claims.Add(new Claim(ClaimTypes.Name, username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, id));
                if (!string.IsNullOrEmpty(ProfilePhoto)) claims.Add(new Claim(ClaimTypes.UserData, ProfilePhoto));
                var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                // Sign In.    
                authenticationManager.SignIn(new AuthenticationProperties()
                {
                    IsPersistent = isPersistent,
                    ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddMinutes(Convert.ToDouble(Configs.SEC_SESSAO_TIMEOUT_TEMPO))) //Session Expiration time?
                }, claimIdenties);
                // Log SigIn Activity
                LogSignIn(UserId, "L");
            }
            catch (Exception ex)
            {
                // Info    
                throw ex;
            }
        }
        // SiginIn Logs
        private void LogSignIn(int UserId, string logtype)
        {
            // Get IpAddress 
            string IpAddress = GetIPAddress();
            //GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            // Get MacAddress 
            string MacAddress = GetClientMAC(GetIPAddress());
            // Get HostName 
            // FIX THIS BENGUELA CAMPUS NETWORK NOT WORKING
            string HostName = GetHostName();
            // Get Lat and Long
            decimal Lat = 0;
            decimal Long = 0;
            // Get ModuleId
            int ModuleId = 1;//Convert.ToInt32(Configs.INST_MDL_ADM_VLRID_MODULO_ADM);
            // Get Url
            string Url = HttpContext.Request.Url.AbsoluteUri;
            // Get Device name
            string DeviceName = HttpContext.Request.UserAgent.ToLower();
            // Log Data to Database
            var log = databaseManager.SP_UTILIZADORES_LOGIN(UserId, ModuleId, IpAddress, MacAddress, HostName, DeviceName, Lat, Long, Url, Convert.ToChar(logtype).ToString()).ToArray();
        }
        // GET: Hostname
        public static string GetHostMachine()
        {
            return Environment.MachineName;
        }
        // GET: Network IP Addresses
        public string GetIPAddress()
        {
            string IpAddress = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(IpAddress))
            {
                string[] addresses = IpAddress.Split(',');
                if (addresses.Length != 0)
                {
                    IpAddress = addresses[0];
                }
            }
            else
            {
                IpAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            }

            // Remove any whitespace
            IpAddress = IpAddress.Trim();
            return IpAddress;
        }
        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);
        // GET: Network Interface Card MAC Address
        public static string GetClientMAC(string strClientIP)
        {
            string mac_dest = "";
            try
            {
                Int32 ldest = inet_addr(strClientIP);
                Int32 lhost = inet_addr("");
                Int64 macinfo = new Int64();
                Int32 len = 6;
                int res = SendARP(ldest, 0, ref macinfo, ref len);
                string mac_src = macinfo.ToString("X");

                while (mac_src.Length < 12)
                {
                    mac_src = mac_src.Insert(0, "0");
                }

                for (int i = 0; i < 11; i++)
                {
                    if (0 == (i % 2))
                    {
                        if (i == 10)
                        {
                            mac_dest = mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                        else
                        {
                            mac_dest = "-" + mac_dest.Insert(0, mac_src.Substring(i, 2));
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("L?i " + err.Message);
            }
            return mac_dest;
        }
        // Check Local or Public IP Address
        public static bool IsLanIP(IPAddress address)
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var iface in interfaces)
            {
                var properties = iface.GetIPProperties();
                foreach (var ifAddr in properties.UnicastAddresses)
                {
                    if (ifAddr.IPv4Mask != null &&
                        ifAddr.Address.AddressFamily == AddressFamily.InterNetwork &&
                        CheckMask(ifAddr.Address, ifAddr.IPv4Mask, address))
                        return true;
                }
            }
            return false;
        }
        private static bool CheckMask(IPAddress address, IPAddress mask, IPAddress target)
        {
            if (mask == null)
                return false;

            var ba = address.GetAddressBytes();
            var bm = mask.GetAddressBytes();
            var bb = target.GetAddressBytes();

            if (ba.Length != bm.Length || bm.Length != bb.Length)
                return false;

            for (var i = 0; i < ba.Length; i++)
            {
                int m = bm[i];

                int a = ba[i] & m;
                int b = bb[i] & m;

                if (a != b)
                    return false;
            }

            return true;
        }
        // GetHostName based on LAN or public
        public string GetHostName()
        {
            string Host = String.Empty;

            try
            {
                if (Configs.INST_INSTITUICAO_SIGLA == "UJPA")
                    Host = AccountController.IsLanIP(System.Net.IPAddress.Parse(GetIPAddress()))
                         ? Dns.GetHostEntry(Request.UserHostAddress).HostName.ToString()
                         : AccountController.GetHostMachine();
                else
                    Host = AccountController.GetHostMachine();
            }
            catch (Exception e)
            {
                //
            }

            return Host;
        }
        // Verify Bruteforce password
        private bool checkbrute(int userid)
        {
            bool exceed = false;
            // Get timestamp
            var DateAfter = DateTime.Now.AddMinutes(-Convert.ToDouble(Configs.SEC_SENHA_TENT_BLOQUEIO_TEMPO));
            // Password tryout counts
            var tryouts = databaseManager.UTILIZADORES_LOGIN_PASSWORD_TENT.Where(x => x.UTILIZADORES_ID == userid && x.DATA > DateAfter).Count();

            if (tryouts >= Configs.SEC_SENHA_TENT_BLOQUEIO)
            {
                exceed = true;
            }

            return exceed;
        }
        // Helpers       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                /*  if (_userManager != null)
                  {
                      _userManager.Dispose();
                      _userManager = null;
                  }

                  if (_signInManager != null)
                  {
                      _signInManager.Dispose();
                      _signInManager = null;
                  }*/
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}