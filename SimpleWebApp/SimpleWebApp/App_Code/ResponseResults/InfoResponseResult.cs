using System.Web;

namespace SimpleWebApp.ResponseResults
{
    public class InfoResponseResult : ResponseResult
    {
        public InfoResponseResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    HttpContext.Response.StatusCode = 404;
                    HttpContext.Response.Status = "404 Not Found";
                    GetResponse("notFound.myhtm");
                }
                else GetResponse("default.myhtm");
                return;
            }
            HttpContext.Response.StatusCode = 206;
            HttpContext.Response.Status = "206 Partial Content";
            GetResponse("info.myhtm");
        }
    }
}