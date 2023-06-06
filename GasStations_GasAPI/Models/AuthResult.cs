﻿using System.IdentityModel.Tokens.Jwt;

namespace GasStations_GasAPI.Models
{
    public class AuthResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Error { get; set; }
    }
}
