using DAL.Entities;
using DAL.Repositories;
using System;

namespace DAL.Validators
{
    public static class Validator
    {
        private readonly static UserRepository _userRepository = new UserRepository();
        private readonly static FriendshipRepository _friendshipRepository = new FriendshipRepository();
        private readonly static MessageRepository _messageRepository = new MessageRepository();
        private readonly static RoleRepository _roleRepository = new RoleRepository();

        #region UserValidator
        public static void UserValidator(User user, out User exUser)
        {
            if (user == null)
                throw new ArgumentNullException("User is null", (Exception)null);
            exUser = null;
            if (user.UserID != 0)
            {
                exUser = _userRepository.GetUserByID(user.UserID);
                if (exUser == null)
                    return;
            }
            if (String.IsNullOrEmpty(user.Username) || String.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username is null or empty", (Exception)null);
            if (user.Username.Length > 50)
                throw new ArgumentException("Username must be less then 50 characters", (Exception)null);
            if (String.IsNullOrEmpty(user.FirstName) || String.IsNullOrWhiteSpace(user.FirstName))
                throw new ArgumentException("First name is null or empty", (Exception)null);
            if (user.FirstName.Length > 50)
                throw new ArgumentException("First name must be less then 50 characters", (Exception)null);
            if (String.IsNullOrEmpty(user.Surname) || String.IsNullOrWhiteSpace(user.Surname))
                throw new ArgumentException("Surname is null or empty", (Exception)null);
            if (user.Surname.Length > 50)
                throw new ArgumentException("Surname must be less then 50 characters", (Exception)null);
            if (String.IsNullOrEmpty(user.Password) || String.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password is null or empty", (Exception)null);
            if (user.Password.Length > 512)
                throw new ArgumentException("Username must be less then 512 characters", (Exception)null);
            if (String.IsNullOrEmpty(user.Email) || String.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException("Email is null or empty", (Exception)null);
            if (user.Email.Length > 50)
                throw new ArgumentException("Email must be less then 50 characters", (Exception)null);
            if (user.NewMessages < 0)
                throw new ArgumentException("Count of new messages < 0", (Exception)null);
            if (user.NewFriends < 0)
                throw new ArgumentException("Count of new friends < 0", (Exception)null);
            if (_roleRepository.GetRoleByID(user.Role) == null)
                throw new ArgumentException("Invalid role", (Exception)null);
        }
        #endregion

        #region RoleValidator
        public static void RoleValidator(RoleEntity role, out RoleEntity exRole)
        {
            if (role == null)
                throw new ArgumentNullException("Role entity is null", (Exception)null);
            exRole = null;
            if (role.RoleID != 0)
            {
                exRole = _roleRepository.GetRoleByID(role.RoleID);
                if (exRole == null)
                    return;
            }
            if (String.IsNullOrEmpty(role.Role) || String.IsNullOrWhiteSpace(role.Role))
                throw new ArgumentException("Role is null or empty", (Exception)null);
            if (role.Role.Length > 20)
                throw new ArgumentException("Role must be less then 20 characters", (Exception)null);
        }
        #endregion

        #region FriendshipValidator
        public static void FriendshipValidator(Friendship friendship, out Friendship exFriendship)
        {
            if (friendship == null)
                throw new ArgumentNullException("Friendship entity is null", (Exception)null);
            exFriendship = null;
            if (friendship.FriendshipID != 0)
            {
                exFriendship = _friendshipRepository.GetFriendshipByID(friendship.FriendshipID);
                if (exFriendship == null)
                    return;
                if (!exFriendship.Waiting) throw new ArgumentException("Friendship cannot be change");
                if ((friendship.Friend1 != exFriendship.Friend1) || (friendship.Friend2 != exFriendship.Friend2))
                    throw new ArgumentException("Invalid state of friendship", (Exception)null);
            }
            if (_userRepository.GetUserByID(friendship.Friend1) == null)
                throw new ArgumentException("Invalid friends", (Exception)null);
            if (_userRepository.GetUserByID(friendship.Friend2) == null)
                throw new ArgumentException("Invalid friends", (Exception)null);
        }
        #endregion

        #region MessageValidator
        public static void MessageValidator(MessageEntity message, out MessageEntity exMessage)
        {
            if (message == null)
                throw new ArgumentNullException("Message entity is null", (Exception)null);
            exMessage = null;
            if (message.MessageID != 0)
            {
                exMessage = _messageRepository.GetMessageByID(message.MessageID);
                if (exMessage == null)
                    return;
                if ((message.FromUser != exMessage.FromUser) || (message.ToUser != exMessage.ToUser))
                    throw new ArgumentException("Invalid state of message entity", (Exception)null);
            }
            if (String.IsNullOrEmpty(message.Message) || String.IsNullOrWhiteSpace(message.Message))
                throw new ArgumentException("Message is null or empty", (Exception)null);
            if (message.Message.Length > 1000)
                throw new ArgumentException("Message must be less then 1000 characters", (Exception)null);
            if (_userRepository.GetUserByID(message.FromUser) == null)
                throw new ArgumentException("Invalid sender", (Exception)null);
            if (_userRepository.GetUserByID(message.ToUser) == null)
                throw new ArgumentException("Invalid recipient", (Exception)null);
            if (message.MessageTime.Day < DateTime.Now.Day)
                throw new ArgumentException("Invalid date of message", (Exception)null);
        }
        #endregion
    }
}
