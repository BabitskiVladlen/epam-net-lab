using BLL.Infrastructure;
using BLL.Security.Infrastructure;
using DAL.Entities;
using System;

namespace BLL.Security.Principal
{
    public class DefaultIndentity : IUserIdentity
    {
        #region .ctors
        public DefaultIndentity(IUserService userService, string id)
        {
            if ((userService != null) &&
                !String.IsNullOrEmpty(id) &&
                !String.IsNullOrWhiteSpace(id))
            {
                int i;
                if (Int32.TryParse(id, out i))
                User = userService.GetUserByID(i);
            }
        } 
        #endregion

        #region User
        public User User { get; set; } 
        #endregion

        #region AuthenticationType
        public string AuthenticationType
        {
            get { return "Basic authentication"; }
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
            get { return User != null ? User.Username : null; }
        } 
        #endregion
    }
}