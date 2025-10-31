using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionHive.Application.DTO.Request;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace VisionHive.API.Controllers

{
    [ApiController]
    [Route("api/v{apiVersion:apiVersion}/auth")]
    [Asp.Versioning.ApiVersion(2.0)]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        //  Esta rota é pública — não precisa de token
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // exemplo simples: usuário fixo
            if (request.Email == "admin@fiap.com" && request.Password == "123456")
            {
                var secretKey = _config["Jwt:SecretKey"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds,
                    claims: new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, request.Email),
                        new Claim("role", "admin")
                    });

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expires_in = "1h"
                });
            }

            return Unauthorized(new { message = "Credenciais inválidas" });
        }
    }
}