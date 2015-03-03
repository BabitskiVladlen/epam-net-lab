using BLL.Security.Infrastructure;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class AdminPasswordValidation : IValidation
    {
        private const string _secretLetter = "V";
        #region IsValid
        public bool IsValid(string password, out List<string> errors)
        {
            errors = new List<string>();
            BasicValidation def = new BasicValidation() { Selector = "Password" };
            if (!def.IsValid(password, out errors))
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
