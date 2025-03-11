using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.EMMA;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Services.Description;
using Gestreino.Classes;
using DocumentFormat.OpenXml.Wordprocessing;
using JeanPiagetSGA;

namespace Gestreino.Classes
{
    public class ExportEmail
    {
        public static class StatusReport
        {
            public static string result { get; set; }
        }

        public void SendEmailMVC(int template, string to, string var1, string var2, string var3, string var4, string var5)
        {
            // Mailer Class

            // Define Template ID
            /*
             * 1 => Criação de Conta,
             * 2 => Alterar Senha de acesso,
             * 3 => Recuperacao da Senha de acesso
             * 4 => Senha de acesso atualizada com successo
             */

            var subject =  string.Empty;
            var body = string.Empty;
            var url = string.Empty;
            var urltitle = string.Empty;
            string content = string.Empty;

            if (template == 1)
            {
                 subject = "Bem vindo";
                 body = "Caríssimo (a) <b>" + var1 + "</b>, a sua conta foi gerada com successo! <br/> Segue em anexo as suas credencias de acesso: <br/> <b>Utilizador: </b> " + var2 + " <br/> <b>Senha: </b>" + var3;
                url = var4;
                 urltitle = "Iniciar Sessão";
            }
            if (template == 2)
            {
                subject = "Actualização da Senha de acesso";
                body = "Caríssimo (a) <b>" + var1 + "</b>, os seus credencias de acesso foram actualizados com successo! <br/> Segue em anexo a sua nova senha de acesso: " + var2 + "";
                url = var3;
                urltitle = "Iniciar Sessão";
            }
            if (template == 3)
            {
                subject = "Recuperar senha de acesso";
                body = "Caríssimo (a) <b>" + var1 + "</b> <br />&nbsp;<br /> Por favor visite o link anexado para efetuar a recuperação da senha de acesso. <br /> O link é valído por "+ Configs.SEC_SENHA_RECU_LIMITE_EMAIL + " minutos apenas. <br />&nbsp;<br /><small>Por favor ignorar este email se não requisitou este link</small><br/>";
                url = var3;
                urltitle = "Recuperar senha de acesso";
            }
            if (template == 4)
            {
                subject = "Senha de acesso atualizada com successo";
                body = "Caríssimo (a) <b>" + var1 + "</b> <br />&nbsp;<br /> A sua senha de acesso foi actualizada com sucesso!<br/>"; //A sua nova senha de acesso é: " + var2+"<br/>"
                url = var3;
                urltitle = "Iniciar Sessão";
            }

            using (System.IO.StreamReader reader = new System.IO.StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Views/Administration/EmailTemplate.htm")))
            {
                content = reader.ReadToEnd();
            }
            content = content.Replace("{UserName}", to);
            content = content.Replace("{Title}", subject);
            content = content.Replace("{Url}", url);
            content = content.Replace("{UrlTitle}", urltitle);
            content = content.Replace("{Description}", body);
            content = content.Replace("{inst_nome}", Configs.INST_INSTITUICAO_NOME);
            content = content.Replace("{inst_sigla}", Configs.INST_INSTITUICAO_SIGLA);

            MailMessage message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(Configs.NET_STMP_FROM);
            message.Subject = Configs.INST_INSTITUICAO_NOME +" - "+ subject;
            message.Body = content;
            message.IsBodyHtml = true;

            // Security check
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Disabled certificate for invalidation error
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);

            try
            {
                SmtpClient smtp = new SmtpClient();

                /*** 
                 * LEAVE EMPTY FOR WEBCONFIG DEFAULT SETTINGS
                 * ***/
                smtp.Host = Configs.NET_STMP_HOST;
                smtp.Port = Convert.ToInt32(Configs.NET_STMP_PORT);
                smtp.Credentials = new System.Net.NetworkCredential(Configs.NET_SMTP_USERNAME, Configs.NET_SMTP_SENHA);
                smtp.EnableSsl = true;
                smtp.Send(message);
                //await smtp.SendMailAsync(message);
                StatusReport.result = "Success";
            }
            catch (SmtpException ex)
            {
                StatusReport.result = ex.ToString();
            }
        }

        //Check
        private bool RemoteServerCertificateValidationCallback(object sender,
         System.Security.Cryptography.X509Certificates.X509Certificate certificate,
         System.Security.Cryptography.X509Certificates.X509Chain chain,
         System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //Console.WriteLine(certificate);
            return true;
        }


        public static async Task<string> SendSMSMVC(string text, string recipients, int? userid)
        {
            var responseData = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    // Send SMS /mimosms/v1/message/send?token=
                    // Create Contact /mimosms/v1/contact/add?token=
                    var sender = Configs.API_SMS_SENDER_ID;
                    var baseAddress = Configs.API_SMS_BASE + "/mimosms/v1/message/send?token=" + Configs.API_SMS_TOKEN;
                    client.DefaultRequestHeaders.Accept.Add(contentType);
                   
                     var data = new Dictionary<string, string>
                         {
                         { "sender",sender },
                             { "recipients", Converters.StripHTML(recipients) },
                             { "text", Converters.StripHTML(text) }
                         };


                    var jsonData = JsonConvert.SerializeObject(data);
                    var contentData = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(baseAddress, contentData);

                    if (response.IsSuccessStatusCode)
                    {
                        var stringData = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<object>(stringData);
                        responseData = "OK";
                    }
                    else
                        responseData = await response.Content.ReadAsStringAsync();
                    
                }
            }
            catch(HttpException i)
            {
                responseData = i.Message;
            }

            return responseData;
        }
      
       
    }
}