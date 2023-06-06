namespace GasStations_GasAPI.Models.API_Key.Service
{
    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
