#region using
using BLL.Infrastructure;
using BLL.Security.Infrastructure;
using BLL.Security.Principal;
using System;
using System.Security.Principal; 
#endregion

namespace BLL.Security.Contexts
{
    public class MockContext : IAppContext
    {
        #region Fields&Props
        private IPrincipal _user = new DefaultPrincipal(null, null);
        private IUserService _userService; 
        #endregion

        #region .ctors
        public MockContext(IUserService userService)
        {
            if (userService == null)
                throw new ArgumentNullException("Service is null", (Exception)null);
            _userService = userService;
        } 
        #endregion

        #region GetUserData
        public string GetUserData()
        {
            return UserData.Data;
        } 
        #endregion

        #region SetUserData
        public void SetUserData(string data)
        {
            if (String.IsNullOrEmpty(data) || String.IsNullOrWhiteSpace(data))
                throw new ArgumentNullException("Data is null or empty", (Exception)null);
            UserData.Data = data;
            _user = new DefaultPrincipal(_userService, data);
        } 
        #endregion

        #region DeleteUserData
        public void DeleteUserData()
        {
            UserData.Data = null;
            _user = new DefaultPrincipal(null, null);
        } 
        #endregion

        #region User
        public IPrincipal User
        {
            get
            {
                return
                    _user != null ? _user :
                    _user = new DefaultPrincipal(_userService, GetUserData());
            }
            set { _user = value; }
        }
        #endregion
    }

    #region UserData
    public static class UserData
    {
        public static string Data { get; set; }
    } 
    #endregion
}
