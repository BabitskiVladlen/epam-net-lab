#region using
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
#endregion

namespace DAL.Entities
{
    [Table("Roles")]
    public class RoleEntity
    {
        [Key]
        public int RoleID { get; set; }
        public string Role { get; set; }
    }
}
