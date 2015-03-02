using SimpleWebApp.RequestResults.Strategies.Interfasces;
using System;
using System.Web;

namespace SimpleWebApp.RequestResults.Strategies
{
    public class DefaultStrategy : IToDoIt
    {
        private readonly IResponse _response;
        public DefaultStrategy(IResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("IResponse is null", (Exception)null);
            _response = response;
            Status = "200 OK";
            StatusCode = 200;
        }
        public string Status { get; set; }
        public int StatusCode { get; set; }

        public void ToDoIt(HttpContext context, Routes? target, bool isAjax = true)
        {
            if (context == null) return;
            context.Response.StatusCode = StatusCode;
            context.Response.Status = Status;
            string response;
            if (isAjax && (target != null))
            {
                response = _response.Get((Routes)target);
                if (response != null) context.Response.Write(response);
            }
            else
            {
                if (context.User.Identity.IsAuthenticated)
                    response = _response.Get(Routes.Index, target);
                else
                    response = _response.Get(Routes.Default, target);
                if (response != null) context.Response.Write(response);
            }
        }
    }
}