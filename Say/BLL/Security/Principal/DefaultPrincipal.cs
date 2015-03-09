#region using
using BLL.Infrastructure;
using System;
using System.Security.Principal; 
#endregion

namespace BLL.Security.Principal
{
    public class DefaultPrincipal : IPrincipal
    {
        #region Fields&Props
        private readonly IIdentity _userIdentity;
        private readonly IUserService _userService;

        #region Identity
        public IIdentity Identity
        {
            get { return _userIdentity; }
        }
        #endregion

        #region IsInRole
        public bool IsInRole(string role)
        {
            if ((_userService != null) && (_userIdentity.Name != null))
            {
                int i;
                if (Int32.TryParse(_userIdentity.Name, out i))
                    return _userService.IsInRole(i, role);
            }
            return false;
        }
        #endregion

        #endregion

        #region .ctors
        public DefaultPrincipal(IUserService userService, string id)
        {
            _userService = userService;
            _userIdentity = new DefaultIndentity(userService, id);
        } 
        #endregion
    }
}