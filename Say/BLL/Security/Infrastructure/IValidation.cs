#region using
using System.Collections.Generic; 
#endregion

namespace BLL.Security.Infrastructure
{
    public interface IValidation
    {
        bool IsValid(string str, List<string> errors);
    }
}
