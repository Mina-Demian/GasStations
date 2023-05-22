using GasStations_GasAPI.Data;
using GasStations_GasAPI.Models;
using GasStations_GasAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

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


        private readonly ILogger<GasAPIController> _logger;

        public GasAPIController(ILogger<GasAPIController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult <IEnumerable<GasDTO>> GetGasStations()
        {
            _logger.LogInformation("Getting all the Gas Stations");
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
                _logger.LogError("Get Gas Station Error with ID of " + id);
                return BadRequest();
            }
            var GasStation = _db.GasStations.FirstOrDefault(u => u.Id == id);
            if (GasStation == null)
            {
                return NotFound();
            }
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
                ModelState.AddModelError("CustomError", "Gas Station Already Exists in the Area!");
                return BadRequest(ModelState);
            }
            if (gasDTO == null)
            {
                return BadRequest(gasDTO);
            }
            if (gasDTO.Id > 0)
            {
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
                return BadRequest();
            }
            var gasStation = _db.GasStations.FirstOrDefault(u => u.Id == id);
            if (gasStation == null)
            {
                return NotFound();
            }
            _db.GasStations.Remove(gasStation);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateGasStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateGasStation(int id, [FromBody] GasDTO gasDTO)
        {
            if (gasDTO == null || id != gasDTO.Id)
            {
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

            return NoContent();
        }

        [HttpPatch("{id:int}", Name = "UpdatePartialGasStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdatePartialGasStation(int id, JsonPatchDocument<GasDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var gasStation = _db.GasStations.FirstOrDefault(u => u.Id == id);

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
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
