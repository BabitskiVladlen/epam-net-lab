#region using
using Ninject;
using RpR.Infrastructure;
using RpR.ResponseEngines.Infrastructure;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web; 
#endregion

namespace RpR.MethodInvokers
{
    public class MethodInvoker : IMethodInvoker
    {
        #region Fields&Props
        private readonly IModelBinder _binder;
        private readonly HttpContext _currentContext; 
        #endregion

        #region .ctors
        public MethodInvoker(IModelBinder binder = null)
        {
            _currentContext = HttpContext.Current;
            if (_currentContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            _binder = binder ?? DependencyResolution.Kernel.Get<IModelBinder>();
        }
        #endregion

        #region InvokeMethod
        public void InvokeMethod(object engine)
        {
            if (engine == null)
                throw new ArgumentNullException("Request engine is null", (Exception)null);
            
            MethodInfo method = null;
            try { method = GetMethod(engine); }
            catch (MethodAccessException exc) { throw exc; }
            NameValueCollection httpCollettion = GetHttpCollection();
            var parameters = method.GetParameters();
            object[] paramsValues = _binder.BindModel(parameters, httpCollettion);
            object result = method.Invoke(engine, paramsValues);
            SendResponse(result);
        }
        #endregion

        #region GetMethod
        private MethodInfo GetMethod(object engine)
        {
            string httpMethod = _currentContext.Request.HttpMethod;
            var method = engine.GetType().GetMethods()
                .FirstOrDefault(m => String.Equals(m.Name, httpMethod, StringComparison.InvariantCultureIgnoreCase));
            if (method == null)
                throw new MethodAccessException
                    ("Public " + httpMethod + "() method doesn't exist in " + engine.GetType().Name, (Exception)null);
            return method;
        } 
        #endregion

        #region GetHttpCollection
        private NameValueCollection GetHttpCollection()
        {
            NameValueCollection httpCollettion = new NameValueCollection();
            if (_currentContext.Request.HttpMethod == "GET") httpCollettion
                = HttpContext.Current.Request.QueryString;
            else httpCollettion = HttpContext.Current.Request.Form;
            return httpCollettion;
        } 
        #endregion

        #region SendResponse
        private void SendResponse(object result)
        {
            if (result == null) return;
            ResponseEngine responseEngine = result as ResponseEngine;
            if (responseEngine != null)
            {
                responseEngine.GetResponse();
                return;
            }
            new Response().Send(result.ToString());
        } 
        #endregion
    }
}
