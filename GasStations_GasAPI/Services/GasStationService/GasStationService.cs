using GasStations_GasAPI.Controllers;
using GasStations_GasAPI.Data;
using GasStations_GasAPI.Models;
using GasStations_GasAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GasStations_GasAPI.Services.GasStationService
{
    public class GasStationService : IGasStationService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<GasAPIController> _logger;
        public GasStationService(ApplicationDbContext db, ILogger<GasAPIController> logger)
        {
            _db = db;
            _logger = logger;
        }
        public List<Gas>GetGasStations()
        {
            _logger.LogInformation("Getting all the Gas Stations");
            var GasStations = _db.GasStations.ToList();
            return GasStations;
        }

        public Gas GetGasStation(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Get Gas Station Error with ID of " + id);
            }
            var GasStation = _db.GasStations.FirstOrDefault(u => u.Id == id);
            if (GasStation == null)
            {
                _logger.LogError("Getting a Gas Station Error with ID of null");
                return null;
            }
            _logger.LogInformation("Getting Gas Station with ID of " + id);
            return GasStation;
        }

        public Gas CreateGasStation([FromBody] Gas gas)
        {
            if (_db.GasStations.FirstOrDefault(u => u.Name.ToLower() == gas.Name.ToLower()) != null)
            {
                _logger.LogError("Creating a Gas Station Error with a Name that already exisits");
            }
            if (gas == null)
            {
                _logger.LogError("Creating a Gas Station Error that is null");
            }
            if (gas.Id > 0)
            {
                _logger.LogError("Creating a Gas Station Error with ID inputted by user");
            }

            Gas model = new()
            {
                Id = gas.Id,
                Name = gas.Name,
                Address = gas.Address,
                Number_of_Pumps = gas.Number_of_Pumps,
                Price = gas.Price,
                Purity = gas.Purity
            };

            _db.GasStations.Add(model);
            _db.SaveChanges();

            _logger.LogInformation("Creating a Gas Station");
            return model;
        }

        public bool DeleteGasStation(int id)
        {
            bool Deleted = false;
            if (id == 0)
            {
                _logger.LogError("Deleting a Gas Station Error with Id of 0");
            }

            var gasStation = _db.GasStations.FirstOrDefault(u => u.Id == id);

            if (gasStation == null)
            {
                _logger.LogError("Deleting a Gas Station Error with Id that does not exist");
            }
            
            if (gasStation != null)
            {
                _db.GasStations.Remove(gasStation);
                _db.SaveChanges();

                Deleted = true;

                _logger.LogInformation("Deleting a Gas Station");
            }

            return Deleted;
        }

        public IActionResult UpdateGasStation(int id, [FromBody] Gas gas)
        {
            if (gas == null || id != gas.Id)
            {
                _logger.LogError("Updating a Gas Station Error with a Gas Station that is null or with an Id that does not match the Id provided");
            }

            Gas model = new()
            {
                Id = gas.Id,
                Name = gas.Name,
                Address = gas.Address,
                Number_of_Pumps = gas.Number_of_Pumps,
                Price = gas.Price,
                Purity = gas.Purity
            };

            _db.GasStations.Update(model);
            _db.SaveChanges();

            _logger.LogInformation("Updating a Gas Station");
            return null;
        }

        //public async Task<IActionResult> UpdatePartialGasStation(int id, JsonPatchDocument<GasDTO> patchDTO)
        //{
        //    if (patchDTO == null || id == 0)
        //    {
        //        _logger.LogError("Partially Updating a Gas Station Error with a Gas Station that is null or an Id of 0");
        //    }

        //    var gasStation = await _db.GasStations.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

        //    GasDTO gasDTO = new()
        //    {
        //        Id = gasStation.Id,
        //        Name = gasStation.Name,
        //        Address = gasStation.Address,
        //        Number_of_Pumps = gasStation.Number_of_Pumps,
        //        Price = gasStation.Price,
        //        Purity = gasStation.Purity
        //    };


        //    if (gasStation == null)
        //    {
        //        _logger.LogError("Partially updating a Gas Station Error that is null");
        //    }
        //    patchDTO.ApplyTo(gasDTO);

        //    Gas model = new Gas()
        //    {
        //        Id = gasDTO.Id,
        //        Name = gasDTO.Name,
        //        Address = gasDTO.Address,
        //        Number_of_Pumps = gasDTO.Number_of_Pumps,
        //        Price = gasDTO.Price,
        //        Purity = gasDTO.Purity
        //    };

        //    _db.GasStations.Update(model);
        //    await _db.SaveChangesAsync();

        //    _logger.LogInformation("Partially updating a Gas Station");
        //    return null;
        //}
    }
}
