namespace GasStations_GasAPI.Services.UserService
{
    public interface IUserService
    {
        Task<bool> Authenticate(string username, string password);
    }
}
