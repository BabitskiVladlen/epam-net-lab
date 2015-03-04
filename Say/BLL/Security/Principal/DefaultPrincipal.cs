using BLL.Infrastructure;
using System.Security.Principal;

namespace BLL.Security.Principal
{
    public class DefaultPrincipal : IPrincipal
    {
        private readonly DefaultIndentity _userIdentity;
        private readonly IUserService _userService;

        #region .ctors
        public DefaultPrincipal(IUserService userService, string id)
        {
            _userService = userService;
            _userIdentity = new DefaultIndentity(userService, id);
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
            return
                _userService != null ?
                _userService.IsInRole(_userIdentity.User.UserID, role) : false;
        } 
        #endregion

    }
}