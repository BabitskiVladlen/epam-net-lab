#region using
using BLL.Security.Infrastructure;
using System.Collections.Generic;
using System.Text.RegularExpressions; 
#endregion

namespace BLL.Security.Validators
{
    public class NameSymbolsValidation : IValidation
    {
        #region IsValid
        public bool IsValid(string name, List<string> errors)
        {
            bool isValid = true;
            if (errors == null) errors = new List<string>();
            BasicValidation def = new BasicValidation() { Selector = "Name/Surname" };
            if (!def.IsValid(name, errors))
                return false;

            Regex rgx = new Regex(@"^[a-zA-Z]+$");
            if (!rgx.IsMatch(name))
            {
                errors.Add("First name or surname is wrong");
                isValid = false;
            }
            return isValid;
        } 
        #endregion
    }
}
