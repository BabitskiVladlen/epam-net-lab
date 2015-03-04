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
#endregion

namespace BLL.Security
{
    public class DefaultRegistration : IRegistration
    {
        private readonly IAppContext _context;
        private readonly IUserService _userService;
        private readonly IPasswordEngine _passwordEngine;
        private readonly IValidatorFactory _validatorsEngine;

        #region .ctors
        public DefaultRegistration()
            : this(new UserService(), new PasswordEngineMD5(), new ValidatorFactory(), new WebContext())
        { }

        public DefaultRegistration(IPasswordEngine passwordEngine)
            : this(new UserService(), passwordEngine, new ValidatorFactory(), new WebContext())
        { }

        public DefaultRegistration(IUserService userService)
            : this(userService, new PasswordEngineMD5(), new ValidatorFactory(), new WebContext(userService))
        { }

        public DefaultRegistration(IUserService userService, IValidatorFactory validatorsEngine)
            : this(userService, new PasswordEngineMD5(), validatorsEngine, new WebContext(userService))
        { }

        public DefaultRegistration(IUserService userService, IAppContext context)
            : this(userService, new PasswordEngineMD5(), new ValidatorFactory(), context)
        { }

        public DefaultRegistration(IUserService userService, IPasswordEngine passwordEngine,
            IValidatorFactory validatorsEngine, IAppContext context)
        {
            if (userService == null)
                throw new ArgumentNullException("Service is null", (Exception)null);
            if (passwordEngine == null)
                throw new ArgumentNullException("Password engine is null", (Exception)null);
            if (validatorsEngine == null)
                throw new ArgumentNullException("Validators engine is null", (Exception)null);
            if (context == null)
                throw new ArgumentNullException("Context is null", (Exception)null);

            _userService = userService;
            _passwordEngine = passwordEngine;
            _validatorsEngine = validatorsEngine;
            _context = context;
        } 
        #endregion

        #region TryAddUser
        public bool TryAddUser(User user, string passwordAgain, List<string> errors)
        {
            if (errors == null) errors = new List<string>();
            bool isValid = true;
            if (_userService.IsExist(user.Username))
            {
                errors.Add("This username exists already");
                isValid = false;
            }

            if (!String.Equals(user.Password, passwordAgain, StringComparison.InvariantCulture))
            {
                errors.Add("Different passwords");
                isValid = false;
            }

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
                if (!v.IsValid(user.Password, errors))
                    isValid = false;
            }  
            #endregion

            #region username
            validator = _validatorsEngine.UsernameValidator;
            IEnumerable<IValidation> usernameValidations = validator.GetValidations();
            foreach (var v in usernameValidations)
            {
                if (!v.IsValid(user.Username, errors))
                    isValid = false;
            } 
            #endregion

            #region names
            validator = _validatorsEngine.NameValidator;
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

            #region email
            validator = _validatorsEngine.EmailValidator;
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
