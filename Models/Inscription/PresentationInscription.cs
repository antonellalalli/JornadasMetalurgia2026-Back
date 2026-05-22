using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jornadas_Metalurgia_2026.Models.Inscription
{
    public class PresentationInscription: Inscription
    {

        [Required]
        public string PresentationTitle { get; set; }

        [Required]
        [Column(TypeName = "text[]")]
        public List<String> PresentationParticipants { get; set; } = new();
        //file
        [Required]
        public string Presentation { get; set; }
    }
}
