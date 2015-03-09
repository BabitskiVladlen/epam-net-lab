#region using
using BLL.Security.Infrastructure;
using System.Collections.Generic; 
#endregion

namespace BLL.Security.Validators
{
    public class EmailValidator : IValidator
    {
        #region GetValidations
        public IEnumerable<IValidation> GetValidations()
        {
            List<IValidation> validators = new List<IValidation>();
            validators.Add(new LengthValidation() { Selector = "Email" });
            validators.Add(new EmailSymbolsValidation());
            return validators;
        } 
        #endregion
    }
}
