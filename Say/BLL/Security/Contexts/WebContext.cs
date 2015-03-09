#region using
using BLL.Infrastructure;
using BLL.Security.Infrastructure;
using BLL.Security.Principal;
using BLL.Services;
using System;
using System.Security.Principal;
using System.Threading;
using System.Web; 
#endregion

namespace BLL.Security.Contexts
{
    public class WebContext : IAppContext
    {
        #region Fields&Props
        private IPrincipal _user;
        private readonly IUserService _userService;
        private readonly HttpContext _httpContext;
        private readonly string _cookieName; 
        #endregion

        #region .ctors
        public WebContext(string cookieName = null)
            : this(new UserService(), cookieName)
        { }

        public WebContext(IUserService userService, string cookieName = null)
        {
            if (userService == null)
                throw new ArgumentNullException("Service is null", (Exception)null);
            _httpContext = HttpContext.Current;
            if (_httpContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            _cookieName = cookieName ?? "say_rpr_cookie";
            _userService = userService;
        }
        #endregion

        #region GetUserData
        public string GetUserData()
        {
            var ticket = Cookie.Read(_cookieName);
            if (ticket == null) return null;
            return ticket.Name;
        } 
        #endregion

        #region SetUserData
        public void SetUserData(string id)
        {
            if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException("Data is null or empty", (Exception)null);
            Cookie.Create(id, _cookieName);
            _user =  _httpContext.User = Thread.CurrentPrincipal = new DefaultPrincipal(_userService, id);
        }
        #endregion

        #region DeleteUserData
        public void DeleteUserData()
        {
            Cookie.Delete(_cookieName);
            _user = _httpContext.User = Thread.CurrentPrincipal =
                new DefaultPrincipal((IUserService)null, (string)null);
        } 
        #endregion

        #region User
        public IPrincipal User
        {
            get
            {
                if (_user == null)
                    _user = new DefaultPrincipal(_userService, GetUserData());
                return _user;
            }
            set { _user = value; }
        }
        #endregion
    }
}
