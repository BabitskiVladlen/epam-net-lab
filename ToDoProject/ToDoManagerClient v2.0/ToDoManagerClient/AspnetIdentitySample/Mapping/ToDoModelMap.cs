using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoManager.Models;

namespace ToDoManager.Mapping
{
    public static class ToDoModelMap
    {
        public static BLL.Interface.Entities.ToDoModel ToBll(this ToDoModel item)
        {
            return new BLL.Interface.Entities.ToDoModel
            {
                Id = item.Id,
                Description = item.Description,
                IsDone = item.IsDone,
                IsDeleted = item.IsDeleted
            };
        }

        public static ToDoModel ToUi(this BLL.Interface.Entities.ToDoModel item)
        {
            return new ToDoModel
            {
                Id = item.Id,
                Description = item.Description,
                IsDone = item.IsDeleted,
                IsDeleted = item.IsDeleted
            };
        }
    }
}