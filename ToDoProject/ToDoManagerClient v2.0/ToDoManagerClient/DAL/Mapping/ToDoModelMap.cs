using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public static class ToDoModelMap
    {
        public static ORM.Model.ToDoModel ToOrm(this ToDoModel item)
        {
            return new ORM.Model.ToDoModel
            {
                 Id = item.Id,
                 Description = item.Description,
                 IsDone = item.IsDone,
                 IsDeleted = item.IsDeleted
            };
        }

        public static ToDoModel ToDal(this ORM.Model.ToDoModel item)
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
