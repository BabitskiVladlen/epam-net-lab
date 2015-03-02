using SimpleWebApp.Security.Interfaces;
using System;
using System.Linq;

namespace SimpleWebApp.Security
{
    public class UserIndentity : IUserIdentity
    {
        public User User { get; set; }

        public string AuthenticationType
        {
            get { return "Basic authentication."; }
        }

        public bool IsAuthenticated
        {
            get { return User != null; }
        }

        public string Name
        {
            get
            {
                return User != null ? User.UserName : null;
            }
        }

        public void Init(string name)
        {
            if ((name != null) && (Users.AllUsers != null))
                User = Users.AllUsers.FirstOrDefault
                    (u => String.Equals(u.UserName, name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}