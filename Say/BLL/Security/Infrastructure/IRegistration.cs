using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Security.Infrastructure
{
    public interface IRegistration
    {
        void AddNewUser(User user, out List<string> errors);
    }
}
