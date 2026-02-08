using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PortalAdminEmpleados.Modelos.Entidades
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Usuario { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public string Rol { get; set; } = "Admin";


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; } = true;


    }
}
