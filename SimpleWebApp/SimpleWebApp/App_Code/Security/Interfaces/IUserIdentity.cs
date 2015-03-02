using System.Security.Principal;

namespace SimpleWebApp.Security.Interfaces
{
    interface IUserIdentity : IIdentity
    {
        User User { get; set; }
    }
}
