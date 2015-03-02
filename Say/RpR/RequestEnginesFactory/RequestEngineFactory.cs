using Ninject;
using RpR.Infrastructure;
using RpR.RequestEngines.Infrastructure;
using System;
using System.Linq;
using System.Reflection;

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
            if (IsNull(target)) return;
            int i = target.IndexOf(".rpr");
            if ((i == -1) || (i == 0))
            {
                LogMessage.Add("Request: " + target, Level.Error);
                // not found
                return;
            }
            string engineName = target.Substring(0, i);
            engineName += "requestengine";

            Type necessaryType = null;
            Assembly[] cuurentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var a in cuurentAssemblies)
            {
                Type[] types = a.GetTypes();
                foreach (Type t in types)
                {
                    if (String.Equals(t.Name, engineName, StringComparison.InvariantCultureIgnoreCase) &&
                        t.IsAssignableFrom(typeof(RequestEngine)))
                    {
                        necessaryType = t;
                        break;
                    }
                    if (necessaryType != null) break;
                }
            }
            if (IsNull(necessaryType)) return;

            var ctor = necessaryType.GetConstructors().FirstOrDefault(c => c.GetParameters().Length == 0);
            if (IsNull(ctor)) return;

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

        #region IsNull
        private bool IsNull(object obj)
        {
            if (obj == null)
            {
                LogMessage.Add(obj.ToString() + "is null", Level.Error);
                // not found
                return true;
            }
            return false;
        } 
        #endregion
    }
}
