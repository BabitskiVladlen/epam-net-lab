#region using
using BLL.Infrastructure;
using DAL.Entities;
using System;
using System.Security.Principal;

#endregion

namespace BLL.Security.Principal
{
    public class DefaultIndentity : IIdentity
    {
        #region Fields&Props
        private readonly IUserService _userService;
        private readonly string _name;

        #region AuthenticationType
        public string AuthenticationType
        {
            get { return "Basic authentication"; }
        }
        #endregion

        #region IsAuthenticted
        public bool IsAuthenticated
        {
            get
            {
                return (_name != null) &&
                    !(_userService.GetUserByID(_name).IsDeleted);
            }
        }
        #endregion

        #region Name
        public string Name
        {
            get { return _name; }
        }
        #endregion 

        #endregion

        #region .ctors
        public DefaultIndentity(IUserService userService, string id)
        {
            if ((userService != null) &&
                !String.IsNullOrEmpty(id) &&
                !String.IsNullOrWhiteSpace(id))
            {
                User user = userService.GetUserByID(id);
                _name = (user != null) ? id : null;
                _userService = userService;
            }
        } 
        #endregion
    }
}