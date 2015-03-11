using BLL.Interface.Abstract;
using BLL.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Mapping;

namespace BLL.Concrete
{
    public class ToDoItemService : IToDoItemService
    {
        private BLL.Service.IToDoManager manager;

        public ToDoItemService(BLL.Service.IToDoManager manager)
        {
            this.manager = manager;
        }

        public async Task<IEnumerable<ToDoModel>> GetAll()
        {
            var result = await this.manager.GetTodoListAsync(1);
            return result.Select(r => r.ToBll());
        }

        public async Task<ToDoModel> Create(ToDoModel todo)
        {
            this.manager.CreateToDoItem(todo.ToService(1));
            return await this.GetById(todo.Id);
        }

        public async Task<ToDoModel> GetById(int id)
        {
            ToDoModel result = null;
            var serviceResult = await this.GetAll();
            var query = serviceResult.Where(r => r.Id == id);
            if (query.Count() != 0)
            {
                result = query.First();
            }
            return result;
        }

        public async Task<bool> RemoveById(int id)
        {
            await this.manager.DeleteToDoItemAsync(id);
            var result = await this.GetById(id);
            return result == null;
        }

        public async Task<ToDoModel> Update(ToDoModel todo)
        {
            await this.manager.UpdateToDoItemAsync(todo.ToService(1));
            return await this.GetById(todo.Id);
        }
    }
}
