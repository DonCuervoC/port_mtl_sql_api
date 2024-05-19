using API_port_montreal.Data;
using API_port_montreal.Models.Dtos;
using API_port_montreal.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API_port_montreal.Controllers
{

    // Route  HTTP pour obtenir les rapports de la base de données
    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GenererRapportMensuel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenererRapportMensuel([FromQuery] int mois, [FromQuery] int annee)
        {
            // Vérifier si les paramètres sont valides
            if (mois < 1 || mois > 12 || annee < 1)
            {
                return BadRequest("Paramètres invalides.");
            }

            try
            {
                // Appeler la méthode asynchrone pour générer le rapport mensuel
                var result = await GenererRapportMensuelAsync(mois, annee);

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Retourner une réponse StatusCode 500 avec un message d'erreur en cas d'erreur interne du serveur
                return StatusCode(500, "Une erreur s'est produite lors du traitement de la demande.");
            }
        }


        // Méthode asynchrone pour générer le rapport mensuel
        // Explication de la fonction :
        /*
        - Cette méthode privée asynchrone est utilisée pour générer un rapport mensuel à partir de la base de données.
        - Elle prend en entrée le mois et l'année pour lesquels le rapport doit être généré.
        - Elle exécute une procédure stockée dans la base de données pour récupérer les données des arrivées et des départs pour le mois et l'année spécifiés.
        - Elle parcourt les résultats et construit des objets anonymes représentant les arrivées et les départs, puis les retourne dans un objet global contenant les deux listes.
        */
        private async Task<object> GenererRapportMensuelAsync(int mois, int annee)
        {
            // Listes pour stocker les arrivées et les départs
            var arrivees = new List<object>();
            var departs = new List<object>();

            // Utiliser une commande SQL pour exécuter la procédure stockée dans la base de données
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                // Spécifier la procédure stockée à exécuter
                command.CommandText = "GenererRapportMensuel";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Ajouter les paramètres pour le mois et l'année
                var moisParam = new SqlParameter("@Mois", mois);
                var anneeParam = new SqlParameter("@Annee", annee);

                // Ajouter les paramètres à la commande
                command.Parameters.Add(moisParam);
                command.Parameters.Add(anneeParam);

                // Ouvrir la connexion à la base de données de manière asynchrone
                await _context.Database.OpenConnectionAsync();

                // Exécuter la commande et récupérer les résultats
                using (var resultReader = await command.ExecuteReaderAsync())
                {
                    // Lire les résultats des 'arrivées'
                    while (await resultReader.ReadAsync())
                    {
                        // Créer un objet pour représenter une arrivée et l'ajouter à la liste des arrivées
                        var arrivee = new
                        {
                            Id = resultReader.GetInt32(0),
                            NomNavire = resultReader.GetString(1),
                            DateHeureArrivee = resultReader.GetDateTime(2),
                            PortOrigine = resultReader.GetString(3),
                            Terminal = resultReader.GetString(4)
                        };
                        arrivees.Add(arrivee);
                    }

                    // Passer au résultat suivant (départs)
                    await resultReader.NextResultAsync();

                    // Lire les résultats des 'départs'
                    while (await resultReader.ReadAsync())
                    {
                        // Créer un objet pour représenter un départ et l'ajouter à la liste des départs
                        var depart = new
                        {
                            Id = resultReader.GetInt32(0),
                            NomNavire = resultReader.GetString(1),
                            DateHeureDepart = resultReader.GetDateTime(2),
                            PortDestination = resultReader.GetString(3),
                            Quai = resultReader.GetString(4),
                            ArriveeId = resultReader.IsDBNull(5) ? (int?)null : resultReader.GetInt32(5)
                        };
                        departs.Add(depart);
                    }
                }
            }

            // Retourner un objet anonyme contenant les arrivées et les départs
            return new { Arrivees = arrivees, Departs = departs };
        }

    }
}
