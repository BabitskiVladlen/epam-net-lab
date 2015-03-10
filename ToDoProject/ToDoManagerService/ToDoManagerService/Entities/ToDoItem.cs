using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ToDoManagerService.Entities
{
    [DataContract]
    public class ToDoItem
    {
        [DataMember]
        public int ToDoId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public bool IsCompleted { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}