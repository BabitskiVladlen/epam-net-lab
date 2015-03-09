#region using
using BLL.Infrastructure;
using BLL.Security.Contexts;
using BLL.Security.Infrastructure;
using BLL.Security.PasswordEngines;
using BLL.Services;
using DAL.Entities;
using System;
using System.Linq; 
#endregion

namespace BLL.Security
{
    public class DefaultAuthentication : IAuthentication
    {
        #region Fields&Props
        private readonly IAppContext _context;
        private readonly IPasswordEngine _passwordEngine;
        private readonly IUserService _userService; 
        #endregion

        #region .ctors
        public DefaultAuthentication(IUserService userService = null, IPasswordEngine passwordEngine = null,
            IAppContext context = null)
        {
            _userService = userService ?? new UserService();
            _passwordEngine = passwordEngine ?? new MD5PasswordEngine();
            _context = context ?? new WebContext(_userService);
        }
        #endregion

        #region SignIn
        public User SignIn(string name, string password)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Name is null or empty", (Exception)null);
            if (String.IsNullOrEmpty(password) || String.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password is null or empty", (Exception)null);

            User user = _userService.Users
                    .FirstOrDefault(u => String.Equals(u.Username, name, StringComparison.InvariantCultureIgnoreCase));
            if ((user != null) &&
                (_passwordEngine.Verify(password, user.Password)))
                _context.SetUserData(user.UserID.ToString());
            return user;
        } 
        #endregion

        #region Logout
        public void Logout()
        {
            _context.DeleteUserData();
        } 
        #endregion
    }
}