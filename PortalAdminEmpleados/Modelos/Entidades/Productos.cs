namespace PortalAdminEmpleados.Modelos.Entidades
{
    public class Productos
    {
        public int Id { get; set; } 
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }


        public Categorias Categoria { get; set; } = null!;
    }
}
