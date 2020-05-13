using Microsoft.EntityFrameworkCore;

namespace backend_cshar.Entities
{
    public class DataBaseDbContext : DbContext
    {
        public DbSet<User> Users {get; set;}
        public DbSet<Product> Products {get; set;}
        public DataBaseDbContext()
        {

        }   
        public DataBaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // define a one-to-many relationship
            modelBuilder.Entity<User>()
                            .HasMany(c => c.Products)
                            .WithOne(p => p.User);

            modelBuilder.Entity<Product>()
                    .HasOne(p => p.User)
                    .WithMany(c => c.Products);                
        }   
    }
}