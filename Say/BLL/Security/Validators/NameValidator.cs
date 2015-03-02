using BLL.Security.Infrastructure;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class NameValidator : IValidator
    {
        public IEnumerable<IValidation> GetValidations()
        {
            List<IValidation> validators = new List<IValidation>();
            validators.Add(new LengthValidation() { Selector = "first name or surname" });
            validators.Add(new NameSymbolsValidation());
            return validators;
        }
    }
}
