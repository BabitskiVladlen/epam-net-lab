using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using ToDoManagerService.Entities;

namespace ToDoManagerService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ToDoManager : IToDoManager
    {
        public List<Entities.ToDoItem> GetTodoList(int userId)
        {
            ToDoManagerDatabaseDataContext context = new ToDoManagerDatabaseDataContext();

            var result = (from todo in context.ToDoItems
                         where todo.UserId == userId
                         select new Entities.ToDoItem()
                         {
                             IsCompleted = todo.IsCompleted,
                             Name = todo.Name,
                             UserId = todo.UserId,
                             ToDoId = todo.ToDoId
                         }).ToList();

            Thread.Sleep(10000);
            return result;
        }

        public void UpdateToDoItem(Entities.ToDoItem todo)
        {
            ToDoManagerDatabaseDataContext context = new ToDoManagerDatabaseDataContext();

            var todoToUpdate = context.ToDoItems.Where(x => x.ToDoId == todo.ToDoId).SingleOrDefault();

            todoToUpdate.IsCompleted = todo.IsCompleted;
            todoToUpdate.Name = todo.Name;

            context.SubmitChanges();

            Thread.Sleep(10000);
        }

        public void DeleteToDoItem(int todoItemId)
        {
            ToDoManagerDatabaseDataContext context = new ToDoManagerDatabaseDataContext();

            var todoTodelete = context.ToDoItems.Where(x => x.ToDoId == todoItemId).SingleOrDefault();

            context.ToDoItems.DeleteOnSubmit(todoTodelete);

            context.SubmitChanges();

            Thread.Sleep(10000);
        }

        public void CreateToDoItem(Entities.ToDoItem todo)
        {
            ToDoManagerDatabaseDataContext context = new ToDoManagerDatabaseDataContext();

            context.ToDoItems.InsertOnSubmit(new ToDoManagerService.ToDoItem()
            {
                IsCompleted = todo.IsCompleted,
                Name = todo.Name,
                UserId = todo.UserId
            });

            context.SubmitChanges();

            Thread.Sleep(10000);
        }

        public int CreateUser(string name)
        {
            ToDoManagerDatabaseDataContext context = new ToDoManagerDatabaseDataContext();

            var user = new User() { Name = name };

            context.Users.InsertOnSubmit(user);

            context.SubmitChanges();

            Thread.Sleep(10000);

            return user.UserId;
        }
    }
}
