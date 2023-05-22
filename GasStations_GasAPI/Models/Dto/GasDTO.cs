using System.ComponentModel.DataAnnotations;

namespace GasStations_GasAPI.Models.Dto
{
    public class GasDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public int Number_of_Pumps { get; set; }
        [Required]
        public double Price { get; set; }
        public int Purity { get; set; }
    }
}
