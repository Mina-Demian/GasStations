namespace GasStations_GasAPI.Models.API_Key.Service
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        public bool IsValid(string apiKey)
        {
            if (apiKey != null)
            {
                if (apiKey == "Testing")
                    return true;
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }   
        }
    }
}
