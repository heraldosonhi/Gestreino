﻿using System;
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
        public static string COOKIES_SIDEBAR_MENU_COLLAPSE = "gestreino_sidemenucollapse";
        public static string COOKIES_GESTREINO_AVALIADO = "gestreino_atletaavaliado";

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