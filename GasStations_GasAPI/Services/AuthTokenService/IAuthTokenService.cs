using GasStations_GasAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GasStations_GasAPI.Services.AuthTokenService
{
    public interface IAuthTokenService
    {
        AuthResult GenerateToken(Users users);
        string CreateRefreshToken();
        void SetRefreshToken(RefreshToken newRefreshToken);
        Users Authenticate(UsersLogin usersLogin);
        ActionResult ValidateRefreshToken();
    }
}
