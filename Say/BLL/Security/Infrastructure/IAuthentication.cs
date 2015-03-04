using DAL.Entities;

namespace BLL.Security.Infrastructure
{
    public interface IAuthentication
    {
        User Login(string name, string password);
        void Logout();
    }
}
