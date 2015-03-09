#region using
using BLL.Infrastructure;
using BLL.Security.Contexts;
using BLL.Security.Infrastructure;
using BLL.Services;
using DAL.Infrastructure;
using Ninject;
using Ninject.Web.Common;
using RpR.Infrastructure;
using RpR.MethodInvokers;
using RpR.ModelBinders;
using RpR.RequestEngineFactories;
using RpR.ResponseEngines.Infrastructure;
#endregion

namespace RpR
{
    public class DependencyResolution
    {
        #region Fields&Props
        public static readonly IKernel Kernel; 
        #endregion

        #region .ctors
        static DependencyResolution()
        {
            Kernel = new StandardKernel();
            AddBindings();
        } 
        #endregion

        #region AddBindings
        private static void AddBindings()
        {
            Kernel.Bind<IRequestEngineFactory>().To<RequestEngineFactory>()
                .WithConstructorArgument("methodInvoker", (IMethodInvoker)null);
            Kernel.Bind<IMethodInvoker>().To<MethodInvoker>()
                .WithConstructorArgument("binder", (IModelBinder)null);
            Kernel.Bind<IModelBinder>().To<ModelBinder>();
            Kernel.Bind<IUserService>().To<UserService>()
                .WithConstructorArgument("userRepository", (IUserRepository)null)
                .WithConstructorArgument("roleService", (IRoleService)null);
            Kernel.Bind<IRoleService>().To<RoleService>()
                .WithConstructorArgument("roleRepository", (IRoleRepository)null);
            Kernel.Bind<IFriendshipService>().To<FriendshipService>()
                .WithConstructorArgument("friendshipRepository", (IFriendshipRepository)null)
                .WithConstructorArgument("userService", (IUserService)null);
            Kernel.Bind<IMessageService>().To<MessageService>()
                .WithConstructorArgument("messageRepository", (IMessageRepository)null)
                .WithConstructorArgument("userService", (IUserService)null);
            Kernel.Bind<IResponseStrategies>().To<ResponseStrategies>();
            Kernel.Bind<IResponse>().To<Response>();
            Kernel.Bind<IAppContext>().To<WebContext>().InRequestScope()
                .WithConstructorArgument("cookieName", (string)null);
        } 
        #endregion
    }
}
