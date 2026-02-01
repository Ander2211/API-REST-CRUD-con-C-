namespace PortalAdminEmpleados.Modelos
{
    public class AddEmpleadoDto
    {
        public required string Nombre { get; set; }
        public required string Email { get; set; }
        public string? Telefono { get; set; }
        public double Salario { get; set; }
    }
}
