using System.Collections.Specialized;
using System.Reflection;

namespace RpR.Infrastructure
{
    public interface IModelBinder
    {
        object[] BindModel(ParameterInfo[] parameters, NameValueCollection httpCollection);
    }
}
