using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jornadas_Metalurgia_2026.Models.Role
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = null;

    }

    public class UserRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

    }
}

