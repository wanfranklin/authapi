using AuthApi.Application.Interfaces;
using AuthApi.Domain.Entities;

namespace AuthApi.Application.Services
{
    public class UserService : IUserService
    {
        public UserProfile GetUserProfile(string username)
        {
            // Implementação para obter o perfil do usuário
            // Este é apenas um exemplo; substitua pela lógica real
            return new UserProfile
            {
                Username = username,
                FullName = "Full Name Example"
            };
        }
    }
}