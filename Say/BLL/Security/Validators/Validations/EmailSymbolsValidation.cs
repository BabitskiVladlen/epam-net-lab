using BLL.Security.Infrastructure;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL.Security.Validators
{
    public class EmailSymbolsValidation : IValidation
    {
        #region IsValid
        public bool IsValid(string email, out List<string> errors)
        {
            bool isValid = true;
            errors = new List<string>();
            BasicValidation def = new BasicValidation() { Selector = "Email" };
            if (!def.IsValid(email, out errors))
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
