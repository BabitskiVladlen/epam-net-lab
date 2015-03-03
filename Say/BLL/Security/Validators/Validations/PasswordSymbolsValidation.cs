using BLL.Security.Infrastructure;
using System;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class PasswordSymbolsValidation : IValidation
    {
        #region IsValid
        public bool IsValid(string password, out List<string> errors)
        {
            bool isValid = true;
            errors = new List<string>();
            BasicValidation def = new BasicValidation() { Selector = "Password" };
            if (!def.IsValid(password, out errors))
                return false;

            bool hasUpperCase = false;
            bool hasDigit = false;
            foreach (var ch in password)
            {
                if (Char.IsUpper(ch)) hasUpperCase = true;
                if (Char.IsDigit(ch)) hasDigit = true;
            }
            if (!hasUpperCase)
            {
                errors.Add("Password must contain at least one uppercase letter");
                isValid = false;
            }
            if (!hasDigit)
            {
                errors.Add("Password must contain at least one digit");
                isValid = false;
            }
            return isValid;
        } 
        #endregion
    }
}
