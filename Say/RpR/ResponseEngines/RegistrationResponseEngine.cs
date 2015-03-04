using RpR.ResponseEngines.Infrastructure;
using System;
using System.Collections.Generic;

namespace RpR.ResponseEngines
{
    public class RegistrationResponseEngine : ResponseEngine
    {
        public void GetResponse(dynamic model, Tuple<string, IEnumerable<string>> errors)
        {
            if (IsAjax)
            {
                AjaxResponse(errors, model);
                return;
            }
        }

        private void AjaxResponse(dynamic model, Tuple<string, IEnumerable<string>> errors)
        {

        }
    }
}