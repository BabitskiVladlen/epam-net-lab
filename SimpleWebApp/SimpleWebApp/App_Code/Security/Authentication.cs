using SimpleWebApp;
using SimpleWebApp.Security.Interfaces;
using System;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SimpleWebApp.Security
{
    public static class Authentication
    {
        public const string CookieName = "__auth_cookie";

        private static IPrincipal _currentUser;

        public static HttpContext HttpContext { get; set; }

        public static User Login(string name, string password)
        {
            if (HttpContext == null)
                throw new InvalidOperationException("HttpContext is null");
            if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Name is null or empty", (Exception)null);
            if (String.IsNullOrEmpty(password) || String.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Password is null or empty", (Exception)null);

            if (Users.AllUsers == null) return null;
            User user = Users.AllUsers
                .FirstOrDefault(u => String.Equals(u.UserName, name, StringComparison.InvariantCultureIgnoreCase) &&
                Password.Verify(password, u.Password));
            if (user != null)
            {
                Cookie.Create(user.UserName, CookieName, HttpContext);
                HttpContext.User = new UserProvider(user.UserName);
            }
            return user;
        }

        public static void Logout()
        {
            if (HttpContext == null)
                throw new InvalidOperationException("HttpContext is null");
            Cookie.Delete(CookieName, HttpContext);
            _currentUser = new UserProvider(null);
            HttpContext.User = _currentUser;
        }

        public static IPrincipal CurrentUser
        {
            get
            {
                if (HttpContext == null)
                    throw new InvalidOperationException("HttpContext is null");
                if (_currentUser != null) return _currentUser;
                var ticket = Cookie.Read(CookieName, HttpContext);
                if ((ticket != null) && (ticket.Name != null))
                    _currentUser = new UserProvider(ticket.Name);
                else _currentUser = new UserProvider(null);
                return _currentUser;
            }
            set
            {
                _currentUser = value;
            }
        }
    }
}