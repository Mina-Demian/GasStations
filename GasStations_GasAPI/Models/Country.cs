using System.ComponentModel.DataAnnotations;

namespace GasStations_GasAPI.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryofOrigin { get; set; }
        public ICollection<Gas> GasStations { get; set; }
    }
}
