#region using
using System.Collections.Generic; 
#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public interface IResponseStrategies
    {
        string BindContent(string content, Dictionary<string, string> placesValues);
        string GetContent(string content, string innerContent, string placement = "main");
        string GetByRoutes(string target, string innerTarget, string placement = "main");
        string RouteToContent(string content, string innerTarget, string placement = "main");
        string ContentToRoute(string target, string innerContent, string placement = "main");
        string SetTitle(string content, string title);
        string SetInfo(string content, IEnumerable<string> errors, string placement);
        string JsonResult(object value);
    }
}
