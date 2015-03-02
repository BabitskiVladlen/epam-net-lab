using BLL.Infrastructure;
using BLL.Security.Infrastructure;
using BLL.Security.Validators;
using BLL.Services;
using DAL.Entities;
using System;
using System.Collections.Generic;

namespace BLL.Security
{
    public class DefaultRegistration : IRegistration
    {
        private readonly IUserService _userService;
        private readonly IPasswordEngine _passwordEngine;
        private readonly IValidatorFactory _validatorsEngine;

        #region .ctors
        public DefaultRegistration()
            : this(new UserService(), new PasswordEngineMD5(), new ValidatorFactory())
        { }

        public DefaultRegistration(IUserService userService)
            : this(userService, new PasswordEngineMD5(), new ValidatorFactory())
        { }

        public DefaultRegistration(IPasswordEngine passwordEngine)
            : this(new UserService(), passwordEngine, new ValidatorFactory())
        { }

        public DefaultRegistration(IValidatorFactory validatorsEngine)
            : this(new UserService(), new PasswordEngineMD5(), validatorsEngine)
        { }

        public DefaultRegistration(IUserService userService, IPasswordEngine passwordEngine)
            : this(userService, passwordEngine, new ValidatorFactory())
        { }

        public DefaultRegistration(IUserService userService, IValidatorFactory validatorsEngine)
            : this(userService, new PasswordEngineMD5(), validatorsEngine)
        { }

        public DefaultRegistration(IPasswordEngine passwordEngine, IValidatorFactory validatorsEngine)
            : this(new UserService(), passwordEngine, validatorsEngine)
        { }

        public DefaultRegistration(IUserService userService, IPasswordEngine passwordEngine,
            IValidatorFactory validatorsEngine)
        {
            if (userService == null)
                throw new ArgumentNullException("Service is null", (Exception)null);
            if (passwordEngine == null)
                throw new ArgumentNullException("Password engine is null", (Exception)null);
            if (validatorsEngine == null)
                throw new ArgumentNullException("Validators engine is null", (Exception)null);

            _userService = userService;
            _passwordEngine = passwordEngine;
            _validatorsEngine = validatorsEngine;
        } 
        #endregion

        #region AddNewUser
        public void AddNewUser(User user, out List<string> errors)
        {
            if (_userService.IsExist(user.Username))
                throw new ArgumentException("User exists already", (Exception)null);

            bool isValid = true;
            errors = new List<string>();
            List<string> outErrors;

            /* Validations: */ 

            #region password
            List<IValidation> passwordValidations = new List<IValidation>();
            IValidator validator = _validatorsEngine.PasswordValidator;
            passwordValidations.AddRange(validator.GetValidations());
            if (_userService.IsInRole(user.UserID, "Admin"))
            {
                validator = _validatorsEngine.AdminPasswordValidator;
                passwordValidations.AddRange(validator.GetValidations());
            }
            foreach (var v in passwordValidations)
            {
                if (!v.IsValid(user.Password, out outErrors))
                {
                    isValid = false;
                    errors.AddRange(outErrors);
                }
            }  
            #endregion

            #region username
            validator = _validatorsEngine.UsernameValidator;
            IEnumerable<IValidation> usernameValidations = validator.GetValidations();
            foreach (var v in usernameValidations)
            {
                if (!v.IsValid(user.Username, out outErrors))
                {
                    isValid = false;
                    errors.AddRange(outErrors);
                }
            } 
            #endregion

            #region names
            validator = _validatorsEngine.NameValidator;
            IEnumerable<IValidation> nameValidations = validator.GetValidations();
            foreach (var v in nameValidations)
            {
                if (!v.IsValid(user.FirstName, out outErrors))
                {
                    isValid = false;
                    errors.AddRange(outErrors);
                }
            }
            foreach (var v in nameValidations)
            {
                if (!v.IsValid(user.Surname, out outErrors))
                {
                    isValid = false;
                    ((List<string>)errors).AddRange(outErrors);
                }
            } 
            #endregion

            #region email
            validator = _validatorsEngine.EmailValidator;
            IEnumerable<IValidation> emailValidations = validator.GetValidations();
            foreach (var v in emailValidations)
            {
                if (!v.IsValid(user.Email, out outErrors))
                {
                    isValid = false;
                    errors.AddRange(outErrors);
                }
            } 
            #endregion

            if (!isValid)
                throw new ArgumentException("Invalid user's data");

            user.Password = _passwordEngine.Create(user.Password);
            try { _userService.SaveUser(user); }
            catch (Exception exc)
            { throw new Exception("Service error", exc); }
        } 
        #endregion
    }
}
