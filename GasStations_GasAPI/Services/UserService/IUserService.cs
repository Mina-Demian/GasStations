using GasStations_GasAPI.Models;

namespace GasStations_GasAPI.Services.UserService
{
    public interface IUserService
    {
        Task<bool> Authenticate(string username, string password);
        User GetById(int id);
    }
}
