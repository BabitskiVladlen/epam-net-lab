#region using
using BLL.Security.Infrastructure;
using System.Collections.Generic; 
#endregion

namespace BLL.Security.Validators
{
    public class PasswordLengthValidation : IValidation
    {
        #region IsValid
        public bool IsValid(string password, List<string> errors)
        {
            bool isValid = true;
            if (errors == null) errors = new List<string>();
            BasicValidation def = new BasicValidation() { Selector = "Password" };
            if (!def.IsValid(password, errors))
                return false;

            if (password.Length < 7)
            {
                errors.Add("Password must be more then 7 characters");
                isValid = false;
            }
            return isValid;
        } 
        #endregion
    }
}
