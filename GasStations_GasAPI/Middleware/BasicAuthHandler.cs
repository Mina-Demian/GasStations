using System.Net.Http.Headers;
using System.Text;

namespace GasStations_GasAPI.Middleware
{
    public class BasicAuthHandler
    {
        private readonly RequestDelegate _next;
        private readonly string _relm;

        public BasicAuthHandler(RequestDelegate next, string relm)
        {
            _next = next;
            _relm = relm;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Does not contain authorization header");
                return;
            }

            var header = context.Request.Headers["Authorization"].ToString();

            var encodedCredentials = header.Substring(6); //After "Basic " (6 characters including the space)

            var bytes = Convert.FromBase64String(encodedCredentials);
            string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
            string emailAddress = credentials[0];
            string password = credentials[1];

            if(emailAddress != "Tester" || password != "Pass")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized: Invalid Email Address or Password");
                return;
            }

            await _next(context);

        }
    }
}
