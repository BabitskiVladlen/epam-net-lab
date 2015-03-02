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

        #region .ctors
        public MessageService()
            : this(DependencyResolution.Kernel.Get<IMessageRepository>())
        { }

        public MessageService(IMessageRepository messageRepository)
        {
            if (messageRepository == null)
                throw new ArgumentNullException("Repository is null", (Exception)null);
            _messageRepository = messageRepository;
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
        } 
        #endregion

        #region DeleteMessage
        public void DeleteMessage(int messageID)
        {
            _messageRepository.DeleteMessage(messageID);
        } 
        #endregion

        #region DisableMeaage
        public void DisableMessage(int messageID)
        {
            MessageEntity message = GetMessageByID(messageID);
            message.IsDeleted = true;
            try { SaveMessage(message); }
            catch (ArgumentException exc) { throw exc; }
        }
        #endregion

        #region GetAllUserMessages
        public IEnumerable<MessageEntity> GetAllUserMessages(int FromUser, int ToUser)
        {
            return Messages
                    .Where(m => ((m.FromUser == FromUser) && (m.ToUser == ToUser) &&
                        (m.FromUser == ToUser) && (m.ToUser == FromUser))).OrderBy(m => m.MessageTime);
        } 
        #endregion
    }
}
