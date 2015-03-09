#region using
using RpR.RequestEngines.Infrastructure;
using System;

#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public class RedirectResponseEngine : ResponseEngine
    {
        #region Fields&Props
        public string Url { get; private set; }
        #endregion
        
        #region .ctors
        public RedirectResponseEngine(string url, RequestEngine requestEngine,
            IResponse response = null, IResponseStrategies responseStrategies = null)
            : base(requestEngine, response, responseStrategies)
        {
            if (String.IsNullOrEmpty(url) || String.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("Url is null or empty", (Exception)null);
            Url = url;
        } 
        #endregion

        #region GetResponse
        public override void GetResponse()
        {
            if (HttpContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);

            if (Stash.Count != 0)
                HttpContext.Cache["redirection_state"] = Stash;
            Response.Redirect(Url);
        } 
        #endregion
    }
}