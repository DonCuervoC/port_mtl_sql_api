using System.ComponentModel.DataAnnotations;

namespace API_port_montreal.Models.Dtos
{
    //on utilise DTO pour ne pas exposer directement le modèle dans l'API.
    public class CreateDepartsDto
    {

        public int Id { get; set; }

        public string NomNavire { get; set; }

        [Required(ErrorMessage = "Le port de destination du navire est obligatoire")]
        public string PortDestination { get; set; }

        [Required(ErrorMessage = "La quai du navire est obligatoire")]
        public string Quai { get; set; }

        public int ArriveeId { get; set; }
    }
}
