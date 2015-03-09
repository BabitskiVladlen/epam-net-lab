#region using
using RpR.ResponseEngines.Infrastructure;
using System.Security.Principal;
using System.Web;
#endregion

namespace RpR.RequestEngines.Infrastructure
{
    public class RequestEngine
    {
        #region Fields&Props
        private IPrincipal _user;
        public HttpContext HttpContext { get; private set; }

        #region User
        public IPrincipal User
        {
            get
            {
                if (HttpContext != null)
                    return HttpContext.User;
                return _user;
            }
            set { _user = value; }
        } 
        #endregion

        #region IsAuthenticated
        public bool IsAuthenticated
        {
            get
            {
                if ((User != null) && (User.Identity != null))
                    return User.Identity.IsAuthenticated ;
                return false;
            }
        } 
        #endregion

        #endregion

        #region .ctors
        public RequestEngine()
        {
            HttpContext = HttpContext.Current;
        }
        #endregion

        #region IsInRole
        public bool IsInRole(string role)
        {
            return (User != null) ?  User.IsInRole(role) : false;
        }
        #endregion

        #region GetDefaultResponse
        public void GetDefault()
        {
            ResponseEngine.GetDefaultResponse();
        } 
        #endregion
    }
}