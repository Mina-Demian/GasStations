using System.ComponentModel.DataAnnotations;

namespace GasStations_GasAPI.Models
{
    public class TokenRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
