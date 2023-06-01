using GasStations_GasAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace GasStations_GasAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
        {
            new User
            {
                Id = 1, Username = "Mina", Password = "Mina123"
            },
            new User
            {
                Id = 2, Username = "Test", Password = "Password"
            }
        };

        public async Task<bool> Authenticate(string username, string password)
        {
            if (await Task.FromResult(_users.SingleOrDefault(u => u.Username == username && u.Password == password)) != null)
            {
                return true;
            }
            return false;
        }
    }
}
