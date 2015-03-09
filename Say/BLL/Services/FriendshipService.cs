#region using
using BLL.Infrastructure;
using DAL.Entities;
using DAL.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq; 
#endregion

namespace BLL.Services
{
    public class FriendshipService : IFriendshipService
    {
        #region Fields&Props
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserService _userService; 
        #endregion

        #region .ctors
        public FriendshipService(IFriendshipRepository friendshipRepository = null, IUserService userService = null)
        {
            _friendshipRepository = friendshipRepository ?? DependencyResolution.Kernel.Get<IFriendshipRepository>();
            _userService = userService ?? new UserService();
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
            SetCountOfUserFriends(friendship, true);
        } 
        #endregion

        #region AcceptFriendship
        public void AcceptFriendship(int friendshipID)
        {
            Friendship friendship = GetFriendshipByID(friendshipID);
            if (friendship == null) return;
            friendship.Waiting = false;
            SaveFriendship(friendship);
            try
            {
                SetCountOfUserFriends(friendshipID, false);
            }
            catch (ArgumentException exc) { throw exc; }
        }
        #endregion

        #region BreakFriendship
        public void BreakFriendship(int friendshipID)
        {
            _friendshipRepository.BreakFriendship(friendshipID);
            try
            {
                SetCountOfUserFriends(friendshipID, false);
            }
            catch (ArgumentException exc) { throw exc; }
        } 
        #endregion

        #region GetAllUserFriends
        public IEnumerable<User> GetAllUserFriends(int userID)
        {
            List<int> friends = Friendships
                .Where(f => f.Friend2 == userID).OrderBy(f => !f.Waiting)
                .Select(f => f.Friend1).ToList<int>();
            friends.AddRange(Friendships
                .Where(f => ((f.Friend1 == userID) && !f.Waiting)).Select(f => f.Friend2));
            return _userService.Users.Where(u => (friends.Contains(u.UserID) && !u.IsDeleted));
        } 
        #endregion

        #region SetCountOfUserFriends
        private void SetCountOfUserFriends(int friendshipID, bool add)
        {
            SetCountOfUserFriends(GetFriendshipByID(friendshipID), add);
        }

        private void SetCountOfUserFriends(Friendship friendship, bool add)
        {
            if (friendship == null) return;
            if (friendship.Waiting)
            {
                User user = _userService.GetUserByID(friendship.Friend2);
                if (user == null) return;
                if (add) ++user.NewFriends;
                else --user.NewFriends;
                _userService.SaveUser(user);
            }
        }
        #endregion
    }
}
