using BLL.Security.Infrastructure;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class PasswordLengthValidation : IValidation
    {
        public bool IsValid(string password, out List<string> errors)
        {
            bool isValid = true;
            errors = new List<string>();
            BasicValidation def = new BasicValidation();
            if (!def.IsValid(password, out errors))
                return false;

            if (password.Length < 7)
            {
                errors.Add("Password must be more then 7 characters");
                isValid = false;
            }
            return isValid;
        }
    }
}
