using AuthApi.Domain.Entities;

namespace AuthApi.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<UserModel> GetUserByUsernameAsync(string username);
        Task AddUserAsync(UserModel user);
    }
}