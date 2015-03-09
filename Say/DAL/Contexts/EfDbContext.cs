#region using
using DAL.Entities;
using System.Data.Entity; 
#endregion

namespace DAL.Contexts
{
    class EfDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
    }
}
