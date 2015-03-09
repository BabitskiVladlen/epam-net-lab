#region using
using BLL.Security;
using BLL.Security.Infrastructure;
using Ninject;
using System;
using System.Threading;
using System.Web;

#endregion

namespace RpR.HttpModules
{
    public class AuthHttpModule : IHttpModule
    {
        #region Fields&Props
        private IAppContext _appContext; 
        #endregion

        #region Init
        public void Init(HttpApplication context)
        {
            _appContext = DependencyResolution.Kernel.Get<IAppContext>();
            context.AuthenticateRequest += Authenticate;
            context.EndRequest += EndRequestHandler;
        } 
        #endregion

        #region Authenticate
        private void Authenticate(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext _httpContext = app.Context;
            _httpContext.User = Thread.CurrentPrincipal = _appContext.User;
        } 
        #endregion

        #region EndRequestHandler
        private void EndRequestHandler(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext _httpContext = app.Context;
            if (_httpContext.User.Identity.IsAuthenticated)
                Cookie.Create(_httpContext.User.Identity.Name, "say_rpr_cookie");
            _httpContext.User = Thread.CurrentPrincipal = _appContext.User = null;
        } 
        #endregion

        #region Dispose
        public void Dispose() { } 
        #endregion
    }
}