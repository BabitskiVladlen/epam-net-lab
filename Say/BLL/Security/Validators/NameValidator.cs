using BLL.Security.Infrastructure;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class NameValidator : IValidator
    {
        #region GetValidations
        public IEnumerable<IValidation> GetValidations()
        {
            List<IValidation> validators = new List<IValidation>();
            validators.Add(new LengthValidation() { Selector = "Name/Surname" });
            validators.Add(new NameSymbolsValidation());
            return validators;
        } 
        #endregion
    }
}
