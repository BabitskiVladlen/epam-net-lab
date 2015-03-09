#region using
using DAL.Entities;
using System.Security.Principal; 
#endregion

namespace BLL.Security.Infrastructure
{
    // wrapper for HttpContext
    public interface IAppContext
    {
        string GetUserData();
        void SetUserData(string data);
        void DeleteUserData();
        IPrincipal User { get; set; }
    }
}
