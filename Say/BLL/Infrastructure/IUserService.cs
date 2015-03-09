#region using
using DAL.Entities;
using System.Collections.Generic;
using System.IO; 
#endregion

namespace BLL.Infrastructure
{
    public interface IUserService
    {
        IEnumerable<User> Users { get; }
        User GetUserByID(int userID);
        User GetUserByID(string userID);
        User GetUser(IEnumerable<string> selector);
        void SaveUser(User user);
        void DeleteUser(int userID);
        void DisableUser(int userID);
        void EnableUser(int userID);
        bool IsInRole(int userID, string role);
        void SaveImage(int userID, Stream inputStream, string contentType);
    }
}
