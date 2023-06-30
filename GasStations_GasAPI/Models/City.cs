using System.ComponentModel.DataAnnotations;

namespace GasStations_GasAPI.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string CityName { get; set; }
        public ICollection<Gas> GasStations { get; set; }
    }
}
