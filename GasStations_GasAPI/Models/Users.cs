namespace GasStations_GasAPI.Models
{
    public class Users
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; }
        public string Role { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpired { get; set; }
    }
}
