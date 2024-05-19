using System.ComponentModel.DataAnnotations;

namespace API_port_montreal.Models.Dtos
{
    ////on utilise DTO pour ne pas exposer directement le modèle dans l'API.
    public class CreateArriveeDto
    {
        // nous n'utilisons pas l'id car il est automatiquement autoincrémenté.

        [Required(ErrorMessage = "Le nom du navire est obligatoire")]
        [MaxLength(30, ErrorMessage = "Max 30 characteres are allowed")]
        public string NomNavire { get; set; }

        [Required(ErrorMessage = "Le port d'origin du navire est obligatoire")]
        [MaxLength(30, ErrorMessage = "Max 30 characteres are allowed")]
        public string PortOrigine { get; set; }

        [Required(ErrorMessage = "La terminal du navire est obligatoire")]
        [MaxLength(30, ErrorMessage = "Max 30 characteres are allowed")]
        public string Terminal { get; set; }

        [Required(ErrorMessage = "Le type cargaison du navire est obligatoire")]
        [MaxLength(30, ErrorMessage = "Max 30 characteres are allowed")]
        public string TypeCargaison { get; set; }

        // nous n'avons pas besoin de la date car elle est générée dynamiquement dans le repository.

    }
}
