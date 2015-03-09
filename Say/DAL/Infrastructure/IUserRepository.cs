#region using
using DAL.Entities;
using System.Linq; 
#endregion

namespace DAL.Infrastructure
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        User GetUserByID(int userID);
        void SaveUser(User user);
        void DeleteUser(int userID);
    }
}
