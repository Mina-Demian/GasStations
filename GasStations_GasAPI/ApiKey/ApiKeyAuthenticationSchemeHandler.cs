using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

//namespace GasStations_GasAPI.ApiKey
//{
//    public class ApiKeyAuthenticationSchemeHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
//    {
//        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            var apiKey = Context.Request.Headers["X-API-KEY"];
//            if (apiKey != Options.ApiKey)
//            {
//                return Task.FromResult(AuthenticateResult.Fail("Invalid X-API-KEY"));
//            }
//            var claims = new[] { new Claim(ClaimTypes.Name, "VALID USER") };
//            var identity = new ClaimsIdentity(claims, Scheme.Name);
//            var principal = new ClaimsPrincipal(identity);
//            var ticket = new AuthenticationTicket(principal, Scheme.Name);
//            return Task.FromResult(AuthenticateResult.Success(ticket));
//        }
//    }
//}
