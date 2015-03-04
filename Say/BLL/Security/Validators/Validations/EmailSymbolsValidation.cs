using BLL.Security.Infrastructure;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL.Security.Validators
{
    public class EmailSymbolsValidation : IValidation
    {
        #region IsValid
        public bool IsValid(string email, List<string> errors)
        {
            bool isValid = true;
            if (errors == null) errors = new List<string>();
            BasicValidation def = new BasicValidation() { Selector = "Email" };
            if (!def.IsValid(email, errors))
                return false;

            Regex rgx = new Regex(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}");
            if (!rgx.IsMatch(email))
            {
                errors.Add("Email is wrong");
                isValid = false;
            }
            return isValid;
        } 
        #endregion
    }
}
