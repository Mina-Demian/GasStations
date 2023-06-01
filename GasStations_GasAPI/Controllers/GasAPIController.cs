using GasStations_GasAPI.Data;
using GasStations_GasAPI.Models;
using GasStations_GasAPI.Models.Dto;
using GasStations_GasAPI.Services.GasStationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GasStations_GasAPI.Controllers
{
    [Route("api/GasAPI")] //Hardcoded the Route as "api/GasAPI" instead of using "api/[controller]" so that if the controller name is changed in the Future, the Old Route should still work.
    [ApiController]
    public class GasAPIController : ControllerBase
    {
        private readonly IGasStationService _gasStationService;
        public GasAPIController(IGasStationService gasStationService)
        {
            _gasStationService = gasStationService;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetGasStations()
        {
            var GasStations = _gasStationService.GetGasStations();

            return Ok(GasStations);
        }

        [HttpGet("{id:int}", Name = "GetGasStation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetGasStation(int id)
        {

            var GasStation = _gasStationService.GetGasStation(id);
            if (GasStation == null)
            {
                return NotFound();
            }
            if (id == 0)
            {
                return BadRequest();
            }
            else
            {
                return Ok(GasStation);
            }

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult CreateGasStation([FromBody] Gas gas)
        {
            var result = _gasStationService.CreateGasStation(gas);

            if (gas == null)
            {
                return BadRequest(gas);
            }

            if (gas.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return CreatedAtRoute("GetGasStation", new { id = gas.Id }, gas);
        }

        [HttpDelete("{id:int}", Name = "DeleteGasStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteGasStation(int id)
        {
            var gasStation = _gasStationService.DeleteGasStation(id);

            if (id == 0)
            {
                return BadRequest();
            }

            if (id == null)
            {
                return NotFound();
            }

            return NoContent();


        }

        [HttpPut("{id:int}", Name = "UpdateGasStation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateGasStation(int id, [FromBody] Gas gas)
        {
            var result = _gasStationService.UpdateGasStation(id, gas);

            if (gas == null || id != gas.Id)
            {
                return BadRequest();
            }

            return NoContent();
        }

        //[HttpPatch("{id:int}", Name = "UpdatePartialGasStation")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]

        //public async Task<IActionResult> UpdatePartialGasStation(int id, JsonPatchDocument<GasDTO> patchDTO)
        //{
        //    if (patchDTO == null || id == 0)
        //    {
        //        return BadRequest();
        //    }

        //    var gasStation = await _gasStationService.UpdateGasStation(id, patchDTO);


        //    if (gasStation == null)
        //    {
        //        return BadRequest();
        //    }

        //    return NoContent();
        //}
    }
}
