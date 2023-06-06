using GasStations_GasAPI.Data;
using GasStations_GasAPI.Handlers;
using GasStations_GasAPI.Models;
//using GasStations_GasAPI.Models.API_Key;
using GasStations_GasAPI.Models.Dto;
using GasStations_GasAPI.Services.GasStationService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GasStations_GasAPI.Controllers
{
    [Route("api/GasAPI")] //Hardcoded the Route as "api/GasAPI" instead of using "api/[controller]" so that if the controller name is changed in the Future, the Old Route should still work.
    [ApiController]
    public class GasAPIController : ControllerBase
    {
        //public static Users user = new Users();

        private readonly IGasStationService _gasStationService;
        //private readonly IConfiguration _config;
        //private readonly TokenValidationParameters _tokenValidationParameters;
        public GasAPIController(IGasStationService gasStationService)//, IConfiguration config)//, TokenValidationParameters tokenValidationParameters)
        {
            _gasStationService = gasStationService;
            //_config = config;
            //_tokenValidationParameters = tokenValidationParameters;
        }

        //[Authorize(Policy = "Level1")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        //[Authorize(AuthenticationSchemes = "Basic Authentication")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetGasStations()
        {
            var GasStations = _gasStationService.GetGasStations();

            return Ok(GasStations);
        }

        [Authorize(Policy = "Level1")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [Authorize(Policy = "Level2")]
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

        [Authorize(Policy = "Level3")]
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

        [Authorize(Policy = "Level2")]
        [Authorize(AuthenticationSchemes = "Jwt")]
        //[Authorize(Policy = "JWT Bearer Authentication")]
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
