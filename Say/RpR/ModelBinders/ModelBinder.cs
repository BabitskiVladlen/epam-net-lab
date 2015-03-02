using RpR.Infrastructure;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Web;

namespace RpR.ModelBinders
{
    public class ModelBinder : IModelBinder
    {
        private readonly HttpContext _currentContext;

        #region .ctor
        public ModelBinder()
        {
            _currentContext = HttpContext.Current;
            if (_currentContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
        }
        #endregion

        #region BindModel
        public object[] BindModel(ParameterInfo[] parameters, NameValueCollection httpCollection)
        {
            if (parameters == null)
                throw new ArgumentNullException("Array of parameters is null", (Exception)null);
            if (parameters.Length == 0) return null;

            object[] paramValues = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                try
                {
                    if (parameters[i].ParameterType == typeof(string))
                        paramValues[i] = Activator.CreateInstance(parameters[i].ParameterType, String.Empty.ToCharArray());
                    else if ((parameters[i].ParameterType.IsAssignableFrom(typeof(HttpPostedFileBase))))
                        paramValues[i] =
                            Activator.CreateInstance(parameters[i].ParameterType,
                                new[] { new HttpPostedFileWrapper(_currentContext.Request.Files[0]) });
                    else if ((parameters[i].ParameterType.IsAssignableFrom(typeof(HttpFileCollection))))
                        paramValues[i] =
                            Activator.CreateInstance(parameters[i].ParameterType,
                                new[] { _currentContext.Request.Files });
                    else paramValues[i] = Activator.CreateInstance(parameters[i].ParameterType);
                }
                catch (Exception exc)
                {
                    throw new InvalidOperationException("Model definition or http-request error", exc);
                }
            }
            if ((httpCollection == null) || (paramValues.Length == 0)) return paramValues;

            TypeConverter converter;
            for (int i = 0; i < parameters.Length; ++i)
            {
                if (paramValues[i] is  HttpPostedFileBase ||
                    paramValues[i] is HttpFileCollection) continue;

                foreach (string key in httpCollection.Keys)
                {
                    if (String.Equals(parameters[i].Name, key, StringComparison.InvariantCultureIgnoreCase))
                    {
                        try
                        {
                            converter = TypeDescriptor.GetConverter(paramValues[i].GetType());
                            paramValues[i] = converter.ConvertFrom(httpCollection[key]);
                            break;
                        }
                        catch { break; }
                    }
                    else
                        TryPropertyBinder(paramValues[i], key, httpCollection[key]);
                }
            }
            return paramValues;
        } 
        #endregion

        #region TryPropertyBinder
        private void TryPropertyBinder(object obj, string key, string value)
        {
            var properties = obj.GetType().GetProperties();
            TypeConverter converter;
            foreach (var p in properties)
            {
                if (String.Equals(p.Name, key, StringComparison.InvariantCultureIgnoreCase))
                {
                    try
                    {
                        converter = TypeDescriptor.GetConverter(p.PropertyType);
                        p.SetValue(obj, converter.ConvertFrom(value));
                        break;
                    }
                    catch { break; }
                }
            }
        } 
        #endregion
    }
}
