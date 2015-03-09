#region using
using BLL.Infrastructure;
using BLL.Security;
using BLL.Security.Infrastructure;
using BLL.Security.Validators;
using BLL.Services;
using DAL.Infrastructure;
using DAL.Repositories;
using Ninject; 
#endregion

namespace BLL
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
            Kernel.Bind<IUserRepository>().To<UserRepository>();
            Kernel.Bind<IMessageRepository>().To<MessageRepository>();
            Kernel.Bind<IRoleRepository>().To<RoleRepository>();
            Kernel.Bind<IFriendshipRepository>().To<FriendshipRepository>();
        } 
        #endregion
    }
}
