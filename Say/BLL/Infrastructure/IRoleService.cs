#region using
using DAL.Entities;
using DAL.Infrastructure;
using System.Collections.Generic; 
#endregion

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
