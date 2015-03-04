#region using
using BLL.Infrastructure;
using BLL.Security;
using BLL.Security.Contexts;
using BLL.Security.Infrastructure;
using BLL.Services;
using Ninject;
using Ninject.Web.Common;
using RpR.Infrastructure;
using RpR.MethodInvokers;
using RpR.ModelBinders;
using RpR.RequestEngineFactories;
using RpR.ResponseEngines;
using RpR.ResponseEngines.Infrastructure.Infrastructure; 
#endregion

namespace RpR
{
    public class DependencyResolution
    {
        public static readonly IKernel Kernel;

        #region .ctor
        static DependencyResolution()
        {
            Kernel = new StandardKernel();
            AddBindings();
        } 
        #endregion

        #region AddBindings
        private static void AddBindings()
        {
            Kernel.Bind<IRequestEngineFactory>().To<RequestEngineFactory>();
            Kernel.Bind<IMethodInvoker>().To<MethodInvoker>();
            Kernel.Bind<IModelBinder>().To<ModelBinder>();
            Kernel.Bind<IUserService>().To<UserService>();
            Kernel.Bind<IRoleService>().To<RoleService>();
            Kernel.Bind<IFriendshipService>().To<FriendshipService>();
            Kernel.Bind<IMessageService>().To<MessageService>();
            Kernel.Bind<IResponse>().To<Response>();
            Kernel.Bind<IAuthentication>().To<DefaultAuthentication>();
            Kernel.Bind<IRegistration>().To<DefaultRegistration>();
            Kernel.Bind<IAppContext>().To<WebContext>().InRequestScope();
        } 
        #endregion
    }
}
