using BLL.Infrastructure;
using System.Security.Principal;

namespace BLL.Security
{
    internal class UserProvider : IPrincipal
    {
        private readonly UserIndentity _userIdentity;
        private readonly IUserService _userService;

        #region .ctor
        public UserProvider(IUserService userService, string id)
        {
            _userService = userService;
            _userIdentity = new UserIndentity(userService, id);
        } 
        #endregion

        #region Identity
        public IIdentity Identity
        {
            get { return _userIdentity; }
        } 
        #endregion

        #region IsInRole
        public bool IsInRole(string role)
        {
            if (_userService == null) return false;
            return
                _userService.IsInRole(_userIdentity.User.UserID, role);
        } 
        #endregion

    }
}