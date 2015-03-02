using BLL.Security.Infrastructure;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class EmailValidator : IValidator
    {
        public IEnumerable<IValidation> GetValidations()
        {
            List<IValidation> validators = new List<IValidation>();
            validators.Add(new LengthValidation() { Selector = "email" });
            validators.Add(new EmailSymbolsValidation());
            return validators;
        }
    }
}
