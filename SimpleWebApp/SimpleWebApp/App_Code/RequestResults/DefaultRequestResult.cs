using System.IO;
using System.Web;

namespace SimpleWebApp.RequestResults
{
    public class DefaultRequestResult : RequestResult
    {
        public DefaultRequestResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            try
            {
                using (StreamReader stream = new StreamReader(BaseDirectory + "Content/" + RequestTarget))
                {
                    HttpContext.Response.Write(stream.ReadToEnd());
                }
            }
            catch (IOException exc)
            {
                LogMessage.Add(exc, Level.Error);
            }    
        }
    }
}