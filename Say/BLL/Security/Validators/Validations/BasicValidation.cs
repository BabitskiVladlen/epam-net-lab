using BLL.Security.Infrastructure;
using System;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class BasicValidation : IValidation
    {
        public string Selector { get; set; }

        #region IsValid
        public bool IsValid(string str, List<string> errors)
        {
            if (errors == null) errors = new List<string>();
            if (String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str))
            {
                if (Selector == null) Selector = "Input string";
                errors.Add(Selector + " is empty");
                return false;
            }
            return true;
        } 
        #endregion
    }
}
