using AuthApi.Domain.Entities;

namespace AuthApi.Application.Interfaces
{
    public interface IUserService
    {
        UserProfile GetUserProfile(string username);
    }
}