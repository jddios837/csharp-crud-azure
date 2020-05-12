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
    }
}