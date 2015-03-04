using BLL.Security.Infrastructure;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class AdminPasswordValidation : IValidation
    {
        private const string _secretLetter = "V";

        #region IsValid
        public bool IsValid(string password, List<string> errors)
        {
            if (errors == null) errors = new List<string>();
            BasicValidation def = new BasicValidation() { Selector = "Password" };
            if (!def.IsValid(password, errors))
                return false;

            if (!password.Contains(_secretLetter))
            {
                errors.Add("Administrator's password must contain one secret letter");
                return false;
            }
            return true;
        } 
        #endregion
    }
}
