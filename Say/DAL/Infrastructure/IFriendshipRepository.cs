#region using
using DAL.Entities;
using System.Linq; 
#endregion

namespace DAL.Infrastructure
{
    public interface IFriendshipRepository
    {
        IQueryable<Friendship> Friendships { get; }
        Friendship GetFriendshipByID(int friendshipID);
        void SaveFriendship(Friendship friendship);
        void BreakFriendship(int friendshipID);
    }
}
