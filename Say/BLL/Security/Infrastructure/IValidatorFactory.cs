namespace BLL.Security.Infrastructure
{
    public interface IValidatorFactory
    {
        IValidator PasswordValidator { get; }
        IValidator AdminPasswordValidator { get; }
        IValidator UsernameValidator { get; }
        IValidator NameValidator { get; }
        IValidator EmailValidator { get; }      
    }
}
