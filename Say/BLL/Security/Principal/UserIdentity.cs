using BLL.Infrastructure;
using BLL.Security.Infrastructure;
using DAL.Entities;
using System;
using System.Linq;

namespace BLL.Security
{
    internal class UserIndentity : IUserIdentity
    {
        private readonly IUserService _userService;

        #region .ctor
        public UserIndentity(IUserService userService, string id)
        {
            _userService = userService;
            int i;
            if (Int32.TryParse(id, out i))
                User = _userService.GetUserByID(i);
        } 
        #endregion

        #region User
        public User User { get; set; } 
        #endregion

        #region AuthenticationType
        public string AuthenticationType
        {
            get { return "Basic authentication."; }
        } 
        #endregion

        #region IsAuthenticted
        public bool IsAuthenticated
        {
            get { return (User != null) && !(User.IsDeleted); }
        } 
        #endregion

        #region Name
        public string Name
        {
            get
            {
                return User != null ? User.Username : null;
            }
        } 
        #endregion
    }
}