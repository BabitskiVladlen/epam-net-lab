#region using
using Ninject;
using RpR.Infrastructure;
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
        private readonly IModelBinder _binder;
        private readonly HttpContext _currentContext;

        #region .ctors
        public MethodInvoker()
            : this(DependencyResolution.Kernel.Get<IModelBinder>())
        { }

        private MethodInvoker(IModelBinder binder)
        {
            if (binder == null)
                throw new ArgumentNullException("IModelBinder is null", (Exception)null);
            _currentContext = HttpContext.Current;
            if (_currentContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            _binder = binder;
        }
        #endregion

        #region InvokeMethod
        public void InvokeMethod(object engine)
        {
            if (engine == null)
                throw new ArgumentNullException("Request engine is null", (Exception)null);

            #region GetMethod
            string httpMethod = _currentContext.Request.HttpMethod;
            var method = engine.GetType().GetMethods(BindingFlags.Public)
                .FirstOrDefault(m => String.Equals(m.Name, httpMethod, StringComparison.InvariantCultureIgnoreCase));
            if (method == null)
                throw new MethodAccessException
                    ("Public " + httpMethod + "() method doesn't exist in " + engine.GetType().Name, (Exception)null); 
            #endregion

            #region GetHttpCollection
            NameValueCollection httpCollettion = new NameValueCollection();
            if (httpMethod == "GET") httpCollettion = HttpContext.Current.Request.QueryString;
            else httpCollettion = HttpContext.Current.Request.Form; 
            #endregion

            var parameters = method.GetParameters();
            object[] paramsValues = _binder.BindModel(parameters, httpCollettion);
            method.Invoke(engine, paramsValues);
        } 
        #endregion
    }
}
