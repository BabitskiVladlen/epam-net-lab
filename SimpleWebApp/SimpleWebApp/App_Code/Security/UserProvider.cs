using System.Security.Principal;

namespace SimpleWebApp.Security
{
    public class UserProvider : IPrincipal
    {
        private readonly UserIndentity _userIdentity;

        public UserProvider(string name)
        {
            _userIdentity = new UserIndentity();
            _userIdentity.Init(name);
        }

        public IIdentity Identity
        {
            get { return _userIdentity; }
        }

        public bool IsInRole(string role)
        {
            return true;
        }

    }
}