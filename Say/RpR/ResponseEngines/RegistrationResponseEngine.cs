#region using
using RpR.RequestEngines.Infrastructure;
using RpR.ResponseEngines.Infrastructure;
using System.Collections.Generic;

#endregion

namespace RpR.ResponseEngines
{
    public class RegistrationResponseEngine : LayoutResponseEngine
    {
        #region .ctors
        public RegistrationResponseEngine(RequestEngine requestEngine,
            IResponse response = null, IResponseStrategies responseStrategies = null)
            : base(requestEngine, response, responseStrategies)
        { } 
        #endregion

        #region PrepareContent
        public override void PrepareContent()
        {
            if (Stash.Count != 0)
            {
                Model = Stash["model"];
                Info.Add(Stash["info"]);
            }

            Dictionary<string, string> placesValues = PropsToDictionary(Model);
            string target = IsAuthenticated ? "profile.rpr" : "registration.rpr";
            string content = ResponseStrategies.GetByRoutes(target, (string)null);
            content = ResponseStrategies.SetInfo(content, Info, "info");
            content = ResponseStrategies.BindContent(content, placesValues);
            if (IsAjax)
                ContentType = "application/json";
            else
                content = ResponseStrategies.GetContent(Layout, content, "main");
            Content = content;
        }
        #endregion
    }
}