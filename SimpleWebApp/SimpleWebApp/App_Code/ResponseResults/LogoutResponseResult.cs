using SimpleWebApp.Security;
using System.Web;

namespace SimpleWebApp.ResponseResults
{
    public class LogoutResponseResult : ResponseResult
    {
        public LogoutResponseResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                Authentication.Logout();
            HttpContext.Response.StatusCode = 206;
            HttpContext.Response.Status = "206 Partial Content";
        }
    }
}