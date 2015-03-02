using BLL.Security.Infrastructure;
using System;
using System.Security.Principal;
using System.Web;

namespace BLL.Security
{
    class WebContext : IContext
    {
        private readonly HttpContext _httpContext;
        private readonly string _cookieName;

        #region .ctor
        public WebContext(string cookieName)
        {
            _httpContext = HttpContext.Current;
            if (_httpContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            _cookieName = cookieName ?? "say_rpr_cookie";
        }
        #endregion

        #region User
        public IPrincipal User
        {
            get
            {
                return _httpContext.User;
            }
            set
            {
                _httpContext.User = value;
            }
        } 
        #endregion

        #region GetUserData
        public string GetUserData()
        {
            var ticket = Cookie.Read(_cookieName, _httpContext);
            if (ticket == null) return null;
            return ticket.Name;
        } 
        #endregion

        #region SetUserData
        public void SetUserData(string data)
        {
            Cookie.Create(data, _cookieName, _httpContext);
        }
        #endregion

        #region DeleteUserData
        public void DeleteUserData()
        {
            Cookie.Delete(_cookieName, _httpContext);
        } 
        #endregion
    }
}
