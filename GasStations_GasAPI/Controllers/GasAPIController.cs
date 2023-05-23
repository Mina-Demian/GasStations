using GasStations_GasAPI.Data;
using GasStations_GasAPI.Models;
using GasStations_GasAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GasStations_GasAPI.Controllers
{
    [Route("api/GasAPI")] //Hardcoded the Route as "api/GasAPI" instead of using "api/[controller]" so that if the controller name is changed in the Future, the Old Route should still work.
    [ApiController]
    public class GasAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _db;
        public GasAPIController(ApplicationDbContext db)
        {
            _db = db;
        }


        //private readonly ILogger<GasAPIController> _logger;

        //public GasAPIController(ILogger<GasAPIController> logger)
        //{
        //    _logger = logger;
        //}


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult <IEnumerable<GasDTO>> GetGasStations()
        {
            //_logger.LogInformation("Getting all the Gas Stations");
            return Ok(_db.GasStations.ToList());
        }

        [HttpGet("{id:int}", Name = "GetGasStation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult <GasDTO> GetGasStation(int id)
        {
            if (id == 0)
            {
                //_logger.LogError("Get Gas Station Error with ID of " + id);
                return BadRequest();
            }
            var GasStation = _db.GasStations.FirstOrDefault(u => u.Id == id);
            if (GasStation == null)
            {
                //_logger.LogError("Getting a Gas Station Error with ID of null");
                return NotFound();
            }
            //_logger.LogInformation("Getting Gas Station with ID of " + id);
            return Ok(GasStation);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult <GasDTO> CreateGasStation([FromBody] GasDTO gasDTO)
        {
            if (_db.GasStations.FirstOrDefault(u => u.Name.ToLower() == gasDTO.Name.ToLower()) != null)
            {
                //_logger.LogError("Creating a Gas Station Error with a Name that already exisits");
                ModelState.AddModelError("CustomError", "Gas Station Already Exists in the Area!");
                return BadRequest(ModelState);
            }
            if (gasDTO == null)
            {
                //_logger.LogError("Creating a Gas Station Error that is null");
                return BadRequest(gasDTO);
            }
            if (gasDTO.Id > 0)
            {
                //_logger.LogError("Creating a Gas Station Error with ID inputted by user");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Gas model = new()
            {
                Id = gasDTO.Id,
                Name = gasDTO.Name,
                Address = gasDTO.Address,
                Number_of_Pumps = gasDTO.Number_of_Pumps,
                Price = gasDTO.Price,
                Purity = gasDTO.Purity
            };

            _db.GasStations.Add(model);
            _db.SaveChanges();

            //_logger.LogInformation("Creating a Gas Station");
            return CreatedAtRoute("GetGasStation", new { id = gasDTO.Id }, gasDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteGasStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteGasStation(int id)
        {
            if (id == 0)
            {
                //_logger.LogError("Deleting a Gas Station Error with Id of 0");
                return BadRequest();
            }
            var gasStation = _db.GasStations.FirstOrDefault(u => u.Id == id);
            if (gasStation == null)
            {
                //_logger.LogError("Deleting a Gas Station Error with Id that does not exist");
                return NotFound();
            }
            _db.GasStations.Remove(gasStation);
            _db.SaveChanges();
            //_logger.LogInformation("Deleting a Gas Station");
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateGasStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateGasStation(int id, [FromBody] GasDTO gasDTO)
        {
            if (gasDTO == null || id != gasDTO.Id)
            {
                //_logger.LogError("Updating a Gas Station Error with a Gas Station that is null or with an Id that does not match the Id provided");
                return BadRequest();
            }

            Gas model = new()
            {
                Id = gasDTO.Id,
                Name = gasDTO.Name,
                Address = gasDTO.Address,
                Number_of_Pumps = gasDTO.Number_of_Pumps,
                Price = gasDTO.Price,
                Purity = gasDTO.Purity
            };

            _db.GasStations.Update(model);
            _db.SaveChanges();

            //_logger.LogInformation("Updating a Gas Station");
            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialGasStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdatePartialGasStation(int id, JsonPatchDocument<GasDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                //_logger.LogError("Partially Updating a Gas Station Error with a Gas Station that is null or an Id of 0");
                return BadRequest();
            }

            var gasStation = _db.GasStations.AsNoTracking().FirstOrDefault(u => u.Id == id);

            GasDTO gasDTO = new()
            {
                Id = gasStation.Id,
                Name = gasStation.Name,
                Address = gasStation.Address,
                Number_of_Pumps = gasStation.Number_of_Pumps,
                Price = gasStation.Price,
                Purity = gasStation.Purity
            };


            if (gasStation == null)
            {
                //_logger.LogError("Partially updating a Gas Station Error that is null");
                return BadRequest();
            }
            patchDTO.ApplyTo(gasDTO, ModelState);

            Gas model = new Gas()
            {
                Id = gasDTO.Id,
                Name = gasDTO.Name,
                Address = gasDTO.Address,
                Number_of_Pumps = gasDTO.Number_of_Pumps,
                Price = gasDTO.Price,
                Purity = gasDTO.Purity
            };

            _db.GasStations.Update(model);
            _db.SaveChanges();

            if (!ModelState.IsValid)
            {
                //_logger.LogError("Partially updating a Gas Station Error with a ModelState that is invalid");
                return BadRequest(ModelState);
            }

            //_logger.LogInformation("Partially updating a Gas Station");
            return NoContent();
        }
    }
}
