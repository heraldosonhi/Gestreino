using System;
using System.Web;

namespace Gestreino.Classes
{
    public class Cookies
    {
        public static string COOKIES_SIDEBAR_MENU_COLLAPSE = "gestreino_sidemenucollapse";
        public static string COOKIES_GESTREINO_AVALIADO = "gestreino_atletaavaliado";

        public void WriteCookie(string entity, string value)
        {
            HttpCookie nameCookie = new HttpCookie(entity);

            nameCookie.Value= value;
            nameCookie.Expires = DateTime.Now.AddDays(180);
            HttpContext.Current.Response.Cookies.Add(nameCookie);
        }
        public static string ReadCookie(string entity)
        {
            HttpCookie nameCookie = HttpContext.Current.Request.Cookies[entity];
            string name = nameCookie != null ? nameCookie.Value : "";
            return name;
        }
        public void DeleteCookie(string entity)
        {
            HttpCookie nameCookie = HttpContext.Current.Request.Cookies[entity];
            nameCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(nameCookie);
        }
    }
}