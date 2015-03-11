using DAL.Interface.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Mapping;
using AmbientDbContext.Interface;
using BLL.Interface.Abstract;
using BLL.Interface.Entities;

namespace BLL.Concrete
{
    public class ToDoItemServiceCache : IToDoItemServiceCache
    {
        private IDbContextScopeFactory dbContextScopeFactory;
        private IToDoItemRepository repository;

        public ToDoItemServiceCache(IToDoItemRepository repository, IDbContextScopeFactory dbContextScopeFactory)
        {
            if (dbContextScopeFactory == null)
            {
                throw new System.ArgumentNullException();
            }
            if (repository == null)
            {
                throw new System.ArgumentNullException();
            }
            this.dbContextScopeFactory = dbContextScopeFactory;
            this.repository = repository;
        }

        public IEnumerable<ToDoModel> GetAll()
        {
            IEnumerable<ToDoModel> result = new List<ToDoModel>();
            using (var context = dbContextScopeFactory.CreateReadOnly())
            {
                var results = this.repository.GetAll();
                if (results.Count() != 0)
                {
                    result = results.Select(r => r.ToBll()).ToList();
                }
            }
            return result;
        }

        public ToDoModel Create(ToDoModel todo)
        {
            using (var context = dbContextScopeFactory.Create())
            {
                this.repository.Create(todo.ToDal());
                context.SaveChanges();
            }
            return this.GetById(todo.Id);
        }

        public ToDoModel GetById(int id)
        {
            ToDoModel result = null;
            using (var context = dbContextScopeFactory.CreateReadOnly())
            {
                var subject = this.repository.GetById(id);
                if (subject != null)
                {
                    result = subject.ToBll();
                }
            }
            return result;
        }

        public bool RemoveById(int id)
        {
            bool result = false;
            using (var context = dbContextScopeFactory.Create())
            {
                var todo = this.repository.GetById(id);
                if (todo != null)
                {
                    this.repository.RemoveById(id);
                    result = true;
                }
                context.SaveChanges();
            }
            return result;
        }

        public ToDoModel Update(ToDoModel todo)
        {
            using (var context = dbContextScopeFactory.Create())
            {
                this.repository.Update(todo.ToDal());
                context.SaveChanges();
            }
            return this.GetById(todo.Id);
        }
    }
}
