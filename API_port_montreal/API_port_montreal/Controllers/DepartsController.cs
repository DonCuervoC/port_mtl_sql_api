using API_port_montreal.Models;
using API_port_montreal.Models.Dtos;
using API_port_montreal.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        // Obtenir la liste de toutes les departures
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetAllDeparts()
        {
            var departsList = _depRepo.GetAllDeparts();

            var listDepartsDto = new List<DepartsDto>(); // List avec dtos

            foreach (var list in departsList)
            {
                listDepartsDto.Add(_mapper.Map<DepartsDto>(list));
            }

            return Ok(listDepartsDto);
        }

        // // Obtenir un departure par son id
        [HttpGet("{departId:int}", Name = "GetDeparts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDeparts(int departId)
        {
            var itemDepart = _depRepo.GetDeparts(departId);

            if (itemDepart == null)
            {
                return NotFound();
            }

            var itemDepartDto = _mapper.Map<DepartsDto>(itemDepart);
            return Ok(itemDepartDto);
        }

        // // créer un nouveau départ
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CreateDepartsDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateDeparts([FromBody] CreateDepartsDto createDepartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (createDepartDto == null)
            {
                return BadRequest(ModelState);
            }

            var myarrivee = _arrRepo.GetArrivee(createDepartDto.ArriveeId);

            if (myarrivee == null)
            {
                return NotFound();
            }

            createDepartDto.ArriveeId = myarrivee.Id;
            createDepartDto.NomNavire = myarrivee.NomNavire;


            var depart = _mapper.Map<Departs>(createDepartDto);
           

            if (!_depRepo.CreateDeparts(depart))
            {
                ModelState.AddModelError("", $"Une erreur s'est produite lors de la création d'un nouveau départ. {depart.NomNavire}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetDeparts", new { departId = depart.Id }, depart);
        }


        //Mise à jour d'un départ
        [HttpPatch("{departId:int}", Name = "UpdatePatcDepart")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePatcDepart(int departId, [FromBody] DepartsDto departsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_depRepo.ExistDepartsById(departId))
            {
                return NotFound();
            }

            var depart = _mapper.Map<Departs>(departsDto);
            
            if (!_depRepo.UpdateDeparts(depart))
            {
                ModelState.AddModelError("", $"Une erreur s'est produite lors de la mise à jour d'un départ. {depart.NomNavire} avec id {depart.Id}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //// Suppression d'un départ
        [HttpDelete("{departId:int}", Name = "DeleteDeparts")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteDeparts(int departId)
        {
            if (!_depRepo.ExistDepartsById(departId))
            {
                return NotFound();
            }

            var depart = _depRepo.GetDeparts(departId);

            if (!_depRepo.DeleteDeparts(depart))
            {
                ModelState.AddModelError("", $"Une erreur s'est produite lors de la suppressionr d'un départ. {depart.NomNavire} avec id {depart.Id}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
