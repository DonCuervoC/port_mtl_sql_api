using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_port_montreal.Models
{

    // Classe de base avec valeurs par défaut dans les tables SQL
    public class Departs
    {
        public int Id { get; set; }

        [Required]
        public string NomNavire { get; set; }

        [Required]
        public DateTime DateHeureDepart { get; set; }

        [Required]
        public string PortDestination { get; set; }

        [Required]
        public string Quai { get; set; }

        public int ArriveeId { get; set; }

    }
}


/*
 CREATE TABLE Departs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NomNavire NVARCHAR(255) NOT NULL,
    DateHeureDepart DATETIME NOT NULL,
    PortDestination NVARCHAR(255) NOT NULL,
    Quai NVARCHAR(255) NOT NULL,
     INT, -- Cette colonne contient l'identifiant de l'arrivée correspondante.
    FOREIGN KEY (ArriveeId) REFERENCES Arrivees(Id)
);

 */