#region using
using Ninject;
using RpR.RequestEngines.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public abstract class ResponseEngine 
    {
        #region Fields&Props
        private readonly RequestEngine _requestEngine;
        public RequestEngine RequestEngine { get { return _requestEngine; } }
        public HttpContext HttpContext { get; private set; }
        public IPrincipal User { get { return _requestEngine.User; } }
        public bool IsAuthenticated { get { return _requestEngine.IsAuthenticated; } }
        public IResponse Response { get; private set; }
        public IResponseStrategies ResponseStrategies { get; private set; }
        public Dictionary<string, dynamic> Stash { get; private set; }
        public bool IsAjax { get; set; }
        #endregion

        #region .ctors
        public ResponseEngine(RequestEngine requestEngine,
            IResponse response = null, IResponseStrategies responseStrategies = null)
        {
            if (requestEngine == null)
                throw new ArgumentNullException("Requesr engine is null", (Exception)null);

            _requestEngine = requestEngine;

            HttpContext = HttpContext.Current;
            if (HttpContext != null)
            {

                IsAjax = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";

                var state = HttpContext.Cache["redirection_state"];
                if (state != null)
                {
                    Stash = state as Dictionary<string, dynamic>;
                    HttpContext.Cache.Remove("redirection_state");
                }
                if (Stash == null)
                    Stash = new Dictionary<string, dynamic>();
            }

            Response = response ?? DependencyResolution.Kernel.Get<IResponse>();
            ResponseStrategies = responseStrategies ?? DependencyResolution.Kernel.Get<IResponseStrategies>();
        } 
        #endregion

        #region GetResponse
        public abstract void GetResponse(); 
        #endregion

        #region GetDefaultResponse
        public static void GetDefaultResponse(dynamic model = null)
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            string requestTarget = context.Request.Url.AbsolutePath
                .Substring(context.Request.Url.AbsolutePath.LastIndexOf("/") + 1);
            context.Response.Write(new ResponseStrategies().GetByRoutes(requestTarget, (string)null));
        } 
        #endregion

        #region PropsToDictionary
        public Dictionary<string, string> PropsToDictionary(dynamic model)
        {
            if (model == null)
                throw new ArgumentNullException("Nodel is null", (Exception)null);

            Dictionary<string, string> placesValues = new Dictionary<string, string>();
            var props = ((PropertyInfo[])(model.GetType().GetProperties())).Where(p => p.CanRead);
            string value;
            foreach (var p in props)
            {
                value = p.GetValue(model) != null ? p.GetValue(model).ToString() : String.Empty;
                placesValues.Add(p.Name, value);
            }
            return placesValues;
        }
        #endregion

        #region IsInRole
        public bool IsInRole(string role)
        {
            return _requestEngine.IsInRole(role);
        }
        #endregion
    }
}