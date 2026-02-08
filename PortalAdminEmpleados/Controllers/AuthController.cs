using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortalAdminEmpleados.Services;
using PortalAdminEmpleados.Modelos;
using System.Security.Claims;
using PortalAdminEmpleados.Modelos.Entidades;


namespace PortalAdminEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.Authenticate(request);

                // Registrar el login exitoso
                _logger.LogInformation($"Login exitoso para el usuario: {request.Email}");

                return Ok(new
                {
                    success = true,
                    data = response,
                    message = "Login exitoso"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"Intento de login fallido para: {request.Email}");
                return Unauthorized(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error durante el login para: {request.Email}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error interno del servidor"
                });
            }
        }

        [HttpPost("validate")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> ValidateToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var isValid = await _authService.ValidateToken(token);

            return Ok(new
            {
                valid = isValid,
                user = User.Identity.Name,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        [HttpPost("logout")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Logout()
        {
            // En JWT stateless, el logout se maneja en el cliente
            // Pero podrías implementar una blacklist de tokens aquí
            return Ok(new
            {
                success = true,
                message = "Logout exitoso"
            });
        }
    }
}
