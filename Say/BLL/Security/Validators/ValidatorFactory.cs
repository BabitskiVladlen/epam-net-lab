#region using
using BLL.Security.Infrastructure;
using System; 
#endregion

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
