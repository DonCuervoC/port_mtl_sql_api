using API_port_montreal.Models;
using API_port_montreal.Models.Dtos;
using API_port_montreal.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace API_port_montreal.Controllers
{
    // Route  HTTP pour les opérations  CRUD arrivées
    [Route("api/arrivees")]
    [ApiController]
    public class ArriveesController : ControllerBase
    {

        private readonly IArriveesRepository _arrRepo; // nous pouvons avoir accès à toutes les méthodes du référentiel
        private readonly IMapper _mapper; // instanciation de l'automapper pour le ctos

        public ArriveesController(IArriveesRepository arrRepo, IMapper mapper)
        {
            _arrRepo = arrRepo; //  accéder au repository avec les méthodes  
            _mapper = mapper; // lien entre le modèle et le dto et vice versa

        }



        // Endpoint pour obtenir toutes les arrivées

        // Cette méthode obtient la liste de toutes les arrivées.
        // Elle ne nécessite aucun paramètre et renvoie un statut HTTP 200 (OK) avec la liste des arrivées.
        // En cas d'erreur, elle peut renvoyer un statut HTTP 404 (Not Found).
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetArrivees()
        {
            // Récupérer la liste de toutes les arrivées
            var arriveesList = _arrRepo.GetArrivees();
            // Créer une liste pour stocker les DTO des arrivées
            var listArriveesDto = new List<ArriveesDto>();
            // Mapper chaque arrivée de la liste d'origine vers le DTO correspondant
            foreach (var list in arriveesList)
            {
                listArriveesDto.Add(_mapper.Map<ArriveesDto>(list));
            }

            return Ok(listArriveesDto);
        }



        // Endpoint pour obtenir une arrivée par ID

        // Cette méthode permet d'obtenir une arrivée spécifique en fournissant son ID.
        // Elle prend un paramètre dans l'URL : arriveeId (de type int).
        // Si l'arrivée est trouvée, elle renvoie un statut HTTP 200 (OK) avec les détails de l'arrivée.
        // En cas d'erreur ou si l'arrivée n'est pas trouvée, elle peut renvoyer différents statuts HTTP : 404 (Not Found), 403 (Forbidden), ou 400 (Bad Request).
        [HttpGet("{arriveeId:int}", Name = "GetArrivee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetArrivee(int arriveeId)
        {
            // Récupérer l'arrivée spécifique en utilisant l'ID fourni
            var itemArrivee = _arrRepo.GetArrivee(arriveeId);
            // Si l'arrivée n'est pas trouvée, renvoyer un statut HTTP 404 (Not Found)
            if (itemArrivee == null)
            {
                return NotFound();
            }
            // Mapper l'arrivée récupérée vers un DTO
            var itemArriveeDto = _mapper.Map<ArriveesDto>(itemArrivee);
            // Retourner l'arrivée sous forme de DTO avec un statut HTTP 200 (OK)
            return Ok(itemArriveeDto);
        }




        // Endpoint pour créer une nouvelle arrivée

        // Cette méthode permet de créer une nouvelle arrivée en fournissant les détails dans le corps de la requête.
        // Elle prend un objet CreateArriveeDto comme paramètre dans le corps de la requête, avec les champs suivants :
        // - nomNavire : string
        // - portOrigine : string
        // - terminal : string
        // - typeCargaison : string
        // Si la création est réussie, elle renvoie un statut HTTP 201 (Created) avec les détails de l'arrivée créée.
        // En cas d'erreur, elle peut renvoyer différents statuts HTTP : 400 (Bad Request) ou 500 (Internal Server Error).
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ArriveesDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateArrivee([FromBody] CreateArriveeDto createArriveeDto)
        {
            // Vérifie si le modèle de la requête est valide
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Vérifie si le DTO de création est null
            if (createArriveeDto == null)
            {
                return BadRequest(ModelState);
            }

            // Mappe le DTO de création à un objet Arrivees
            var arrivee = _mapper.Map<Arrivees>(createArriveeDto);
            // Crée l'arrivée en utilisant le référentiel
            if (!_arrRepo.CreateArrivee(arrivee))
            {
                ModelState.AddModelError("", $"Un problème s'est produit lors de l'enregistrement de {arrivee.NomNavire}");
                return StatusCode(500, ModelState);
            }
            // Retourne le statut HTTP 201(Created) avec les détails de l'arrivée créée
            return CreatedAtRoute("GetArrivee", new { arriveeId = arrivee.Id }, arrivee);
        }




        // Mise à jour d'une arrivée

        // Cette méthode permet de mettre à jour une arrivée existante en fournissant les détails dans le corps de la requête.
        // Elle prend un objet ArriveesDto comme paramètre dans le corps de la requête, avec les champs suivants :
        // - id : int (doit être fourni pour identifier l'enregistrement à modifier)
        // - nomNavire : string
        // - portOrigine : string
        // - terminal : string
        // - typeCargaison : string
        // Si la mise à jour est réussie, elle renvoie un statut HTTP 204 (No Content).
        // En cas d'erreur, elle peut renvoyer différents statuts HTTP : 400 (Bad Request), 404 (Not Found) ou 500 (Internal Server Error).

        [HttpPatch("{arriveeId:int}", Name = "UpdatePatchArrivees")]
        [ProducesResponseType(201, Type = typeof(ArriveesDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatchArrivees(int arriveeId, [FromBody] ArriveesDto arriveeDto)
        {
            // Vérifie si le modèle de la requête est valide
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mappe le DTO d'arrivée à un objet Arrivees
            var arrivee = _mapper.Map<Arrivees>(arriveeDto);

            // Vérifie si l'arrivée avec l'ID donné existe
            if (!_arrRepo.ExistArriveeById(arrivee.Id) || !_arrRepo.ExistArriveeById(arriveeId))
            {
                return NotFound();
            }

            // Met à jour l'arrivée en utilisant le référentiel
            if (!_arrRepo.UpdateArrivee(arrivee))
            {
                ModelState.AddModelError("", $"Un problème s'est produit lors de mise à jour de {arrivee.NomNavire} avec id {arrivee.Id}");
                return StatusCode(500, ModelState);
            }

            // Retourne le statut HTTP 204 (No Content) pour indiquer que la mise à jour a été effectuée avec succès
            return NoContent();
        }





        // Supprimer une arrivée

        // Cette méthode permet de supprimer une arrivée existante en fournissant l'ID de l'arrivée dans l'URL.
        // Elle prend un paramètre arriveId dans l'URL, qui est un entier représentant l'ID de l'arrivée à supprimer.
        // Si la suppression est réussie, elle renvoie un statut HTTP 204 (No Content).
        // En cas d'erreur, elle peut renvoyer différents statuts HTTP : 400 (Bad Request), 403 (Forbidden), ou 404 (Not Found).
        [HttpDelete("{arriveId:int}", Name = "DeleteArrivee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteArrivee(int arriveId)
        {
            // Vérifie si l'arrivée avec l'ID donné existe
            if (!_arrRepo.ExistArriveeById(arriveId))
            {
                return NotFound();
            }

            var arrivee = _arrRepo.GetArrivee(arriveId);

            // Supprime l'arrivée en utilisant le référentiel
            if (!_arrRepo.DeleteArrivee(arrivee))
            {
                ModelState.AddModelError("", $"Un problème s'est produit lors de l'élimination de {arrivee.NomNavire} avec id {arrivee.Id}");
                return StatusCode(500, ModelState);
            }

            // Retourne le statut HTTP 204 (No Content) pour indiquer que la suppression a été effectuée avec succès
            return NoContent();
        }

    }
}
