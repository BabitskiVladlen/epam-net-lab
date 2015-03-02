using System;
using System.Web;

namespace RpR.RequestEngines.Infrastructure
{
    public abstract class RequestEngine
    {
        private readonly string _requestTarget;
        private readonly string _baseDirectory;
        private readonly HttpContext _context;
        public string RequestTarget { get { return _requestTarget;  } }
        public string BaseDirectory { get { return _baseDirectory; } }
        public HttpContext HttpContext { get { return _context; } }

        #region .ctor
        public RequestEngine()
        {
            _context = HttpContext.Current;
            if (_context == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _context.Response.ContentType = "text/html";
            _requestTarget = _context.Request.Url.AbsolutePath
                .Substring(_context.Request.Url.AbsolutePath.LastIndexOf("/") + 1);
        } 
        #endregion

        public abstract void GetResponse();
    }
}