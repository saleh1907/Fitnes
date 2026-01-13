using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplication4.Models;

namespace WebApplication4.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

       public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Departament>Departaments { get; set; }
    }
}
