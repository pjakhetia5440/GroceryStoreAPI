using GroceryStoreAPI.Entities;
using Microsoft.EntityFrameworkCore;


namespace GroceryStoreAPI.Data.DataProviders
{
    public interface IGroceryStoreAPIDBContext : IDbContext
    {
        DbSet<Customer> Customer { get; set; }
    }
    public class GroceryStoreAPIDBContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }
        public GroceryStoreAPIDBContext(DbContextOptions<GroceryStoreAPIDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    }
}
