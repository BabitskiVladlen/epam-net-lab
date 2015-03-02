using DAL.Entities;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;

namespace BLL.Security.Infrastructure
{
    public interface IAuthentication
    {
        IPrincipal CurrentUser { get; set; }
        User Login(string name, string password);
        void Logout();
    }
}
