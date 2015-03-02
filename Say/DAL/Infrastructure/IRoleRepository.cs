using DAL.Entities;
using System.Linq;

namespace DAL.Infrastructure
{
    public interface IRoleRepository
    {
        IQueryable<RoleEntity> Roles { get; }
        RoleEntity GetRoleByID(int roleID);
        void SaveRole(RoleEntity role);
        void DeleteRole(int roleID);
    }
}
