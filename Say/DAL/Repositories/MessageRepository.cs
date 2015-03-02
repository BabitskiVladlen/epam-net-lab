using DAL.Contexts;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Validators;
using System;
using System.Linq;

namespace DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly EfDbContext _context = new EfDbContext();

        #region Messages
        public IQueryable<MessageEntity> Messages
        {
            get { return _context.Messages; }
        } 
        #endregion

        #region GetMessageByID
        public MessageEntity GetMessageByID(int messageID)
        {
            return _context.Messages.Find(messageID);
        } 
        #endregion

        #region SaveMessage
        public void SaveMessage(MessageEntity message)
        {
            MessageEntity exMessage;
            try { Validator.MessageValidator(message, out exMessage); }
            catch (ArgumentException exc)
            { throw exc; }

            if (message.MessageID == 0)
                _context.Messages.Add(message);
            else if (exMessage != null)
            {
                exMessage.Message = message.Message;
                exMessage.IsDeleted = message.IsDeleted;
            }
            _context.SaveChanges();
        } 
        #endregion

        #region DeleteMessage
        public void DeleteMessage(int messageID)
        {
            MessageEntity message = GetMessageByID(messageID);
            if (message != null)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }
        } 
        #endregion
    }
}
