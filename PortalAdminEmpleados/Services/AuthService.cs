using System;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using PortalAdminEmpleados.Data;
using PortalAdminEmpleados.Helpers;
using PortalAdminEmpleados.Modelos;
using PortalAdminEmpleados.Modelos.Entidades;
using Microsoft.Extensions.Configuration;

namespace PortalAdminEmpleados.Services
{
    public class AuthService : IAuthService
    {
        private readonly Conexion _context;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _configuracion;

        public AuthService(Conexion context, JwtHelper jwtHelper, IConfiguration configuracion)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _configuracion = configuracion;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest request)
        {
            // Buscar usuario por email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

            if (user == null)
                throw new UnauthorizedAccessException("Credenciales inválidas");

            // Verificar contraseña (usando BCrypt)
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Credenciales inválidas");

            // Verificar si es administrador
            if (string.IsNullOrEmpty(user.Rol) || !user.Rol.Contains("Admin"))
                throw new UnauthorizedAccessException("Acceso denegado. Se requieren privilegios de administrador");

            // Actualizar último login
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Generar token JWT localmente (evita llamar a un método inexistente en JwtHelper)
            var secret = _configuracion["Jwt:Key"];
            var issuer = _configuracion["Jwt:Issuer"];
            var audience = _configuracion["Jwt:Audience"];
            var expireHoursConfig = _configuracion["Jwt:ExpireHours"];
            if (string.IsNullOrEmpty(secret))
                throw new InvalidOperationException("Configuración inválida: Jwt:Key no encontrada.");

            double expireHours = 1;
            if (!string.IsNullOrEmpty(expireHoursConfig) && double.TryParse(expireHoursConfig, out var parsed))
                expireHours = parsed;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Usuario ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Rol ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expireHours),
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return new LoginResponse
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(expireHours),
                User = new LoginResponse.UserInfo
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Usuario,
                    Role = user.Rol
                }
            };
        }
        
        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id) ?? default!;
        }

        public Task<bool> ValidateToken(string token)
        {
            var principal = _jwtHelper.ValidateToken(token);
            return Task.FromResult(principal != null);
        }
    }
}


