#region using
using Ninject;
using RpR.ResponseEngines.Infrastructure.Infrastructure;
using System;
using System.Security.Principal;
using System.Web; 
#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public class ResponseEngine 
    {
        #region Fields&Props
        private readonly IResponse _response;
        private readonly HttpContext _context;
        private readonly bool _isAjax;
        public IResponse Response { get { return _response; } } 
        public HttpContext HttpContext { get { return _context; } }
        public HttpResponse HttpResponse { get { return _context.Response; } }
        public IPrincipal CurrentUser { get { return _context.User; } }
        public bool IsAuthenticated { get { return _context.User.Identity.IsAuthenticated; } }
        public bool IsAjax { get { return _isAjax; } }
        public string Status { get; set; }
        public int StatusCode { get; set; } 
        #endregion

        #region ContentType
        public string ContentType
        {
            get { return _context.Response.ContentType; }
            set { _context.Response.ContentType = value; }
        }
        #endregion

        #region .ctors
        public ResponseEngine()
            : this(DependencyResolution.Kernel.Get<IResponse>())
        { }

        public ResponseEngine(IResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("IResponse is null", (Exception)null);
            _context = HttpContext.Current;
            if (_context == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            _response = response;
            _isAjax = _context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            Status = "200 OK";
            StatusCode = 200;
            ContentType = "text/html";
        } 
        #endregion

        #region GetDefaultResponse
        public void GetDefaultResponse(dynamic model = null)
        {
            string requestTarget = _context.Request.Url.AbsolutePath
                .Substring(_context.Request.Url.AbsolutePath.LastIndexOf("/") + 1);
            HttpResponse.Write(_response.GetByRoutes(requestTarget, (string)null));
        } 
        #endregion
    }
}