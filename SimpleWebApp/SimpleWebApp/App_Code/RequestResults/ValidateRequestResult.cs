using SimpleWebApp.Security;
using System;
using System.Web;

namespace SimpleWebApp.RequestResults
{
    public class ValidateRequestResult : RequestResult
    {
        public ValidateRequestResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            string username = HttpContext.Request["username"];
            if (!Users.AllUsers.Exists(u => String.Equals(u.UserName, username, StringComparison.InvariantCultureIgnoreCase)))
                HttpContext.Response.Write("Nonexistent user...");
        }
    }
}