using RpR.ResponseEngines.Infrastructure;
using System;
using System.Collections.Generic;
using System.Web;

namespace RpR.ResponseEngines
{
    public class RegistrationResponseEngine : ResponseEngine
    {
        public void GetResponse(dynamic model, Tuple<IEnumerable<string>, string> errors)
        {
            if (IsAjax)
            {
                AjaxResponse(model, errors);
                return;
            }

            if (errors != null)
            {
                string regForm =

                #region form
                    @"<p id=""validation""></p>
                    <form id=""registration"" name=""registration"" action=""registration.rpr"" method=""post"">
                            <div>
                                <h3>Registration</h3>
                            </div>

                            <div>
                                <p>FirstName</p>
                                <input class=""textbox"" id=""firstname"" name=""firstname"" type=""text"" value="""
                                                + model.FirstName + @"""/>
                            </div>

                            <div>
                                <p>Surname</p>
                                <input class=""textbox"" id=""surname"" name=""surname"" type=""text"" value="""
                                                + model.Surname + @"""/>
                            </div>

                            <div>
                                <p>Username</p>
                                <input class=""textbox"" id=""username"" name=""username"" type=""text"" value="""
                                                + model.Username + @"""/>
                            </div>

                            <div>
                                <p>Password</p>
                                <input class=""textbox"" id=""password"" name=""password"" type=""password"" value="""
                                                + model.Password + @"""/>
                            </div>
                            <div>
                                <p>Password again</p>
                                <input class=""textbox"" id=""passwordAgain"" name=""passwordAgain"" type=""password"" value="""
                                                + model.PasswordAgain + @"""/>
                            </div>
                            <div>
                                <p>Email</p>
                                <input class=""textbox"" id=""email"" name=""email"" type=""text"" value="""
                                                + model.Email + @"""/>
                            </div>
                            <div>
                            <input type=""submit"" value=""Register"" class=""button"" id=""reg_button""/>
                        </div>
                    </form>"; 
                #endregion

                Response.Errors = errors;
                HttpResponse.Write(Response.ContentToRoute("registration.rpr", regForm));
            }
        }

        private void AjaxResponse(dynamic model, Tuple<IEnumerable<string>, string> errors)
        {

        }
    }
}