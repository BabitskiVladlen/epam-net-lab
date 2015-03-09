#region using
using System;
using System.Net;
using System.Web; 
#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public class Response : IResponse
    {
        #region Fields&Props
        private readonly HttpContext _context;
        public string ContentType { get; set; }
        public HttpStatusCode StatusCode { get; set; } 
        #endregion

        #region .ctors
        public Response()
        {
            _context = HttpContext.Current;
            StatusCode = HttpStatusCode.OK;
            ContentType = "text/html";
        } 
        #endregion

        #region Send(string)
        public void Send(string response)
        {
            if (_context == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            if (String.IsNullOrEmpty(response) || String.IsNullOrWhiteSpace(response))
                throw new ArgumentNullException("Response is null or empty", (Exception)null);

            _context.Response.Status = (int)StatusCode + " " + StatusCode.ToString();
            _context.Response.StatusCode = (int)StatusCode;
            if (!String.IsNullOrEmpty(ContentType) || !String.IsNullOrWhiteSpace(ContentType))
                _context.Response.ContentType = ContentType;
            _context.Response.Write(response);
        }
        #endregion

        #region Send(byte[])
        public void Send(byte[] response)
        {
            if (_context == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            if ((response == null) || (response.Length == 0))
                throw new ArgumentNullException("Response is null or empty", (Exception)null);

            _context.Response.Status = (int)StatusCode + " " + StatusCode.ToString();
            _context.Response.StatusCode = (int)StatusCode;
            if (!String.IsNullOrEmpty(ContentType) || !String.IsNullOrWhiteSpace(ContentType))
                _context.Response.ContentType = ContentType;
            _context.Response.BinaryWrite(response);
        }
        #endregion

        #region Redirect
        public void Redirect(string url)
        {
            if (String.IsNullOrEmpty(url) || String.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("Url is null or empty", (Exception)null);

            if (_context.Response.IsClientConnected)
                _context.Response.Redirect(url);
            else _context.Response.End();
        } 
        #endregion

        #region NoContent
        public void NoContent()
        {
            StatusCode = HttpStatusCode.NoContent;
            _context.Response.Status = (int)StatusCode + " " + StatusCode.ToString();
            _context.Response.StatusCode = (int)StatusCode;
        } 
        #endregion
    }
}