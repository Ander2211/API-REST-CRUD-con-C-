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
    }
}
