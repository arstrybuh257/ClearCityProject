using ClearCity.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ClearCity.DAL
{
    public class ClearCityContext : DbContext
    {

        public ClearCityContext() : base("ClearCityContext")
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Microdistrict> Microdistricts { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Plan> Plans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}