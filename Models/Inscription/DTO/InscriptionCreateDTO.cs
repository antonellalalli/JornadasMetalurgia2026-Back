using System.ComponentModel.DataAnnotations;

namespace Jornadas_Metalurgia_2026.Models.Inscription.DTO
{
    public class InscriptionCreateDTO
    {
        [Required]
        public string StudentName { get; set; }
        [Required]
        public string StudentEmail { get; set; }
        [Required]
        public int StudentDni { get; set; }
        [Required]

        public string StudentInstitution { get; set; }
        [Required]
        public string InscriptionType { get; set; }
        public string? PresentationTitle { get; set; }
        public List<string>? Participants { get; set; }
        public IFormFile? Presentation { get; set; }
    }
}
