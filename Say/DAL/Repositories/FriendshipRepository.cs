#region using
using DAL.Contexts;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Validators;
using System;
using System.Linq; 
#endregion

namespace DAL.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        #region Fields&Props
        private readonly EfDbContext _context = new EfDbContext(); 
        #endregion

        #region Friendships
        public IQueryable<Friendship> Friendships
        {
            get { return _context.Friendships; }
        } 
        #endregion

        #region GetFriendshipByID
        public Friendship GetFriendshipByID(int friendshipID)
        {
            return _context.Friendships.Find(friendshipID);
        } 
        #endregion

        #region SaveFriendship
        public void SaveFriendship(Friendship friendship)
        {
            Friendship exFriendship;
            try { exFriendship = Validator.FriendshipValidator(friendship); }
            catch (ArgumentException exc)
            { throw exc; }

            if (friendship.FriendshipID == 0)
                _context.Friendships.Add(friendship);
            else if (exFriendship != null)
                exFriendship.Waiting = friendship.Waiting;
            _context.SaveChanges();
        } 
        #endregion

        #region BreakFriendship
        public void BreakFriendship(int friendshipID)
        {
            Friendship friendship = GetFriendshipByID(friendshipID);
            if (friendship != null)
            {
                _context.Friendships.Remove(friendship);
                _context.SaveChanges();
            }
        } 
        #endregion
    }
}
