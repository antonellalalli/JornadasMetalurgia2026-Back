using System.ComponentModel.DataAnnotations;

namespace Jornadas_Metalurgia_2026.Models.Inscription.DTO
{
    public class InscriptionResponseDTO
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
       
        public string StudentEmail { get; set; }
        
        public int StudentDni { get; set; }
        

        public string StudentInstitution { get; set; }
        
        public string InscriptionType { get; set; }
        public string? PresentationTitle { get; set; }
        public string? PresentationParticipants { get; set; }
        public string? Presentation { get; set; }
    }
}
