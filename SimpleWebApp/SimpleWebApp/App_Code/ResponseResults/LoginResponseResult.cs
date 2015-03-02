using SimpleWebApp.Security;
using System;
using System.IO;
using System.Web;

namespace SimpleWebApp.ResponseResults
{
    public class LoginResponseResult : ResponseResult
    {
        public LoginResponseResult(HttpContext context) : base(context) { }
        public override void GetResponse()
        {
            if (HttpContext.Request.HttpMethod == "GET")
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        HttpContext.Response.StatusCode = 206;
                        HttpContext.Response.Status = "206 Partial Content";
                        GetResponse("notFound.myhtm");
                    }
                    else
                        GetResponse("index.myhtm");
                    return;
                }
                if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                {
                    GetResponse("default.myhtm");
                    return;
                }
                HttpContext.Response.StatusCode = 206;
                HttpContext.Response.Status = "206 Partial Content";
                GetResponse("login.myhtm");
            }
            else if (HttpContext.Request.HttpMethod == "POST")
            {
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        GetResponse("notFound.myhtm");
                    else GetResponse("index.myhtm");
                    return;
                }
                string username = HttpContext.Request["username"];
                string password = HttpContext.Request["password"];
                if (String.IsNullOrEmpty(username) || String.IsNullOrWhiteSpace(username))
                {
                    HttpContext.Response.StatusCode = 401;
                    HttpContext.Response.Status = "401 Unauthorized";
                    GetResponse("login.myhtm");
                    return;
                }
                if (String.IsNullOrEmpty(password) || String.IsNullOrWhiteSpace(password))
                {
                    ValidateUsername();
                    return;
                }

                User user = Authentication.Login(username, password);
                if (user == null)
                {
                    HttpContext.Response.StatusCode = 401;
                    HttpContext.Response.Status = "401 Unauthorized";
                    GetResponse("login.myhtm");
                }
                else
                {
                    HttpContext.Response.StatusCode = 206;
                    HttpContext.Response.Status = "206 Partial Content";
                }
            }
        }

        #region ValidateUsername
        private void ValidateUsername()
        {
            try
            {
                string username = HttpContext.Request["username"];
                if (!Users.AllUsers.Exists(u => String.Equals(u.UserName, username, StringComparison.InvariantCultureIgnoreCase)))
                    HttpContext.Response.Write("Nonexistent user...");
            }
            catch (Exception exc)
            {
                LogMessage.Add(exc, Level.Error);
            }
        } 
        #endregion
    }
}