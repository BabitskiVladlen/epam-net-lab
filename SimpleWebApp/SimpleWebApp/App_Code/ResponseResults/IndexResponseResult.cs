using System.Web;

namespace SimpleWebApp.ResponseResults
{
    public class IndexResponseResult : ResponseResult
    {
        public IndexResponseResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                GetResponse("default.myhtm");
                return;
            }
            GetResponse("index.myhtm");
        }
    }
}