using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Security.Infrastructure
{
    public interface IRegistration
    {
        bool TryAddUser(User user, string passwordAgain, List<string> errors);
    }
}
