using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Infrastructure
{
    public interface IUserService
    {
        IEnumerable<User> Users { get; }
        User GetUserByID(int userID);
        bool IsExist(string selector);
        void SaveUser(User user);
        void DeleteUser(int userID);
        void DisableUser(int userID);
        bool IsInRole(int userID, string role);
    }
}
