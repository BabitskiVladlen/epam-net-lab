#region using
using BLL.Security.Infrastructure;
using System;
using System.Collections.Generic; 
#endregion

namespace BLL.Security.Validators
{
    public class BasicValidation : IValidation
    {
        #region Fields&Props
        public string Selector { get; set; } 
        #endregion

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
