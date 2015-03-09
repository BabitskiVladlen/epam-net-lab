#region using
using BLL.Infrastructure;
using BLL.Security.Contexts;
using BLL.Security.Infrastructure;
using BLL.Security.PasswordEngines;
using BLL.Security.Validators;
using BLL.Services;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace BLL.Security
{
    public class DefaultRegistration : IRegistration
    {
        #region Fields&Props
        private readonly IAppContext _context;
        private readonly IUserService _userService;
        private readonly IPasswordEngine _passwordEngine;
        private readonly IValidatorFactory _validatorFactory; 
        #endregion

        #region .ctors
        public DefaultRegistration(IUserService userService = null, IValidatorFactory validatorFactory = null,
            IPasswordEngine passwordEngine = null, IAppContext context = null)
        {
            _userService = userService ?? new UserService();
            _validatorFactory = validatorFactory ?? new ValidatorFactory();
            _passwordEngine = passwordEngine ?? new MD5PasswordEngine();
            _context = context ?? new WebContext();
        } 
        #endregion

        #region TryAddUser
        public bool TryAddUser(User user, string passwordAgain, List<string> errors)
        {
            if (user == null)
                throw new ArgumentNullException("User is null", (Exception)null);

            if (errors == null) errors = new List<string>();
            bool isValid = true;

            /* Validations: */

            #region ExistsAlready
            User exUser = _userService.GetUser(new string[] { user.Username, user.Email }.AsEnumerable<string>());
            if ((exUser != null) && (user.UserID != exUser.UserID))
            {
                errors.Add("This username or email exist already");
                isValid = false;
            } 
            #endregion

            #region DifferentPasswords
            if (!String.Equals(user.Password, passwordAgain, StringComparison.InvariantCulture))
            {
                errors.Add("Different passwords");
                isValid = false;
            } 
            #endregion

            #region Password
            IValidator validator = _validatorFactory.PasswordValidator;
            IEnumerable<IValidation> passwordValidations = validator.GetValidations();
            foreach (var v in passwordValidations)
            {
                if (!v.IsValid(user.Password, errors))
                    isValid = false;
            }  
            #endregion

            #region Username
            validator = _validatorFactory.UsernameValidator;
            IEnumerable<IValidation> usernameValidations = validator.GetValidations();
            foreach (var v in usernameValidations)
            {
                if (!v.IsValid(user.Username, errors))
                    isValid = false;
            } 
            #endregion

            #region Names
            validator = _validatorFactory.NameValidator;
            IEnumerable<IValidation> nameValidations = validator.GetValidations();
            foreach (var v in nameValidations)
            {
                if (!v.IsValid(user.FirstName, errors))
                    isValid = false;
            }
            foreach (var v in nameValidations)
            {
                if (!v.IsValid(user.Surname, errors))
                    isValid = false;
            } 
            #endregion

            #region Email
            validator = _validatorFactory.EmailValidator;
            IEnumerable<IValidation> emailValidations = validator.GetValidations();
            foreach (var v in emailValidations)
            {
                if (!v.IsValid(user.Email, errors))
                    isValid = false;
            } 
            #endregion

            if (!isValid) return false;

            user.Password = _passwordEngine.Create(user.Password);
            _userService.SaveUser(user);
            _context.SetUserData(user.UserID.ToString());
            return true;
        } 
        #endregion
    }
}
