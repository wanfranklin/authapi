using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using AuthApi.Application.Interfaces;
using AuthApi.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly string _secretKey;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _authService = authService;
            _logger = logger;
            _secretKey = configuration["Jwt:Key"]; // Obter a chave secreta do appsettings.json
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestModel request)
        {
            await _authService.RegisterAsync(request.Username, request.Password);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var token = await _authService.AuthenticateAsync(request.Username, request.Password);
            return Ok(new { Token = token });
        }

        [HttpGet("generate-key")]
        public IActionResult GenerateKey()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var key = new byte[32];
                rng.GetBytes(key);
                var base64Key = Convert.ToBase64String(key);
                return Ok(new { Key = base64Key });
            }
        }

        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] TokenRequestModel request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            try
            {
                tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true, // Verifica a expiração do token
                    ValidateIssuerSigningKey = true, // Verifica a assinatura do token
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                return Ok(new { IsValid = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsValid = false, Message = ex.Message });
            }
        }
    }
}