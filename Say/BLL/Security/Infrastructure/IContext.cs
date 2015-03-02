using DAL.Entities;
using System.Security.Principal;

namespace BLL.Security.Infrastructure
{
    // wrapper for HttpContext
    public interface IContext
    {
        string GetUserData();
        void SetUserData(string data);
        void DeleteUserData();
        IPrincipal User { get; set; }
    }
}
