using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Gestreino.Classes
{
    public class Cookies
    {
        //Static Vars
        public static string COOKIES_GA_CALENDAR_COLUMN_SIZE = "IPIAGET_gacalendarcolumnsize";
        public static string COOKIES_GA_REQUEST_INIT_VIEW = "IPIAGET_garequestinitview";
        public static string COOKIES_GA_EXAMENROLLMENT_INIT_VIEW = "IPIAGET_gaexamenrollmentinitview";
        public static string COOKIES_GA_ENROLLMENTS_INIT_VIEW = "IPIAGET_garenrollmentsinitview";


        public void WriteCookie(string entity, string value)
        {
            //Create a Cookie with a suitable Key.
            HttpCookie nameCookie = new HttpCookie(entity);

            //Set the Cookie value.
            //nameCookie.Values["Name"] = value;
            nameCookie.Value= value;

            //Set the Expiry date.
            nameCookie.Expires = DateTime.Now.AddDays(180);

            //Add the Cookie to Browser.
            HttpContext.Current.Response.Cookies.Add(nameCookie);
        }
        public static string ReadCookie(string entity)
        {
            //Fetch the Cookie using its Key.
            HttpCookie nameCookie = HttpContext.Current.Request.Cookies[entity];

            //If Cookie exists fetch its value.
            string name = nameCookie != null ? nameCookie.Value : "";

            //Return its value
            return name;
        }
        public void DeleteCookie(string entity)
        {
            //Fetch the Cookie using its Key.
            HttpCookie nameCookie = HttpContext.Current.Request.Cookies[entity];

            //Set the Expiry date to past date.
            nameCookie.Expires = DateTime.Now.AddDays(-1);

            //Update the Cookie in Browser.
            HttpContext.Current.Response.Cookies.Add(nameCookie);
        }
    }
}