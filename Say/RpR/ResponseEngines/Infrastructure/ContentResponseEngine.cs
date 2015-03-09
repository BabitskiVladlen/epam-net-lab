#region using
using RpR.RequestEngines.Infrastructure;
using System;
using System.Collections.Generic; 
#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public abstract class ContentResponseEngine : ResponseEngine
    {
        #region Fields&Props
        private readonly List<string> _info = new List<string>();
        public string Content { get; protected set; }
        public dynamic Model { get; set; }
        public string ContentType { get; set; }
        public List<string> Info { get { return _info; } }
        #endregion

        #region .ctors
        public ContentResponseEngine(RequestEngine requestEngine,
            IResponse response = null, IResponseStrategies responseStrategies = null)
            : base(requestEngine, response, responseStrategies)
        { } 
        #endregion

        #region GetResponse
        public override sealed void GetResponse()
        {
            if (HttpContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);

            PrepareContent();
            if (String.IsNullOrEmpty(Content) || String.IsNullOrWhiteSpace(Content))
                throw new InvalidOperationException("Content is null or empty", (Exception)null);
            if (!String.IsNullOrEmpty(ContentType) || !String.IsNullOrWhiteSpace(ContentType))
                Response.ContentType = ContentType;
            Response.Send(Content);
        } 
        #endregion

        #region PrepareContent
        public abstract void PrepareContent();  
        #endregion
    }
}