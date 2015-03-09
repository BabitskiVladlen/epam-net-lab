#region using
using RpR.RequestEngines.Infrastructure;
using System;
using System.Net; 
#endregion

namespace RpR.ResponseEngines.Infrastructure
{
    public class NotFoundResponseEngine : ContentResponseEngine
    {
        #region .ctors
        public NotFoundResponseEngine(RequestEngine requestEngine,
            IResponse response = null, IResponseStrategies responseStrategies = null)
            : base(requestEngine, response, responseStrategies)
        { } 
        #endregion

        #region PrepareContent
        public override void PrepareContent()
        {
            Response.StatusCode = HttpStatusCode.NotFound;
            try
            {
                Content = ResponseStrategies.GetByRoutes("notfound.rpr", (string)null);
            }
            catch
            {
                throw new InvalidOperationException("Page notfound.rpr doesn't exist", (Exception)null);
            }
        } 
        #endregion
    }
}