using AuthApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetUserProfile()
        {
            // Verifica se o usuário está autenticado
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Usuário não autenticado");
            }

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userProfile = _userService.GetUserProfile(username);

            if (userProfile == null)
            {
                return NotFound("Perfil não encontrado");
            }

            return Ok(userProfile);
        }
    }
}
