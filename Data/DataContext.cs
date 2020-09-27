using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> User { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}