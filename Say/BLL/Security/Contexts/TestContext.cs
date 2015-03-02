using BLL.Security.Infrastructure;
using System.Security.Principal;

namespace BLL.Security
{
    internal class TestContext : IContext
    {
        private IPrincipal _user = new UserProvider(null, null);

        #region GetUserData
        public string GetUserData()
        {
            return UserData.Data;
        } 
        #endregion

        #region User
        public IPrincipal User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }
        #endregion

        #region SetUserData
        public void SetUserData(string data)
        {
            UserData.Data = data;
        } 
        #endregion

        #region DeleteUserData
        public void DeleteUserData()
        {
            UserData.Data = null;
        } 
        #endregion
    }

    public static class UserData
    {
        public static string Data { get; set; }
    }
}
