#region using
using RpR.ResponseEngines.Infrastructure; 
#endregion

namespace RpR.RequestEngines.Infrastructure
{
    public class NotFoundRequestEngine : RequestEngine
    {
        #region Get
        public ResponseEngine Get()
        {
            return new NotFoundResponseEngine(this);
        } 
        #endregion
    }
}