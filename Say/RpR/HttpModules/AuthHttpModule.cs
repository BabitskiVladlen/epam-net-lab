using BLL.Security;
using BLL.Security.Infrastructure;
using Ninject;
using System;
using System.Threading;
using System.Web;

namespace RpR.HttpModules
{
    public class AuthHttpModule : IHttpModule
    {
        #region Init
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Authenticate;
            context.EndRequest += EndRequestHandler;
        } 
        #endregion

        #region Authenticate
        private void Authenticate(object source, EventArgs e)
        {
            IAuthentication authentication =
                DependencyResolution.Kernel.Get<IAuthentication>();
            HttpContext.Current.User = authentication.CurrentUser;
            Thread.CurrentPrincipal = authentication.CurrentUser;
        } 
        #endregion

        #region EndRequestHandler
        private void EndRequestHandler(object source, EventArgs e)
        {
            IAuthentication authentication =
                DependencyResolution.Kernel.Get<IAuthentication>();
            HttpContext context = HttpContext.Current;
            if (context.User.Identity.IsAuthenticated)
                Cookie.Create(context.User.Identity.Name, "say_rpr_cookie");
            authentication.CurrentUser = null;
            HttpContext.Current.User = null;
            Thread.CurrentPrincipal = null;
        } 
        #endregion

        public void Dispose() { }
    }
}