#region using
using BLL.Security.Infrastructure;
using System.Collections.Generic; 
#endregion

namespace BLL.Security.Validators
{
    public class LengthValidation : IValidation
    {
        public string Selector { get; set; }

        #region IsValid
        public bool IsValid(string str, List<string> errors)
        {
            bool isValid = true;
            if (errors == null) errors = new List<string>();
            BasicValidation def = new BasicValidation();
            def.Selector = this.Selector;

            if (!def.IsValid(str, errors))
                return false;
            if (str.Length > 50)
            {
                if (Selector == null) Selector = "Input string";
                errors.Add(Selector + " must be less then 50 characters");
                isValid = false;
            }
            return isValid;
        } 
        #endregion
    }
}
