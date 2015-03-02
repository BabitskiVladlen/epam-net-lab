using System;

namespace DAL.Entities
{
    public class MessageEntity
    {
        public int MessageID { get; set; }
        public int FromUser { get; set; }
        public int ToUser { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
