using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Infrastructure
{
    public interface IFriendshipService
    {
        IEnumerable<Friendship> Friendships { get; }
        Friendship GetFriendshipByID(int friendshipID);
        void SaveFriendship(Friendship friendship);
        void BreakFriendship(int friendshipID);
        IEnumerable<int> GetAllUserFriends(int userID);
    }
}
