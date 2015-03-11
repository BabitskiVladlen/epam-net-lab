using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapping
{
    public static class ToDoModelMap
    {
        public static DAL.Interface.Entities.ToDoModel ToDal(this ToDoModel item)
        {
            return new DAL.Interface.Entities.ToDoModel
            {
                Id = item.Id,
                Description = item.Description,
                IsDone = item.IsDone,
                IsDeleted = item.IsDeleted
            };
        }

        public static ToDoModel ToBll(this DAL.Interface.Entities.ToDoModel item)
        {
            return new ToDoModel
            {
                Id = item.Id,
                Description = item.Description,
                IsDone = item.IsDeleted,
                IsDeleted = item.IsDeleted
            };
        }

        public static BLL.Service.ToDoItem ToService(this ToDoModel item, int userId)
        {
            return new BLL.Service.ToDoItem
            {
                ToDoId = item.Id,
                Name = item.Description,
                IsCompleted = item.IsDone,
                UserId = userId
            };
        }

        public static ToDoModel ToBll(this BLL.Service.ToDoItem item)
        {
            return new ToDoModel
            {
                Id = item.ToDoId,
                Description = item.Name,
                IsDone = item.IsCompleted
            };
        }
    }
}
