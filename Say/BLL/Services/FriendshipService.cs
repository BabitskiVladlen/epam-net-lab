using BLL.Infrastructure;
using DAL.Entities;
using DAL.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;

        #region .ctors
        public FriendshipService()
            : this(DependencyResolution.Kernel.Get<IFriendshipRepository>())
        { }

        public FriendshipService(IFriendshipRepository friendshipRepository)
        {
            if (friendshipRepository == null)
                throw new ArgumentNullException("Repository is null", (Exception)null);
            _friendshipRepository = friendshipRepository;
        } 
        #endregion

        #region Friendships
        public IEnumerable<Friendship> Friendships
        {
            get { return _friendshipRepository.Friendships.AsEnumerable<Friendship>(); }
        } 
        #endregion

        #region GetFriendshipByID
        public Friendship GetFriendshipByID(int friendshipID)
        {
            return _friendshipRepository.GetFriendshipByID(friendshipID);
        } 
        #endregion

        #region SaveFriendship
        public void SaveFriendship(Friendship friendship)
        {
            try { _friendshipRepository.SaveFriendship(friendship); }
            catch (ArgumentException exc) { throw exc; }
        } 
        #endregion

        #region BreakFriendship
        public void BreakFriendship(int friendshipID)
        {
            _friendshipRepository.BreakFriendship(friendshipID);
        } 
        #endregion

        #region GetAllUserFriends
        public IEnumerable<int> GetAllUserFriends(int userID)
        {
            List<int> friends = Friendships
                .Where(f => f.Friend1 == userID).Select(f => f.Friend2).ToList<int>();
            friends.AddRange(Friendships
                .Where(f => f.Friend2 == userID).Select(f => f.Friend1));
            return friends.AsEnumerable<int>();
        } 
        #endregion
    }
}
