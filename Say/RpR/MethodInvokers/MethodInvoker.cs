using Ninject;
using RpR.Infrastructure;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace RpR.ActionInvokers
{
    public class MethodInvoker : IMethodInvoker
    {
        private readonly IModelBinder _binder;
        private readonly HttpContext _currentContext;

        #region .ctors
        public MethodInvoker()
            : this(DependencyResolution.Kernel.Get<IModelBinder>(), HttpContext.Current)
        { }

        public MethodInvoker(IModelBinder binder)
            : this(binder, HttpContext.Current)
        { }

        private MethodInvoker(IModelBinder binder, HttpContext currentContext)
        {
            if (binder == null)
                throw new ArgumentNullException("IModelBinder is null", (Exception)null);
            if (currentContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
            _binder = binder;
            _currentContext = currentContext;
        }
        #endregion

        #region InvokeMethod
        public void InvokeMethod(object engine)
        {
            if (engine == null)
                throw new ArgumentNullException("Request engine is null", (Exception)null);

            string httpMethod = _currentContext.Request.HttpMethod;
            var method = engine.GetType().GetMethods()
                .FirstOrDefault(m => String.Equals(m.Name, httpMethod, StringComparison.InvariantCultureIgnoreCase));
            if (method == null)
                throw new MethodAccessException
                    (httpMethod + " method doesn't exist in this request engine", (Exception)null);

            var parameters = method.GetParameters();
            NameValueCollection qsCollettion = new NameValueCollection();
            if (httpMethod == "GET") qsCollettion = HttpContext.Current.Request.QueryString;
            else qsCollettion = HttpContext.Current.Request.Form;

            try
            {
                object[] paramsValues = _binder.BindModel(parameters, qsCollettion);
                method.Invoke(engine, paramsValues);
            }
            catch (Exception exc)
            {
                throw new Exception("Action exception", exc);
            }
        } 
        #endregion
    }
}
