using API_FarmaciaChavarria.Context;
using API_FarmaciaChavarria.Models;
using API_FarmaciaChavarria.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_FarmaciaChavarria.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly GenerateToken _generateToken;

        public LoginController(AppDbContext context, GenerateToken generateToken)
        {
            _context = context;
            _generateToken = generateToken;
        }

        [EnableRateLimiting("loginLimiter")]
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            var usuario = await _context.Usuarios
        .FirstOrDefaultAsync(u => u.Nombre == userLogin.Nombre);

            if (usuario == null)
            {
                return Unauthorized("Usuario no encontrado");
            }

            if (usuario.Pin != userLogin.Pin)
            {
                return Unauthorized("Contraseña incorrecta");
            }

            var token = _generateToken.GenerateJwtToken(usuario.Nombre, usuario.Rol);
            return Ok(new
            {
                token,
                usuario.Rol
            });
        }


    }
}