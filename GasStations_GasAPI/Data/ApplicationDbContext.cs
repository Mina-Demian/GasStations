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
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries_of_Origin { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<City>().HasData(
                new City
                {
                    Id = 1,
                    CityName = "Mississauga"
                },
                new City
                {
                    Id = 2,
                    CityName = "Oakville"
                }
                );

            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    CountryofOrigin = "France"
                },
                new Country
                {
                    Id = 2,
                    CountryofOrigin = "Canada"
                }
                );

        
            modelBuilder.Entity<Gas>()
                .HasOne(g => g.City)
                .WithMany(c => c.GasStations)
                .HasForeignKey(g => g.CityId);


            modelBuilder.Entity<Gas>()
                .HasOne(g => g.CountryofOrigin)
                .WithMany(c => c.GasStations)
                .HasForeignKey(g => g.CountryofOriginId);

            modelBuilder.Entity<Gas>().HasData(
                new Gas
                {
                    Id = 1,
                    Name = "Shell",
                    Address = "600 Dundas St",
                    Number_of_Pumps = 8,
                    Price = 147.3,
                    Purity = 87,
                    CreatedDate = DateTime.Now,
                    CityId = 2,
                    CountryofOriginId = 1
                },
                new Gas
                {
                    Id = 2,
                    Name = "Petro Canada",
                    Address = "1525 Burnhamthorpe Rd",
                    Number_of_Pumps = 12,
                    Price = 146.8,
                    Purity = 87,
                    CreatedDate = DateTime.Now,
                    CityId = 1,
                    CountryofOriginId = 2
                }
                );
        
        }
    }
}
