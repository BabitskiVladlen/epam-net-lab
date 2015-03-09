#region using
using RpR.Infrastructure;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
#endregion

namespace RpR.ModelBinders
{
    public class ModelBinder : IModelBinder
    {
        #region Fields&Props
        private readonly HttpContext _httpContext; 
        #endregion

        #region .ctors
        public ModelBinder()
        {
            _httpContext = HttpContext.Current;
            if (_httpContext == null)
                throw new InvalidOperationException("Current http-context is null", (Exception)null);
        }
        #endregion

        #region BindModel
        public object[] BindModel(ParameterInfo[] parameters, NameValueCollection httpCollection)
        {
            if (parameters == null)
                throw new ArgumentNullException("Array of parameters is null", (Exception)null);
            if (parameters.Length == 0) return new object[0];

            object[] paramValues = InitParamValues(parameters, httpCollection);
            if ((httpCollection == null) || (httpCollection.Count == 0)) return paramValues; 

            TypeConverter converter;
            for (int i = 0; i < parameters.Length; ++i)
            {
                if (paramValues[i] == null || paramValues[i] is HttpPostedFileBase ||
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

        #region InitParamValues
        private object[] InitParamValues(ParameterInfo[] parameters, NameValueCollection httpCollection)
        {
            object[] paramValues = new object[parameters.Length];
            HttpFileCollection fileColl = _httpContext.Request.Files;
            for (int i = 0; i < parameters.Length; ++i)
            {
                if (parameters[i].ParameterType == typeof(string))
                    paramValues[i] = Activator.CreateInstance(parameters[i].ParameterType, String.Empty.ToCharArray());
                else if ((parameters[i].ParameterType.IsAssignableFrom(typeof(HttpPostedFileBase))) &&
                    (fileColl != null) && (fileColl.Count != 0))
                    paramValues[i] = new HttpPostedFileWrapper(fileColl[0]);
                else if ((parameters[i].ParameterType.IsAssignableFrom(typeof(HttpFileCollection))))
                    paramValues[i] = fileColl;
                else
                    try
                    {
                        paramValues[i] = Activator.CreateInstance(parameters[i].ParameterType);
                    }
                    catch
                    {
                        paramValues[i] = null;
                    }
                if (parameters[i].IsOptional &&
                    httpCollection.AllKeys.FirstOrDefault(k =>
                        String.Equals(k, parameters[i].Name, StringComparison.InvariantCultureIgnoreCase)) == null)
                    paramValues[i] = parameters[i].DefaultValue;                    
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
