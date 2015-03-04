#region using
using BLL.Infrastructure;
using BLL.Security.Contexts;
using BLL.Security.Infrastructure;
using BLL.Security.PasswordEngines;
using BLL.Security.Principal;
using BLL.Services;
using DAL.Entities;
using System;
using System.Linq; 
#endregion

namespace BLL.Security
{
    public class DefaultAuthentication : IAuthentication
    {
        private readonly IAppContext _context;
        private readonly IPasswordEngine _passwordEngine;
        private readonly  IUserService _userService;

        #region .ctors
        public DefaultAuthentication()
            : this(new UserService(), new PasswordEngineMD5(), new WebContext())
        { }

        public DefaultAuthentication(IPasswordEngine passwordEngine)
            : this(new UserService(), passwordEngine, new WebContext())
        { }

        public DefaultAuthentication(IUserService userService)
            : this(userService, new PasswordEngineMD5(), new WebContext(userService))
        { }

        public DefaultAuthentication(IUserService userService, IAppContext context)
            : this(userService, new PasswordEngineMD5(), context)
        { }

        public DefaultAuthentication(IUserService userService, IPasswordEngine passwordEngine, IAppContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null", (Exception)null);
            if (userService == null)
                throw new ArgumentNullException("Service is null", (Exception)null);
            if (passwordEngine == null)
                throw new ArgumentNullException("Password engine is null", (Exception)null);

            _context = context;
            _userService = userService;
            _passwordEngine = passwordEngine;
        }
        #endregion

        #region Login
        public User Login(string name, string password)
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