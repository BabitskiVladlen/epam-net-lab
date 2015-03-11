using AmbientDbContext;
using AmbientDbContext.Interface;
using BLL.Concrete;
using BLL.Interface.Abstract;
using DAL;
using DAL.Interface.Abstract;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependencyResolver
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAmbientDbContextLocator>().To<AmbientDbContextLocator>();
            Bind<IDbContextScopeFactory>().To<DbContextScopeFactory>().WithConstructorArgument((IDbContextFactory)null);
            Bind<IToDoItemRepository>().To<ToDoItemRepository>();

            Bind<IToDoItemService>().To<ToDoItemService>();
            Bind<IToDoItemServiceCache>().To<ToDoItemServiceCache>();
            Bind<BLL.Service.IToDoManager>().To<BLL.Service.ToDoManagerClient>();
        }
    }
}
