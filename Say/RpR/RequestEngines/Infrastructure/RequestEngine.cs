using RpR.ResponseEngines.Infrastructure;
using System;
using System.Security.Principal;
using System.Web;

namespace RpR.RequestEngines.Infrastructure
{
    public class RequestEngine
    {
        #region Fields&Props
        private readonly HttpContext _context;
        public HttpContext HttpContext { get { return _context; } }
        public IPrincipal CurrentUser { get { return _context.User; } }
        public bool IsAuthenticated { get { return _context.User.Identity.IsAuthenticated; } } 
        #endregion

        #region .ctor
        public RequestEngine()
        {
            _context = HttpContext.Current;
            if (_context == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
        }
        #endregion

        #region IsInRole
        public bool IsInRole(string role)
        {
            if (_context.User != null)
                return _context.User.IsInRole(role);
            return false;
        }
        #endregion

        #region GetDefault
        public void GetDefault()
        {
            ResponseEngine responseEngine = new ResponseEngine();
            responseEngine.GetDefaultResponse();
        } 
        #endregion
    }
}