using GasStations_GasAPI.Models.Dto;

namespace GasStations_GasAPI.Data
{
    public static class GasStore
    {
        public static List<GasDTO> GasList = new List<GasDTO>{
                new GasDTO {Id = 1, Name = "Shell", Address = "600 Dundas St", Number_of_Pumps = 8 },
                new GasDTO {Id = 2, Name = "Petro Canada", Address = "1525 Burnhamthorpe Rd", Number_of_Pumps = 12}
            };
    }
}
