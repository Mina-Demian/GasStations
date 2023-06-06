using Microsoft.AspNetCore.Mvc;

namespace GasStations_GasAPI.Models.API_Key
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute() : base(typeof(ApiKeyAuthorizationFilter))
        {
            
        }
    }
}
