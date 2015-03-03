using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("Messages")]
    public class MessageEntity
    {
        [Key]
        public int MessageID { get; set; }
        public int FromUser { get; set; }
        public int ToUser { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsNew { get; set; }
    }
}
