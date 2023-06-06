namespace GasStations_GasAPI.Models
{
    public class RefreshToken
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
