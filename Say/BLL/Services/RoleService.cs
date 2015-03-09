#region using
using BLL.Infrastructure;
using DAL.Entities;
using DAL.Infrastructure;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq; 
#endregion

namespace BLL.Services
{
    public class RoleService : IRoleService
    {
        #region Fields&Props
        private readonly IRoleRepository _roleRepository; 
        #endregion

        #region .ctors
        public RoleService(IRoleRepository roleRepository = null)
        {
            _roleRepository = roleRepository ?? DependencyResolution.Kernel.Get<IRoleRepository>();
        } 
        #endregion

        #region Roles
        public IEnumerable<RoleEntity> Roles
        {
            get { return _roleRepository.Roles.AsEnumerable<RoleEntity>(); }
        } 
        #endregion

        #region GetRoleByID
        public RoleEntity GetRoleByID(int roleID)
        {
            return _roleRepository.GetRoleByID(roleID);
        } 
        #endregion

        #region SaveRole
        public void SaveRole(RoleEntity role)
        {
            try { _roleRepository.SaveRole(role); }
            catch (ArgumentException exc) { throw exc; }
        } 
        #endregion

        #region DeleteRole
        public void DeleteRole(int roleID)
        {
            _roleRepository.DeleteRole(roleID);
        }
        #endregion
    }
}
