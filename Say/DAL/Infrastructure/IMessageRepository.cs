#region using
using DAL.Entities;
using System.Linq; 
#endregion

namespace DAL.Infrastructure
{
    public interface IMessageRepository
    {
        IQueryable<MessageEntity> Messages { get; }
        MessageEntity GetMessageByID(int messageID);
        void SaveMessage(MessageEntity message);
        void DeleteMessage(int messageID);
    }
}
