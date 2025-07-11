using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TallleresPaco.Models
{
    public class Contexto : DbContext                                                                                                               
    {
        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {
        }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Vehiculos> Vehiculos { get; set; } 
        public DbSet<Alquileres> Alquileres { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación: Usuarios → Alquileres
            modelBuilder.Entity<Alquileres>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Alquileres)
                .HasForeignKey(a => a.UsuarioId)
                .HasConstraintName("alquileres_fk1")
                .OnDelete(DeleteBehavior.Restrict);

            // Relación: Vehiculos → Alquileres
            modelBuilder.Entity<Alquileres>()
                .HasOne(a => a.Vehiculo)
                .WithMany(v => v.Alquileres)
                .HasForeignKey(a => a.VehiculoId)
                .HasConstraintName("alquileres_fk2")
                .OnDelete(DeleteBehavior.Restrict);
        }*/
    }
}
