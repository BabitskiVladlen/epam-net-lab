using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface.Abstract
{
    public interface IToDoItemServiceCache
    {
        IEnumerable<ToDoModel> GetAll();

        ToDoModel Create(ToDoModel todo);

        ToDoModel GetById(int id);

        bool RemoveById(int id);

        ToDoModel Update(ToDoModel todo);
    }
}
