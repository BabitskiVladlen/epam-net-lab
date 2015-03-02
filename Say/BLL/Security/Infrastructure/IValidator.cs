using System.Collections.Generic;

namespace BLL.Security.Infrastructure
{
    public interface IValidator
    {
        IEnumerable<IValidation> GetValidations();
    }
}
