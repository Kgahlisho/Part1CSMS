using Microsoft.EntityFrameworkCore;
using Part1ex.Models;

namespace Part1ex.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Calculations> Claims { get; set; }
        public DbSet<User> Users { get;set; }
        public DbSet<Rol> Roles { get;set; }

    }
}
