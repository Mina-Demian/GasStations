using System.Net.Http.Headers;
using System.Text;

namespace GasStations_GasAPI.Middleware
{
    public class BasicAuthHandler
    {
        private readonly RequestDelegate next;
        private readonly string relm;

        public BasicAuthHandler(RequestDelegate next, string relm)
        {
            this.next = next;
            this.relm = relm;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
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
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await next(context);

        }
    }
}
