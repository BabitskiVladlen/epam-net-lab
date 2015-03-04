using System;
using System.Collections.Generic;
using System.Text;

namespace RpR.ResponseEngines.Infrastructure.Infrastructure
{
    public interface IResponse
    {
        string Title { get; set; }
        Tuple<string, IEnumerable<string>> Errors { get; set; } 
        void BindContent(StringBuilder content, Dictionary<string, string> placeValue);
        string GetContent(string content, string innerContent, string placement = "main", bool isJson = false);
        string GetByRoutes(string target, string innerTarget, string placement = "main", bool isJson = false);
        string RouteToContent(string content, string innerTarget, string placement = "main", bool isJson = false);
        string ContentToRoute(string target, string innerContent, string placement = "main", bool isJson = false);

    }
}
