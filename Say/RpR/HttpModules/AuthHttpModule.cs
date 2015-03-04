#region using
using BLL.Security;
using BLL.Security.Contexts;
using BLL.Security.Infrastructure;
using System;
using System.Threading;
using System.Web; 
#endregion

namespace RpR.HttpModules
{
    public class AuthHttpModule : IHttpModule
    {
        private HttpContext _httpContext;
        private IAppContext _appContext;

        #region Init
        public void Init(HttpApplication context)
        {
            _httpContext = HttpContext.Current;
            _appContext = new WebContext();
            context.AuthenticateRequest += Authenticate;
            context.EndRequest += EndRequestHandler;
        } 
        #endregion

        #region Authenticate
        private void Authenticate(object source, EventArgs e)
        {
            _httpContext.User = _appContext.User;
            Thread.CurrentPrincipal = _appContext.User;
        } 
        #endregion

        #region EndRequestHandler
        private void EndRequestHandler(object source, EventArgs e)
        {
            if (_httpContext.User.Identity.IsAuthenticated)
                Cookie.Create(_httpContext.User.Identity.Name, "say_rpr_cookie");
            _appContext.User = _httpContext.User = Thread.CurrentPrincipal = null;
        } 
        #endregion

        public void Dispose() { }
    }
}