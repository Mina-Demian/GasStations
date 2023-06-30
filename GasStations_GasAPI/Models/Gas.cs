using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GasStations_GasAPI.Models
{
    public class Gas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Number_of_Pumps { get; set; }
        public double Price { get; set; }
        public int Purity { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set;}

        public int? CityId { get; set; }
        public City City { get; set; }

        public int? CountryofOriginId { get; set; }
        public Country CountryofOrigin { get; set; }
    }
}
