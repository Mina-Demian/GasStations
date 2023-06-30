using GasStations_GasAPI.Models;
using GasStations_GasAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GasStations_GasAPI.Services.GasStationService
{
    public interface IGasStationService
    {
        List<Gas> GetGasStations();
        Gas GetGasStation(int id);
        List<Gas> GetGasStationsByCityandCountryId(int CityId, int CountryofOriginId);
        Gas CreateGasStation([FromBody] Gas gas);
        bool DeleteGasStation(int id);
        IActionResult UpdateGasStation(int id, [FromBody] Gas gas);
        //Task<IActionResult> UpdatePartialGasStation(int id, JsonPatchDocument<GasDTO> patchDTO);
    }
}
