namespace BLL.Security.Infrastructure
{
    public interface IPasswordEngine
    {
        string Create(string plainText);
        bool Verify(string plainText, string password);
    }
}
