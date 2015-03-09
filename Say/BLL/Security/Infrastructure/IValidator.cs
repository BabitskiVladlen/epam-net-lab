#region using
using System.Collections.Generic; 
#endregion

namespace BLL.Security.Infrastructure
{
    public interface IValidator
    {
        IEnumerable<IValidation> GetValidations();
    }
}
