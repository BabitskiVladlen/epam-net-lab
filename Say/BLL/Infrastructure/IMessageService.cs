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
        IEnumerable<MessageEntity> GetAllUserMessages(int FromUser, int ToUser);
    }
}
