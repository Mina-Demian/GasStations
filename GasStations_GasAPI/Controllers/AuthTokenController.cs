using Azure.Core;
using Azure;
using GasStations_GasAPI.Models;
using GasStations_GasAPI.Services.GasStationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GasStations_GasAPI.Controllers
{
    [Route("api/AuthToken")] //Hardcoded the Route as "api/GasAPI" instead of using "api/[controller]" so that if the controller name is changed in the Future, the Old Route should still work.
    [ApiController]
    public class AuthTokenController : ControllerBase
    {

        public static Users user = new Users();

        private readonly IConfiguration _config;
        //private readonly TokenValidationParameters _tokenValidationParameters;
        public AuthTokenController(IConfiguration config)//, TokenValidationParameters tokenValidationParameters)
        {
            _config = config;
            //_tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login([FromBody] UsersLogin usersLogin)
        {
            var user = Authenticate(usersLogin);
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return NotFound("User Not Found");
        }

        private AuthResult GenerateToken(Users users)
        {
            var user = users.Username;
            var userRoles = users.Role;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,users.Username),
                //new Claim(ClaimTypes.Role,users.Role)
            };

            authClaims.Add(new Claim(ClaimTypes.Role, userRoles));


            //var Subject = new ClaimsIdentity(claims);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims: authClaims,
                //(IEnumerable<Claim>)Subject,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                Token = CreateRefreshToken(),
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                Username = users.Username,
                Role = users.Role
            };

            //var refreshToken = GetRefreshToken();

            SetRefreshToken(refreshToken);

            //return new JwtSecurityTokenHandler().WriteToken(token);
            return new AuthResult()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                Success = true
            };

        }

        //private RefreshToken GetRefreshToken()
        //{
        //    var refreshToken = new RefreshToken()
        //    {
        //        Token = CreateRefreshToken(),
        //        AddedDate = DateTime.UtcNow,
        //        ExpiryDate = DateTime.UtcNow.AddDays(7)
        //    };
        //    return refreshToken; 
        //}

        private static string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.ExpiryDate
            };

            Response.Cookies.Append("RefreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.AddedDate;
            user.TokenExpired = newRefreshToken.ExpiryDate;
            user.Username = newRefreshToken.Username;
            user.Role = newRefreshToken.Role;
        }

        private Users Authenticate(UsersLogin usersLogin)
        {
            var currentUser = UsersList.Users.FirstOrDefault(u => u.Username.ToLower() == usersLogin.Username.ToLower() && u.Password == usersLogin.Password);
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        [HttpPost]
        [Route("RefreshToken")]
        public ActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (user.TokenExpired < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            var newTokens = GenerateToken(user);
            return Ok(newTokens);

        }

        [HttpPost]
        [Route("VaildateToken")]
        public ActionResult ValidateToken()
        {
           return ValidateRefreshToken();
        }

        private ActionResult ValidateRefreshToken()
        {
            var refreshTokens = Request.Cookies["RefreshToken"];
            if (!user.RefreshToken.Equals(refreshTokens))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if (user.TokenExpired < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }
            return Ok("Refresh Token is Valid");
        }

    }
}
