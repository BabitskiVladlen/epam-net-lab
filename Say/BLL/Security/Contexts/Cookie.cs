﻿using System;
using System.Web;
using System.Web.Security;

namespace BLL.Security
{
    internal static class Cookie
    {
        #region Create
        public static void Create(string username, string cookieName,
    HttpContext context, bool isPersistent = true)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException("Username is null or empty", (Exception)null);
            if (String.IsNullOrEmpty(cookieName) || String.IsNullOrWhiteSpace(cookieName))
                throw new ArgumentNullException("CookieName is null or empty", (Exception)null);
            if (context == null)
                throw new ArgumentNullException("Context is null.", (Exception)null);

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                username,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                String.Empty,
                FormsAuthentication.FormsCookiePath);
            string secreticket = FormsAuthentication.Encrypt(ticket);
            HttpCookie AuthCookie = new HttpCookie(cookieName, secreticket);
            AuthCookie.Expires = DateTime.Now.Add(FormsAuthentication.Timeout);
            context.Response.Cookies.Set(AuthCookie);
        } 
        #endregion

        #region Read
        public static FormsAuthenticationTicket Read(string cookieName, HttpContext context)
        {
            if (String.IsNullOrEmpty(cookieName) || String.IsNullOrWhiteSpace(cookieName))
                throw new ArgumentNullException("CookieName is null or empty", (Exception)null);
            if (context == null)
                throw new ArgumentNullException("Context is null", (Exception)null);

            FormsAuthenticationTicket ticket = null;
            HttpCookie userCookie = context.Request.Cookies[cookieName];
            if ((userCookie != null) && !(String.IsNullOrEmpty(userCookie.Value)))
                ticket = FormsAuthentication.Decrypt(userCookie.Value);
            return ticket;
        } 
        #endregion

        #region Delete
        public static void Delete(string cookieName, HttpContext context)
        {
            if (context.Request.Cookies[cookieName] != null)
            {
                HttpCookie newCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1d) };
                context.Response.Cookies.Set(newCookie);
            }
        } 
        #endregion
    }
}