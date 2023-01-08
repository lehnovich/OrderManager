using Microsoft.EntityFrameworkCore;

namespace OrderManager.Models.Data
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<StatusHistory> StatusHistories { get; set; }

        public ApplicationContext() 
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OrderManagerDB;Trusted_Connection=True;");
        }
    }
}
