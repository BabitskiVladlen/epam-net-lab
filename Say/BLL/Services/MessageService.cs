using BLL.Infrastructure;
using DAL.Entities;
using DAL.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserService _userService;

        #region .ctors
        public MessageService()
            : this(DependencyResolution.Kernel.Get<IMessageRepository>(), new UserService())
        { }

        public MessageService(IMessageRepository messageRepository)
            : this(messageRepository, new UserService())
        { }

        public MessageService(IMessageRepository messageRepository, IUserService userService)
        {
            if (messageRepository == null)
                throw new ArgumentNullException("Repository is null", (Exception)null);
            if (userService == null)
                throw new ArgumentNullException("Service is null", (Exception)null);
            _messageRepository = messageRepository;
            _userService = userService;
        } 
        #endregion

        #region Messages
        public IEnumerable<MessageEntity> Messages
        {
            get { return _messageRepository.Messages.AsEnumerable<MessageEntity>(); }
        } 
        #endregion

        #region GetMessageByID
        public MessageEntity GetMessageByID(int messageID)
        {
            return _messageRepository.GetMessageByID(messageID);
        } 
        #endregion

        #region SaveMessage
        public void SaveMessage(MessageEntity message)
        {
            try { _messageRepository.SaveMessage(message); }
            catch (ArgumentException exc) { throw exc; }
            SetCountOfUserMessages(message, true);
        } 
        #endregion

        #region DeleteMessage
        public void DeleteMessage(int messageID)
        {
            _messageRepository.DeleteMessage(messageID);
            SetCountOfUserMessages(messageID, false);
        } 
        #endregion

        #region DisableMessage
        public void DisableMessage(int messageID)
        {
            MessageEntity message = GetMessageByID(messageID);
            message.IsDeleted = true;
            SaveMessage(message);
            try
            {
                SetCountOfUserMessages(messageID, false);
            }
            catch (ArgumentException exc) { throw exc; } 
        }
        #endregion

        #region EnableMeaage
        public void EnableMessage(int messageID)
        {
            MessageEntity message = GetMessageByID(messageID);
            message.IsDeleted = false;
            SaveMessage(message);
            try
            {
                SetCountOfUserMessages(messageID, true);
            }
            catch (ArgumentException exc) { throw exc; } 
        }
        #endregion

        #region GetAllUserMessages
        public IEnumerable<MessageEntity> GetAllUserMessages(int userID)
        {
            return Messages.Reverse().Distinct(new MessagesEqualityComparer())
                .Where(m => m.FromUser == userID || m.ToUser == userID);
        } 

        public IEnumerable<MessageEntity> GetAllUserMessages(int User1, int User2)
        {
            return Messages
                    .Where(m => (((m.FromUser == User1) || (m.FromUser == User2)) &&
                        ((m.ToUser == User1) || (m.ToUser == User2)))).Reverse();
        }
        #endregion

        #region SetCountOfUserMessages
        private void SetCountOfUserMessages(int messageID, bool add)
        {
            SetCountOfUserMessages(GetMessageByID(messageID), add);
        }

        private void SetCountOfUserMessages(MessageEntity message, bool add)
        {
            if (message.IsNew)
            {
                User user = _userService.GetUserByID(message.ToUser);
                if (add) ++user.NewMessages;
                else --user.NewMessages;
                _userService.SaveUser(user);
            }
        } 
        #endregion
    }

    #region MessagesEqualityComparer
    internal class MessagesEqualityComparer : IEqualityComparer<MessageEntity>
    {
        public bool Equals(MessageEntity x, MessageEntity y)
        {
            return ((x.ToUser == y.ToUser && x.FromUser == y.FromUser)
                    || (x.ToUser == y.FromUser && x.FromUser == y.ToUser));
        }

        public int GetHashCode(MessageEntity obj)
        {
            int first = obj.ToUser, second = obj.FromUser;
            if (obj.FromUser <= obj.ToUser) { first = obj.FromUser; second = obj.ToUser; }
            return new Tuple<int, int>(first, second).GetHashCode();
        }
    } 
    #endregion
}
