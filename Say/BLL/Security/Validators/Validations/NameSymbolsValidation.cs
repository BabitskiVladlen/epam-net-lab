using BLL.Security.Infrastructure;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BLL.Security.Validators
{
    public class NameSymbolsValidation : IValidation
    {
        public bool IsValid(string name, out List<string> errors)
        {
            bool isValid = true;
            errors = new List<string>();
            BasicValidation def = new BasicValidation();
            if (!def.IsValid(name, out errors))
                return false;

            Regex rgx = new Regex(@"^[a-zA-Z]+$");
            if (!rgx.IsMatch(name))
            {
                errors.Add("First name or surname is wrong");
                isValid = false;
            }
            return isValid;
        }
    }
}
