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
    public class MessageRepository : IMessageRepository
    {
        #region Fields&Props
        private readonly EfDbContext _context = new EfDbContext(); 
        #endregion

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
            try { exMessage = Validator.MessageValidator(message); }
            catch (ArgumentException exc)
            { throw exc; }

            if (message.MessageID == 0)
                _context.Messages.Add(message);
            else if (exMessage != null)
            {
                exMessage.Message = message.Message;
                exMessage.IsDeleted = message.IsDeleted;
                exMessage.IsNew = message.IsNew;
                exMessage.MessageTime = message.MessageTime;
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
