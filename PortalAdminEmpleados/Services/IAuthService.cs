using PortalAdminEmpleados.Modelos;
using PortalAdminEmpleados.Modelos.Entidades;

namespace PortalAdminEmpleados.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Authenticate(LoginRequest request);
        Task<User> GetUserById(int id);
        Task<bool> ValidateToken(string token);
    }
}
