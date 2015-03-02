using System;
using System.Web;

namespace SimpleWebApp.Security
{
    public class AuthHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Authenticate;
            context.EndRequest += EndRequestHandler;
        }

        private void Authenticate(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;
            Authentication.HttpContext = context;
            context.User = Authentication.CurrentUser;
        }

        private void EndRequestHandler(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;
            if (context.User.Identity.IsAuthenticated)
                Cookie.Create(context.User.Identity.Name, Authentication.CookieName, context);
            Authentication.CurrentUser = null;
        }

        public void Dispose() { }
    }
}