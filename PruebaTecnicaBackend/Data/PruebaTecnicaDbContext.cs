using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackend.Models;

namespace PruebaTecnicaBackend.Data
{
    public class PruebaTecnicaDbContext : DbContext
    {
        public PruebaTecnicaDbContext(DbContextOptions<PruebaTecnicaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>().HasKey(p => p.Identificador);
            modelBuilder.Entity<Usuario>().HasKey(u => u.Identificador);

            base.OnModelCreating(modelBuilder);
        }
    }
}
