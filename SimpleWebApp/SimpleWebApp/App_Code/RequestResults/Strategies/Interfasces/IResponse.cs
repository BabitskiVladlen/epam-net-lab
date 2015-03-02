using System.Web;

namespace SimpleWebApp.RequestResults.Strategies.Interfasces
{
    public interface IResponse
    {
        string Get(Routes target, Routes? innerTarget = null, string id = "main");
    }
}
