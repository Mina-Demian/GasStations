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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult <IEnumerable<GasDTO>> GetGasStations()
        {
            return Ok(GasStore.GasList);
        }

        [HttpGet("{id:int}", Name = "GetGasStation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult <GasDTO> GetGasStation(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var GasStation = GasStore.GasList.FirstOrDefault(u => u.Id == id);
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
            if (GasStore.GasList.FirstOrDefault(u => u.Name.ToLower() == gasDTO.Name.ToLower()) != null)
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
            gasDTO.Id = GasStore.GasList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            GasStore.GasList.Add(gasDTO);

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
            var gasStation = GasStore.GasList.FirstOrDefault(u => u.Id == id);
            if (gasStation == null)
            {
                return NotFound();
            }
            GasStore.GasList.Remove(gasStation);
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
            var gasStation = GasStore.GasList.FirstOrDefault(u => u.Id == id);
            gasStation.Name = gasDTO.Name;
            gasStation.Address = gasDTO.Address;
            gasStation.Number_of_Pumps = gasDTO.Number_of_Pumps;
            
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
            var gasStation = GasStore.GasList.FirstOrDefault(u => u.Id == id);
            if (gasStation == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(gasStation, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
