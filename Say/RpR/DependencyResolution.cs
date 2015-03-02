using BLL.Infrastructure;
using BLL.Services;
using Ninject;
using RpR.ActionInvokers;
using RpR.Infrastructure;
using RpR.ModelBinders;
using RpR.RequestEngineFactories;

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
        } 
        #endregion
    }
}
