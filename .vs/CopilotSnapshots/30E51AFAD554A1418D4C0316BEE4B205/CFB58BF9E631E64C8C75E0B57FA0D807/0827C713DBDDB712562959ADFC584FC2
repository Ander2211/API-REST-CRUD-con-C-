using Microsoft.EntityFrameworkCore;
using PortalAdminEmpleados.Modelos.Entidades;



namespace PortalAdminEmpleados.Data
{
    public class Conexion : DbContext
    {
        public Conexion(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Empleados> Empleados { get; set; }
        public DbSet<Categorias> Categorias => Set<Categorias>();
        public DbSet<Productos> Productos => Set<Productos>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Productos>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId);
        }
    }
}
