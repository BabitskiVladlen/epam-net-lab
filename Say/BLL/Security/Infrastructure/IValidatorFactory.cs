namespace BLL.Security.Infrastructure
{
    public interface IValidatorFactory
    {
        IValidator PasswordValidator { get; }
        IValidator UsernameValidator { get; }
        IValidator NameValidator { get; }
        IValidator EmailValidator { get; }      
    }
}
