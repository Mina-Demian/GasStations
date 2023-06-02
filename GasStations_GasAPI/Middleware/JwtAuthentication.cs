using System.Security.Claims;
using System.Text;

namespace GasStations_GasAPI.Middleware
{
    public class JwtAuthentication
    {
        private readonly RequestDelegate _next;

        public JwtAuthentication(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];

            if (authHeader != null)
            {
                int startPoint = authHeader.IndexOf(".") + 1;
                int endPoint = authHeader.LastIndexOf(".");

                var tokenString = authHeader.Substring(startPoint, endPoint - startPoint).Split(".");
                var token = tokenString[0].ToString() + "==";

                var credentialString = Encoding.UTF8.GetString(Convert.FromBase64String(token));

                var credentials = credentialString.Split(new char[]
                {
                    ':',','
                });

                var userRole = credentials[3].Replace("\"", "");
                var userName = credentials[1].Replace("\"", "");

                var claims = new[]
                {
                    new Claim("name", userName),
                    new Claim(ClaimTypes.Role, userRole),
                };
                var identity = new ClaimsIdentity(claims, "basic");
                context.User = new ClaimsPrincipal(identity);
            }

            await _next(context);
        }
    }
}
