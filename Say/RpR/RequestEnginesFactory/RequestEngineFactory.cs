using Ninject;
using RpR.Infrastructure;
using RpR.RequestEngines.Infrastructure;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace RpR.RequestEngineFactories
{
    public class RequestEngineFactory : IRequestEngineFactory
    {
        private readonly IMethodInvoker _methodInvoker;

        #region .ctors
        public RequestEngineFactory()
        {
            _methodInvoker = DependencyResolution.Kernel.Get<IMethodInvoker>();
        }

        public RequestEngineFactory(IMethodInvoker methodInvoker)
        {
            if (methodInvoker == null)
                throw new ArgumentNullException("IMethodInvoker is null", (Exception)null);
            _methodInvoker = methodInvoker;
        } 
        #endregion

        #region CreateEngine
        public void CreateEngine(string target)
        {
            int i = target.IndexOf(".rpr");
            if ((i == -1) || (i == 0))
            {
                LogMessage.Add("Bad requested target: " + target, Level.Error);
                // not found
                return;
            }
            string engineName = target.Substring(0, i);
            engineName += "requestengine";

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
                HttpContext.Current.Response.Write("Not Fround =)"); // add not found
                return;
            }

            var ctor = necessaryType.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 0);
            if (ctor == null)
            {
                LogMessage.Add("Constructor without parameters of " +
                    engineName.ToUpper(CultureInfo.InvariantCulture) + " is null", Level.Error);
                // not found
            }

            object instance = ctor.Invoke(null);
            try
            {
                _methodInvoker.InvokeMethod(instance);
            }
            catch (Exception exc)
            {
                LogMessage.Add(exc, Level.Error);
                // error to client
            }
        } 
        #endregion
    }
}
