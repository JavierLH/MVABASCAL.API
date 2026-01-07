using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaAduanero.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaAduanero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AduanaDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AduanaDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            // 1. Buscar usuario por Email y que esté Activo
            var usuario = await _context.Usuarios
                .Include(u => u.Rol) // Traemos el rol para meterlo al token
                .FirstOrDefaultAsync(u => u.Username == login.Username && u.Activo == true);

            if (usuario == null) return Unauthorized("Usuario no encontrado.");

            // 2. Validar contraseña (SIMULACIÓN TEMPORAL)
            // IMPORTANTE: En producción, aquí compararías el Hash, no texto plano.
            // Por ahora, asumimos que en la DB el password está plano para probar.
            bool esPasswordValido = BCrypt.Net.BCrypt.Verify(login.Password, usuario.PasswordHash);

            if (!esPasswordValido)
            {
                return Unauthorized("Usuario o contraseña incorrectos.");
            }

            // 3. Generar el Token JWT
            var token = GenerarToken(usuario);

            return Ok(new { token = token });
        }

        // POST: api/Auth/registro
        [HttpPost("registro")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] LoginDto registro)
        {
            // 1. Encriptar la contraseña antes de guardarla
            string passwordEncriptado = BCrypt.Net.BCrypt.HashPassword(registro.Password);

            // 2. Crear el usuario (Aquí hardcodeamos el cliente por ahora para probar)
            var nuevoUsuario = new Usuario
            {
                Username = registro.Username,
                PasswordHash = passwordEncriptado, // <--- Aquí guardamos el Hash, nunca el texto
                NombreCompleto = "JLopez",
                ClienteId = 1, // Asignado al cliente 1
                RolId = 1, // Rol Admin
                Activo = true
       
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok("Usuario creado con contraseña encriptada.");
        }

        private string GenerarToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Metemos datos dentro del token (Claims)
            var claims = new[]
             {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
        
                // CAMBIO AQUÍ: Guardamos el Username en el token
                new Claim(ClaimTypes.Name, usuario.Username),

                new Claim(ClaimTypes.Role, usuario.Rol.NombreRol),
                new Claim("ClienteId", usuario.ClienteId.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddHours(8), // El token dura 8 horas
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}