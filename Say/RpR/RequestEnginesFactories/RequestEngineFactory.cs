#region using
using Ninject;
using RpR.Infrastructure;
using RpR.RequestEngines.Infrastructure;
using RpR.ResponseEngines.Infrastructure;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
#endregion

namespace RpR.RequestEngineFactories
{
    public class RequestEngineFactory : IRequestEngineFactory
    {
        #region Fields&Props
        private readonly IMethodInvoker _methodInvoker; 
        #endregion

        #region .ctors
        public RequestEngineFactory(IMethodInvoker methodInvoker = null)
        {
            _methodInvoker = methodInvoker ?? DependencyResolution.Kernel.Get<IMethodInvoker>();
        } 
        #endregion

        #region CreateRequestEngine
        public void CreateRequestEngine(string target)
        {
            object instance = GetRequestEngineInstance(target);
            if (instance == null) return;
            try
            {
                _methodInvoker.InvokeMethod(instance);
            }
            catch (Exception exc)
            {
                LogMessage.Add(exc, Level.Error);
                Error();
            } 
        } 
        #endregion

        #region GetRequestEngineInstance
        private object GetRequestEngineInstance(string target)
        {
            Type necessaryType = GetRequestEngineType(target);
            if (necessaryType == null) return null;

            var ctor = necessaryType.GetConstructor(new Type[0]);
            if (ctor == null)
            {
                LogMessage.Add("Public constructor without parameters of " +
                    necessaryType.Name.ToUpper(CultureInfo.InvariantCulture) + " doesn't exist", Level.Error);
                NotFound();
                return null;
            }

            return ctor.Invoke(null);
        } 
        #endregion

        #region GetRequestEngineType
        private Type GetRequestEngineType(string target)
        {
            string engineName = GetEngineName(target);
            if (engineName == null) return null;

            Assembly[] curentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = curentAssemblies.SelectMany(c => c.GetTypes());
            Type necessaryType = types.FirstOrDefault(t =>
                (String.Equals(t.Name, engineName, StringComparison.InvariantCultureIgnoreCase) &&
                        t.IsSubclassOf(typeof(RequestEngine))));
            if (necessaryType == null)
                try
                {
                    new RequestEngine().GetDefault();
                }
                catch (Exception exc)
                {
                    LogMessage.Add(exc, Level.Error);
                    NotFound();
                    return null;
                }
            return necessaryType;
        } 
        #endregion

        #region GetEngineName
        private string GetEngineName(string target)
        {
            int i = target.IndexOf(".rpr");
            if ((i == -1) || (i == 0))
            {
                LogMessage.Add("Bad requested target: " + target, Level.Error);
                NotFound();
                return null;
            }
            return target.Substring(0, i) + "requestengine";
        } 
        #endregion

        #region NotFound
        private void NotFound()
        {
            NotFoundRequestEngine notFound = new NotFoundRequestEngine();
            try
            {
                notFound.Get().GetResponse();
            }
            catch (Exception exc)
            {
                LogMessage.Add(exc, Level.Error);
                Error();
            }
        } 
        #endregion

        #region Error
        private void Error()
        {
            IResponse response = new Response();
            response.StatusCode = HttpStatusCode.InternalServerError;
            try
            {
                string result = new ResponseStrategies().GetByRoutes("error.rpr", (string)null);
                response.Send(result);
            }
            catch
            {
                LogMessage.Add("Page error.html doesn't exist", Level.Error);
                response.Send("Internal Server Error...");
            }
        }
        #endregion
    }
}
