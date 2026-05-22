using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jornadas_Metalurgia_2026.Models.Inscription
{
    public class Inscription
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string StudentName { get; set; }

        [Required]
        [EmailAddress]
        public string StudentEmail { get; set; }
        [Required]
        public int StudentDni { get; set; }
        [Required]
        public string StudentInstitution { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
