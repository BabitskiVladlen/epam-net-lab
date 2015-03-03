using BLL.Security.Infrastructure;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class LengthValidation : IValidation
    {
        public string Selector { get; set; }

        #region IsValid
        public bool IsValid(string str, out List<string> errors)
        {
            bool isValid = true;
            errors = new List<string>();
            BasicValidation def = new BasicValidation();
            def.Selector = this.Selector;

            if (!def.IsValid(str, out errors))
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
