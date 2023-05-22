using GasStations_GasAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GasStations_GasAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Gas> GasStations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gas>().HasData(
                new Gas
                {
                    Id = 1,
                    Name = "Shell",
                    Address = "600 Dundas St",
                    Number_of_Pumps = 8,
                    Price = 147.3,
                    Purity = 87,
                    CreatedDate = DateTime.Now
                },
                new Gas
                {
                    Id = 2,
                    Name = "Petro Canada",
                    Address = "1525 Burnhamthorpe Rd",
                    Number_of_Pumps = 12,
                    Price = 146.8,
                    Purity = 87,
                    CreatedDate = DateTime.Now
                }
                );
        }
    }
}
