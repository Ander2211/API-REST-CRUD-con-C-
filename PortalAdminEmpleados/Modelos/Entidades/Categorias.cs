namespace PortalAdminEmpleados.Modelos.Entidades
{
    public class Categorias
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }

        public ICollection<Productos> Productos { get; set; } = new List<Productos>();
    }
}
