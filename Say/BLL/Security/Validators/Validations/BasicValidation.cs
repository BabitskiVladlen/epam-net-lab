using BLL.Security.Infrastructure;
using System;
using System.Collections.Generic;

namespace BLL.Security.Validators
{
    public class BasicValidation : IValidation
    {
        public bool IsValid(string str, out List<string> errors)
        {
            errors = new List<string>();
            if (String.IsNullOrEmpty(str) || String.IsNullOrWhiteSpace(str))
            {
                errors.Add("Input string is empty");
                return false;
            }
            return true;
        }
    }
}
