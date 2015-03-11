using DAL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface.Abstract
{
    public interface IToDoItemRepository
    {
        IEnumerable<ToDoModel> GetAll();

        ToDoModel Create(ToDoModel todo);

        ToDoModel GetById(int id);

        bool RemoveById(int id);

        ToDoModel Update(ToDoModel todo);
    }
}
