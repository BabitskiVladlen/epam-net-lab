using System;
using System.Collections.Generic;

namespace RpR.ResponseEngines.Infrastructure.Infrastructure
{
    public interface IResponse
    {
        string Title { get; set; }
        Tuple<IEnumerable<string>, string> Errors { get; set; }
        string GetContent(string content, string innerContent, string id = "main", bool isJson = false);
        string GetByRoutes(string target, string innerTarget, string id = "main", bool isJson = false);
        string RouteToContent(string content, string innerTarget, string id = "main", bool isJson = false);
        string ContentToRoute(string target, string innerContent, string id = "main", bool isJson = false);

    }
}
