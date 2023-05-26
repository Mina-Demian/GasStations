using GasStations_GasAPI.Models;
using GasStations_GasAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GasStations_GasAPI.Services.GasStationService
{
    public interface IGasStationService
    {
        Task<ActionResult<IEnumerable<Gas>>> GetGasStations();
        Task<ActionResult<Gas>> GetGasStation(int id);
        Task<ActionResult<Gas>> CreateGasStation([FromBody] GasDTO gasDTO);
        Task<IActionResult> DeleteGasStation(int id);
        Task<IActionResult> UpdateGasStation(int id, [FromBody] GasDTO gasDTO);
        //Task<IActionResult> UpdatePartialGasStation(int id, JsonPatchDocument<GasDTO> patchDTO);
    }
}
