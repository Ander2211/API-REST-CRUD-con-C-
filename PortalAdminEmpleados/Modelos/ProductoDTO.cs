namespace PortalAdminEmpleados.Modelos
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }
    }
}
