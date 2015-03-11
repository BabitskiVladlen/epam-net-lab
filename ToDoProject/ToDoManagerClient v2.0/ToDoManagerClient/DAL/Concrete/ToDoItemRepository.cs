using DAL.Interface.Abstract;
using ORM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Mapping;
using DAL.Interface.Entities;
using AmbientDbContext.Interface;

namespace DAL
{
    public class ToDoItemRepository : IToDoItemRepository
    {

        private readonly IAmbientDbContextLocator ambientDbContextLocator;

        private DbContext context
        {
            get
            {
                var dbContext = this.ambientDbContextLocator.Get<EFDbContext>();
                if (dbContext == null)
                {
                    throw new InvalidOperationException("It is impossible to use repository because DbContextScope has not been created.");
                }
                return dbContext;
            }
        }

        public ToDoItemRepository(IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (ambientDbContextLocator == null)
            {
                throw new System.ArgumentNullException("ambientDbContextLocator", "Ambient dbContext locator is null.");
            }
            this.ambientDbContextLocator = ambientDbContextLocator;
        }


        public IEnumerable<ToDoModel> GetAll()
        {
            IEnumerable<ORM.Model.ToDoModel> result = this.context.Set<ORM.Model.ToDoModel>();
            return result.Select(q => q.ToDal()).ToList();
        }

        public ToDoModel Create(ToDoModel todo)
        {
            this.context.Set<ORM.Model.ToDoModel>().Add(todo.ToOrm());
            this.context.SaveChanges();
            return this.GetById(todo.Id);
        }

        public ToDoModel GetById(int id)
        {
            ToDoModel result = null;
            var ormResult = this.GetOrmToDoModel(id);
            if (ormResult != null)
            {
                result = ormResult.ToDal();
            }
            return result;
        }

        public bool RemoveById(int id)
        {
            var result = this.GetOrmToDoModel(id);
            if (result != null)
            {
                this.context.Set<ORM.Model.ToDoModel>().Remove(result);
                this.context.SaveChanges();
                return true;
            }
            return false;
        }

        public ToDoModel Update(Interface.Entities.ToDoModel todo)
        {
            var result = this.GetOrmToDoModel(todo.Id);
            if (result != null)
            {
                result.Description = todo.Description;
                result.IsDone = todo.IsDone;
                this.context.SaveChanges();
            }
            return this.GetById(todo.Id);
        }


        private ORM.Model.ToDoModel GetOrmToDoModel(int modelId)
        {
            ORM.Model.ToDoModel result = null;
            var query = this.context.Set<ORM.Model.ToDoModel>().Where(q => q.Id == modelId);
            if (query.Count() != 0)
            {
                result = query.First();
            }
            return result;
        }
    }
}
