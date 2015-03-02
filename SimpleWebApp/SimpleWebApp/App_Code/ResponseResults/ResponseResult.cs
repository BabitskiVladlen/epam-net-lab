using System;
using System.IO;
using System.Web;

namespace SimpleWebApp.ResponseResults
{
    public abstract class ResponseResult
    {
        private readonly string _requestTarget;
        private readonly string _baseDirectory;
        private readonly HttpContext _context;
        public string RequestTarget { get { return _requestTarget;  } }
        public string BaseDirectory { get { return _baseDirectory; } }
        public HttpContext HttpContext { get { return _context; } }
        public ResponseResult()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }
        public ResponseResult(HttpContext context) : this()
        {
            if (context == null) throw new ArgumentNullException("Context is null");
            context.Response.ContentType = "text/html";
            _requestTarget = context.Request.Url.AbsolutePath
                .Substring(context.Request.Url.AbsolutePath.LastIndexOf("/") + 1);
            _context = context;
        }

        public void GetResponse(string target)
        {
            if (String.IsNullOrEmpty(target) || String.IsNullOrWhiteSpace(target))
                throw new ArgumentNullException("Target is null or empty", (Exception)null);
            if (HttpContext == null) return;
            try
            {
                using (StreamReader stream = new StreamReader(BaseDirectory + "Content/" + target))
                {
                    HttpContext.Response.Write(stream.ReadToEnd());
                }
            }
            catch (Exception exc)
            {
                LogMessage.Add(exc, Level.Error);
            }
        }
        public abstract void GetResponse();
    }
}