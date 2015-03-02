using System;
using System.IO;
using System.Web;

namespace SimpleWebApp.ResponseResults
{
    public class DefaultResponseResult : ResponseResult
    {
        public DefaultResponseResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            try
            {
                using (StreamReader stream = new StreamReader(BaseDirectory + "Content/" + RequestTarget))
                {
                    HttpContext.Response.Write(stream.ReadToEnd());
                }
            }
            catch (Exception exc)
            {
                LogMessage.Add(exc, Level.Error);
            }    
        }
    }
}