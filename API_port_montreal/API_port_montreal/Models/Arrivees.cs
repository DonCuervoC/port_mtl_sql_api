using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;

namespace API_port_montreal.Models
{
    // Classe de base avec valeurs par défaut dans les tables SQL
    public class Arrivees
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NomNavire { get; set; }
        [Required]
        public DateTime DateHeureArrivee { get; set; }
        [Required]
        public string PortOrigine { get; set; }
        [Required]
        public string Terminal { get; set; }
        [Required]
        public string TypeCargaison { get; set; }

    }
}

/*
CREATE TABLE Arrivees (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NomNavire NVARCHAR(255) NOT NULL,
    DateHeureArrivee DATETIME NOT NULL,
    PortOrigine NVARCHAR(255) NOT NULL,
    Terminal NVARCHAR(255) NOT NULL
);
*/