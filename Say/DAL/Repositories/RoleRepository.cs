﻿using DAL.Contexts;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Validators;
using System;
using System.Linq;

namespace DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly EfDbContext _context = new EfDbContext();

        #region Roles
        public IQueryable<RoleEntity> Roles
        {
            get { return _context.Roles; }
        } 
        #endregion

        #region GetRoleByID
        public RoleEntity GetRoleByID(int roleID)
        {
            return _context.Roles.Find(roleID);
        } 
        #endregion

        #region SaveRole
        public void SaveRole(RoleEntity role)
        {
            RoleEntity exRole;
            try { Validator.RoleValidator(role, out exRole); }
            catch (ArgumentException exc)
            { throw exc; }

            if (role.RoleID == 0)
                _context.Roles.Add(role);
            else if (exRole != null)
                exRole.Role = role.Role;
            _context.SaveChanges();
        } 
        #endregion

        #region DeleteRole
        public void DeleteRole(int roleID)
        {
            RoleEntity role = GetRoleByID(roleID);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
            }
        } 
        #endregion
    }
}