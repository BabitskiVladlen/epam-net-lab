using SimpleWebApp.RequestResults.Strategies;
using SimpleWebApp.RequestResults.Strategies.Interfasces;
using System.Web;

namespace SimpleWebApp.RequestResults
{
    public class IndexRequestResult : RequestResult
    {
        public IndexRequestResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            IResponse response = new Response();
            string respText;
            if (!HttpContext.User.Identity.IsAuthenticated)
                respText = response.Get(Routes.Default);
            else
                respText = response.Get(Routes.Index);
            if (respText != null) HttpContext.Response.Write(respText);
        }
    }
}