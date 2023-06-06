namespace GasStations_GasAPI.Models
{
    public class UsersList
    {
        public static List<Users> Users = new()
        {
            new Users()
            {
                Username = "Test1",
                Password = "Pass1",
                Role = "Employee"
            },
             new Users()
            {
                Username = "Test2",
                Password = "Pass2",
                Role = "Admin"
            },
              new Users()
            {
                Username = "Test3",
                Password = "Pass3",
                Role = "Manager"
            }
        };
    }
}
