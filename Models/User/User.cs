using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jornadas_Metalurgia_2026.Models.User
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; } = null;
        [Required]
        public string Email { get; set; } = null;
        [Required]
        [StringLength(300)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null;
        public List<Role.Role> Roles { get; set; } = new();
    }
}
