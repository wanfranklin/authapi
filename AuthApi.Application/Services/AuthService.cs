using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AuthApi.Application.Interfaces;
using AuthApi.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using AuthApi.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace AuthApi.Application.Services
{
    public class AuthService(IUserRepository userRepository, ILogger<AuthService> logger, IConfiguration configuration) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly string _jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("JWT Key not configured");

        public async Task<string> AuthenticateAsync(string username, string password)
        {
            _logger.LogInformation("Autenticando usuário {Username}", username);
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogWarning("Usuário {Username} não encontrado", username);
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning("Senha incorreta para usuário {Username}", username);
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }

            _logger.LogInformation("Gerando token para usuário {Username}", username);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task RegisterAsync(string username, string password)
        {
            _logger.LogInformation("Registrando novo usuário {Username}", username);
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                _logger.LogWarning("Usuário {Username} já existe", username);
                throw new InvalidOperationException("Usuário já existe");
            }

            var user = new UserModel
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            await _userRepository.AddUserAsync(user);
        }
    }
}
