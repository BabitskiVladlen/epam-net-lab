using BLL.Security.Infrastructure;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class PasswordValidator : IValidator
    {
        #region GetValidations
        public IEnumerable<IValidation> GetValidations()
        {
            List<IValidation> validators = new List<IValidation>();
            validators.Add(new PasswordLengthValidation());
            validators.Add(new PasswordSymbolsValidation());
            validators.Add(new LengthValidation() { Selector = "Password" });
            return validators;
        } 
        #endregion
    }
}
