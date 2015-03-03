using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Infrastructure
{
    public interface IMessageService
    {
        IEnumerable<MessageEntity> Messages { get; }
        MessageEntity GetMessageByID(int messageID);
        void SaveMessage(MessageEntity message);
        void DeleteMessage(int messageID);
        void DisableMessage(int messageID);
        void EnableMessage(int messageID);
        IEnumerable<MessageEntity> GetAllUserMessages(int userID);
        IEnumerable<MessageEntity> GetAllUserMessages(int User1, int User2);
    }
}
