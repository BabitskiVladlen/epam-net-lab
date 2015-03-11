using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Abstract
{
    public interface IToDoItemService
    {
        Task<IEnumerable<ToDoModel>> GetAll();

        Task<ToDoModel> Create(ToDoModel todo);

        Task<ToDoModel> GetById(int id);

        Task<bool> RemoveById(int id);

        Task<ToDoModel> Update(ToDoModel todo);
    }
}
