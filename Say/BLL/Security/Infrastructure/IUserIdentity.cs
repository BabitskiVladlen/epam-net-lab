using DAL.Entities;
using System.Security.Principal;

namespace BLL.Security.Infrastructure
{
    interface IUserIdentity : IIdentity
    {
        User User { get; set; }
    }
}
