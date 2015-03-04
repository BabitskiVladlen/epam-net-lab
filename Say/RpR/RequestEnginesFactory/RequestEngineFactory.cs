#region using
using Ninject;
using RpR.Infrastructure;
using RpR.RequestEngines.Infrastructure;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web; 
#endregion

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
                throw new ArgumentNullException("MethodInvoker is null", (Exception)null);
            _methodInvoker = methodInvoker;
        } 
        #endregion

        #region CreateEngine
        public void CreateEngine(string target)
        {
            #region EngineName
            int i = target.IndexOf(".rpr");
            if ((i == -1) || (i == 0))
            {
                LogMessage.Add("Bad requested target: " + target, Level.Error);
                // not found
                return;
            }
            string engineName = target.Substring(0, i);
            engineName += "requestengine"; 
            #endregion

            #region GetType
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
            #endregion

            #region GetCtor
            var ctor = necessaryType.GetConstructors(BindingFlags.Public)
                .FirstOrDefault(c => c.GetParameters().Length == 0);
            if (ctor == null)
            {
                LogMessage.Add("Public constructor without parameters of " +
                    engineName.ToUpper(CultureInfo.InvariantCulture) + " doesn't exist", Level.Error);
                // not found
            }
            object instance = ctor.Invoke(null);
            #endregion

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
