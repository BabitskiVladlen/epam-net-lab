#region using
using DAL.Entities; 
#endregion

namespace BLL.Security.Infrastructure
{
    public interface IAuthentication
    {
        User SignIn(string name, string password);
        void Logout();
    }
}
