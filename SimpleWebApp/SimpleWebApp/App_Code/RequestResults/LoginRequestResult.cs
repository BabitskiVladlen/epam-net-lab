using SimpleWebApp.RequestResults.Strategies;
using SimpleWebApp.RequestResults.Strategies.Interfasces;
using SimpleWebApp.Security;
using System;
using System.IO;
using System.Web;

namespace SimpleWebApp.RequestResults
{
    public class LoginRequestResult : RequestResult
    {
        public LoginRequestResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            IResponse response = new Response();
            IToDoIt notFound = new DefaultStrategy(response) { Status = "404 Not Found", StatusCode = 404 };
            IToDoIt partial = new DefaultStrategy(response) { Status = "206 Partial Content", StatusCode = 206 };
            IToDoIt unauth = new DefaultStrategy(response) { Status = "401 Unauthorized", StatusCode = 401 };

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                notFound.ToDoIt(HttpContext, Routes.NotFound,
                    HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
            }
            else if (HttpContext.Request.HttpMethod == "GET")
            {
                partial.ToDoIt(HttpContext, Routes.Login,
                    HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
            }
            else if (HttpContext.Request.HttpMethod == "POST")
            {
                string username = HttpContext.Request["username"];
                string password = HttpContext.Request["password"];
                if (String.IsNullOrEmpty(username) || String.IsNullOrWhiteSpace(username) ||
                    (String.IsNullOrEmpty(password) || String.IsNullOrWhiteSpace(password)))
                {
                    unauth.ToDoIt(HttpContext, Routes.InvalidLogin,
                        HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
                    return;
                }
                User user = Authentication.Login(username, password);
                if (user == null)
                    unauth.ToDoIt(HttpContext, Routes.InvalidLogin,
                        HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
                else partial.ToDoIt(HttpContext, null,
                    HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest");
            }
        }
    }
}