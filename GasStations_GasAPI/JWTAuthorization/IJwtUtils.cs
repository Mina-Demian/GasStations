using GasStations_GasAPI.Models;

namespace GasStations_GasAPI.JWTAuthorization
{
    public interface IJwtUtils
    {
        public string GenerateToken(User user);
        public int? ValidateToken(string token);
    }
}
