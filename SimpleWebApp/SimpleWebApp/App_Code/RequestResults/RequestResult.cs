using System;
using System.IO;
using System.Web;

namespace SimpleWebApp.RequestResults
{
    public abstract class RequestResult
    {
        private readonly string _requestTarget;
        private readonly string _baseDirectory;
        private readonly HttpContext _context;
        public string RequestTarget { get { return _requestTarget;  } }
        public string BaseDirectory { get { return _baseDirectory; } }
        public HttpContext HttpContext { get { return _context; } }
        public RequestResult()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }
        public RequestResult(HttpContext context) : this()
        {
            if (context == null) throw new ArgumentNullException("Context is null");
            context.Response.ContentType = "text/html";
            _requestTarget = context.Request.Url.AbsolutePath
                .Substring(context.Request.Url.AbsolutePath.LastIndexOf("/") + 1);
            _context = context;
        }

        public abstract void GetResponse();
    }
}