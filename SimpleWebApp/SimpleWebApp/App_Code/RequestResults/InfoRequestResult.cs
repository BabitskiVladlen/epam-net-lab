using SimpleWebApp.RequestResults.Strategies;
using SimpleWebApp.RequestResults.Strategies.Interfasces;
using System.Web;

namespace SimpleWebApp.RequestResults
{
    public class InfoRequestResult : RequestResult
    {
        public InfoRequestResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            IResponse response = new Response();
            IToDoIt notFound = new DefaultStrategy(response) { Status = "404 Not Found", StatusCode = 404 };
            IToDoIt partial = new DefaultStrategy(response) { Status = "206 Partial Content", StatusCode = 206 };

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                partial.ToDoIt(HttpContext, Routes.Info,
                    HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
                return;
            }
            notFound.ToDoIt(HttpContext, Routes.NotFound,
                HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }
    }
}