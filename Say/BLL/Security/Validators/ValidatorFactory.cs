using BLL.Security.Infrastructure;
using System;

namespace BLL.Security.Validators
{
    public class ValidatorFactory : IValidatorFactory
    {
        #region PasswordValidator
        public IValidator PasswordValidator
        {
            get { return new PasswordValidator(); }
        } 
        #endregion

        #region AdminPasswordValidator
        public IValidator AdminPasswordValidator
        {
            get { return new AdminPasswordValidator(); }
        } 
        #endregion

        #region UsernameValidator
        public IValidator UsernameValidator
        {
            get { return new UsernameValidator(); }
        } 
        #endregion

        #region NameValidator
        public IValidator NameValidator
        {
            get { return new NameValidator(); }
        }
        #endregion

        #region EmailValidator
        public IValidator EmailValidator
        {
            get { return new EmailValidator(); }
        } 
        #endregion
    }
}
