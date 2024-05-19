using API_port_montreal.Models;
using API_port_montreal.Models.Dtos;
using API_port_montreal.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // Obtenir la liste de toutes les arrivées
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetArrivees()
        {
            var arriveesList = _arrRepo.GetArrivees();

            var listArriveesDto = new List<ArriveesDto>(); // List avec dtos

            foreach (var list in arriveesList)
            {
                listArriveesDto.Add(_mapper.Map<ArriveesDto>(list));
            }

            return Ok(listArriveesDto);
        }

        // // Obtenir une arrivée par ID
        [HttpGet("{arriveeId:int}", Name = "GetArrivee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetArrivee(int arriveeId)
        {
            var itemArrivee = _arrRepo.GetArrivee(arriveeId);

            if (itemArrivee == null)
            {
                return NotFound();
            }

            var itemArriveeDto = _mapper.Map<ArriveesDto>(itemArrivee);
            return Ok(itemArriveeDto);
        }

        // // Créer une nouvelle arrivée
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ArriveesDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateArrivee([FromBody] CreateArriveeDto createArriveeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (createArriveeDto == null)
            {
                return BadRequest(ModelState);
            }

            var arrivee = _mapper.Map<Arrivees>(createArriveeDto);
            // ici nous créons l'arrivée dans le if en utilisant la méthode
            if (!_arrRepo.CreateArrivee(arrivee))
            {
                ModelState.AddModelError("", $"Un problème s'est produit lors de l'enregistrement de {arrivee.NomNavire}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetArrivee", new { arriveeId = arrivee.Id }, arrivee);
        }

        // Mise à jour d'une arrivée
        [HttpPatch("{arriveeId:int}", Name = "UpdatePatchArrivees")]
        [ProducesResponseType(201, Type = typeof(ArriveesDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatchArrivees(int arriveeId, [FromBody] ArriveesDto arriveeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (arriveeDto == null || arriveeId != arriveeDto.Id)
            {
                return BadRequest(ModelState);
            }

            var arrivee = _mapper.Map<Arrivees>(arriveeDto);

            if (!_arrRepo.UpdateArrivee(arrivee))
            {
                ModelState.AddModelError("", $"Un problème s'est produit lors de mise à jour de {arrivee.NomNavire} avec id {arrivee.Id}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Supprimer une arrivée
        [HttpDelete("{arriveId:int}", Name = "DeleteArrivee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteArrivee(int arriveId)
        {
            if (!_arrRepo.ExistArriveeById(arriveId))
            {
                return NotFound();
            }

            var arrivee = _arrRepo.GetArrivee(arriveId);

            // ici nous créons l'arrivée dans le if en utilisant la méthode
            if (!_arrRepo.DeleteArrivee(arrivee))
            {
                ModelState.AddModelError("", $"Un problème s'est produit lors de l'élimination de {arrivee.NomNavire} avec id {arrivee.Id}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
