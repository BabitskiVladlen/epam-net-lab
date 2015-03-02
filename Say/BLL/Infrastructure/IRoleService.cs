using DAL.Entities;
using DAL.Infrastructure;
using System.Collections.Generic;

namespace BLL.Infrastructure
{
    public interface IRoleService
    {
        IEnumerable<RoleEntity> Roles { get; }
        RoleEntity GetRoleByID(int roleID);
        void SaveRole(RoleEntity role);
        void DeleteRole(int roleID);
    }
}
