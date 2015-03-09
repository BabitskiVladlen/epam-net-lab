#region using
using DAL.Entities;
using System.Collections.Generic; 
#endregion

namespace BLL.Infrastructure
{
    public interface IFriendshipService
    {
        IEnumerable<Friendship> Friendships { get; }
        Friendship GetFriendshipByID(int friendshipID);
        void SaveFriendship(Friendship friendship);
        void AcceptFriendship(int friendshipID);
        void BreakFriendship(int friendshipID);
        IEnumerable<User> GetAllUserFriends(int userID);
    }
}
