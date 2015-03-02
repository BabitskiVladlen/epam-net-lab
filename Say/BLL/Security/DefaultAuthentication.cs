using BLL.Infrastructure;
using BLL.Security.Infrastructure;
using BLL.Services;
using DAL.Entities;
using System;
using System.Linq;
using System.Security.Principal;

namespace BLL.Security
{
    public class DefaultAuthentication : IAuthentication
    {
        private IPrincipal _currentUser;
        private IContext _context;
        private IPasswordEngine _passwordEngine;
        private IUserService _userService;

        #region .ctors
        public DefaultAuthentication(IContext context)
            : this(context, new UserService(), new PasswordEngineMD5())
        { }

        public DefaultAuthentication(IContext context, IUserService userService)
            : this(context, userService, new PasswordEngineMD5())
        { }

        public DefaultAuthentication(IContext context, IPasswordEngine passwordEngine)
            : this(context, new UserService(), passwordEngine)
        { }

        public DefaultAuthentication(IContext context, IUserService userService, IPasswordEngine passwordEngine)
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
            {
                _context.SetUserData(user.UserID.ToString());
                _context.User = new UserProvider(_userService, user.UserID.ToString());
            }
            return user;
        } 
        #endregion

        #region Logout
        public void Logout()
        {
            _context.DeleteUserData();
            _currentUser = new UserProvider(null, null);
            _context.User = _currentUser;
        } 
        #endregion

        #region CurrentUser
        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser != null) return _currentUser;
                _currentUser = new UserProvider(_userService, _context.GetUserData());
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        } 
        #endregion
    }
}