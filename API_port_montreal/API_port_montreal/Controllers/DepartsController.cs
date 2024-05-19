using API_port_montreal.Models;
using API_port_montreal.Models.Dtos;
using API_port_montreal.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace API_port_montreal.Controllers
{
    // Route  HTTP pour les opérations  CRUD arrivées
    [Route("api/departs")]
    [ApiController]
    public class DepartsController : ControllerBase
    {
        private readonly IArriveesRepository _arrRepo;
        private readonly IDepartsRepository _depRepo; // nous pouvons avoir accès à toutes les méthodes du référentiel
        private readonly IMapper _mapper; // instanciation de l'automapper pour le ctos

        public DepartsController(IDepartsRepository depRepo, IMapper mapper, IArriveesRepository arrRepo)
        {
            _depRepo = depRepo; //  accéder au repository avec les méthodes  
            _mapper = mapper; // lien entre le modèle et le dto et vice versa
            _arrRepo = arrRepo;

        }

        // Obtenir la liste de toutes les départs

        // Cette méthode permet de récupérer tous les enregistrements de navires qui ont quitté le port.
        // Elle ne prend aucun paramètre et renvoie une liste de tous les départs trouvés dans la base de données, ordonnés par ID.
        // Si la récupération est réussie, elle renvoie un statut HTTP 200 (OK).
        // En cas d'erreur d'autorisation, elle peut renvoyer un statut HTTP 403 (Forbidden).
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetAllDeparts()
        {
            var departsList = _depRepo.GetAllDeparts();

            // Créer une liste pour stocker les DTO 
            var listDepartsDto = new List<DepartsDto>();

            // Mapper chaque départ de la liste d'origine vers le DTO correspondant
            foreach (var list in departsList)
            {
                listDepartsDto.Add(_mapper.Map<DepartsDto>(list));
            }

            // Retourne le statut HTTP 200 (OK) avec la liste de tous les départs
            return Ok(listDepartsDto);
        }






        // Obtenir un départ par son ID

        // Cette méthode permet de récupérer un enregistrement de navire qui a quitté le port en utilisant son ID.
        // Elle prend comme paramètre l'ID du départ et renvoie les détails de ce départ.
        // Si l'enregistrement est trouvé, elle renvoie un statut HTTP 200 (OK) avec les informations du départ.
        // En cas d'erreur de requête ou si l'ID est invalide, elle renvoie un statut HTTP 400 (Bad Request).
        // Si l'enregistrement n'est pas trouvé, elle renvoie un statut HTTP 404 (Not Found).
        // En cas d'erreur d'autorisation, elle peut renvoyer un statut HTTP 403 (Forbidden).
        [HttpGet("{departId:int}", Name = "GetDeparts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDeparts(int departId)
        {
            // Vérifie si l'ID fourni est valide
            var itemDepart = _depRepo.GetDeparts(departId);

            // Si aucun enregistrement avec l'ID spécifié n'est trouvé, renvoie un code 404 (Not Found)
            if (itemDepart == null)
            {
                return NotFound();
            }
            // Mappe l'enregistrement de départ récupéré en DTO (Data Transfer Object)
            // Cela permet de convertir l'objet de domaine en un objet plus léger et structuré pour le transfert de données.
            var itemDepartDto = _mapper.Map<DepartsDto>(itemDepart);

            // Renvoie un code 200 (OK) avec les informations de l'enregistrement de départ
            return Ok(itemDepartDto);
        }






        // Créer un nouveau départ

        // Cette méthode permet de créer un nouveau départ pour un navire qui quitte le port.
        // Elle prend comme paramètre un objet "CreateDepartsDto" contenant les informations du nouveau départ.
        // Si le modèle de données est invalide ou si l'objet est null, elle renvoie un code 400 (Bad Request).
        // Ensuite, elle récupère les informations sur l'arrivée associée au départ à partir de l'ID de l'arrivée fourni.
        // Si aucune arrivée n'est trouvée pour cet ID, elle renvoie un code 404 (Not Found).
        // Elle assigne ensuite les informations de l'arrivée au départ à créer.
        // Ensuite, elle mappe les données du DTO à un objet de type "Departs".
        // Si le départ est créé avec succès dans la base de données, elle renvoie un code 201 (Created) avec le départ créé et son ID.
        // En cas d'erreur lors de la création du départ dans la base de données, elle renvoie un code 500 (Internal Server Error) avec un message d'erreur.
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CreateDepartsDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateDeparts([FromBody] CreateDepartsDto createDepartDto)
        {
            // Vérifie si le modèle reçu est valide
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (createDepartDto == null)
            {
                return BadRequest(ModelState);
            }
            // Récupère les informations sur l'arrivée liée au départ
            var myarrivee = _arrRepo.GetArrivee(createDepartDto.ArriveeId);

            if (myarrivee == null)
            {
                // Si l'arrivée n'est pas trouvée, renvoie un code 404
                return NotFound();
            }

            // Assigne les informations de l'arrivée au départ à créer
            createDepartDto.ArriveeId = myarrivee.Id;
            createDepartDto.NomNavire = myarrivee.NomNavire;

            // Mappe les données du DTO à un objet de type "Departs"
            var depart = _mapper.Map<Departs>(createDepartDto);
           

            if (!_depRepo.CreateDeparts(depart))
            {
                // Si une erreur survient lors de la création, renvoie un code 500
                ModelState.AddModelError("", $"Une erreur s'est produite lors de la création d'un nouveau départ. {depart.NomNavire}");
                return StatusCode(500, ModelState);
            }

            // Renvoie un code 201 avec le départ créé
            return CreatedAtRoute("GetDeparts", new { departId = depart.Id }, depart);
        }





        // Mise à jour d'un départ
        // Cette méthode est utilisée pour mettre à jour les informations d'un départ dans la base de données.
        // Elle prend en paramètre l'ID du départ à mettre à jour et les nouvelles informations sous forme de DTO.
        // Si le modèle de données reçu n'est pas valide, elle renvoie un code 400 (Bad Request).
        // Si le départ avec l'ID spécifié n'est pas trouvé dans la base de données, elle renvoie un code 404 (Not Found).
        // Si une erreur survient lors de la mise à jour du départ, elle renvoie un code 500 (Internal Server Error).
        // Si la mise à jour est réussie, elle renvoie un code 204 (No Content).
        [HttpPatch("{departId:int}", Name = "UpdatePatcDepart")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatcDepart(int departId, [FromBody] DepartsDto departsDto)
        {
            // Vérifie si le modèle reçu est valide
            if (!ModelState.IsValid)
            {
                // Renvoie un code 400 (Bad Request) si le modèle est invalide
                return BadRequest(ModelState);
            }

            // Mappe les données du DTO à un objet de type "Departs"
            var depart = _mapper.Map<Departs>(departsDto);

            // Vérifie si le départ avec l'ID donné existe
            if (!_depRepo.ExistDepartsById(depart.Id) || !_depRepo.ExistDepartsById(departId))
            {
                return NotFound();
            }
            //if (!_depRepo.ExistDepartsById(departId))
            //{
            //    return NotFound();
            //}

            // Met à jour le départ dans la base de données
            if (!_depRepo.UpdateDeparts(depart))
            {
                // // Si une erreur survient lors de la mise à jour, renvoie un code 500 (Internal Server Error)
                ModelState.AddModelError("", $"Une erreur s'est produite lors de la mise à jour d'un départ. {depart.NomNavire} avec id {depart.Id}");
                return StatusCode(500, ModelState);
            }
            // Renvoie un code 204 (No Content) pour indiquer que la mise à jour a été effectuée avec succès
            return NoContent();
        }





        // Suppression d'un départ
        // Cette méthode est utilisée pour supprimer une sortie de navire de la base de données en fonction de son ID.
        // Elle prend en paramètre l'ID du départ à supprimer.
        // Si le départ avec l'ID spécifié n'est pas trouvé dans la base de données, elle renvoie un code 404 (Not Found).
        // Si une erreur survient lors de la suppression du départ, elle renvoie un code 500 (Internal Server Error).
        // Si la suppression est réussie, elle renvoie un code 204 (No Content).
        [HttpDelete("{departId:int}", Name = "DeleteDeparts")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteDeparts(int departId)
        {

            // Vérifie si le départ avec l'ID donné existe
            if (!_depRepo.ExistDepartsById(departId))
            {
                return NotFound();
            }

            var depart = _depRepo.GetDeparts(departId);

            // Supprime le départ de la base de données
            if (!_depRepo.DeleteDeparts(depart))
            {
                ModelState.AddModelError("", $"Une erreur s'est produite lors de la suppressionr d'un départ. {depart.NomNavire} avec id {depart.Id}");
                return StatusCode(500, ModelState);
            }

            // Renvoie un code 204 (No Content) pour indiquer que la suppression a réussi
            return NoContent();
        }


    }
}
