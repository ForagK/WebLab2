using Microsoft.EntityFrameworkCore;
using WebLab2.Models;

namespace WebLab2.DataBase
{
    public class AppDbContext : DbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Admin> Admins { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
