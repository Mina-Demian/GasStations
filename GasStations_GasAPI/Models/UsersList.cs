namespace GasStations_GasAPI.Models
{
    public class UsersList
    {
        public static List<Users> Users = new()
        {
            new Users()
            {
                Username = "Testing",
                Password = "Testing10",
                Role = "Admin"
            }
        };
    }
}
