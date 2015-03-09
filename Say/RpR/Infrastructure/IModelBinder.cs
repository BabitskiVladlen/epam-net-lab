#region using
using System.Collections.Specialized;
using System.Reflection; 
#endregion

namespace RpR.Infrastructure
{
    public interface IModelBinder
    {
        object[] BindModel(ParameterInfo[] parameters, NameValueCollection httpCollection);
    }
}
