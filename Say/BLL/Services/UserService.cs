using BLL.Infrastructure;
using DAL.Entities;
using DAL.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        #region .ctors
        public UserService()
            : this(DependencyResolution.Kernel.Get<IUserRepository>())
        { }

        public UserService(IUserRepository userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException("Repository is null", (Exception)null);
            _userRepository = userRepository;
        } 
        #endregion

        #region Users
        public IEnumerable<User> Users
        {
            get { return _userRepository.Users.AsEnumerable<User>(); }
        } 
        #endregion

        #region GetUserByID
        public User GetUserByID(int userID)
        {
            return _userRepository.GetUserByID(userID);
        } 
        #endregion

        #region IsExist
        public bool IsExist(string username)
        {
            User user = Users.FirstOrDefault(u => u.Username == username);
            return user != null;
        } 
        #endregion

        #region SaveUser
        public void SaveUser(User user)
        {
            try
            {
                _userRepository.SaveUser(user);
            }
            catch (ArgumentException exc) { throw exc; }
        } 
        #endregion

        #region DeleteUser
        public void DeleteUser(int userID)
        {
            _userRepository.DeleteUser(userID);
        } 
        #endregion

        #region DisableUser
        public void DisableUser(int userID)
        {
            User user = GetUserByID(userID);
            user.IsDeleted = true;
            try { SaveUser(user); }
            catch (ArgumentException exc)
            { throw exc; }
        } 
        #endregion

        #region IsInRole
        public bool IsInRole(int userID, string role)
        {
            try
            {
                User user = GetUserByID(userID);
                int roleID = user.Role;
                RoleService roleService = new RoleService();
                RoleEntity roleEntity = roleService.GetRoleByID(roleID);
                return String.Equals(roleEntity.Role, role, StringComparison.InvariantCultureIgnoreCase);
            }
            catch (Exception exc)
            { throw new Exception("Repository error", exc); }
        } 
        #endregion
    }
}
