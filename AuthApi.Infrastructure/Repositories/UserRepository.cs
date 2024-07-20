using AuthApi.Domain.Entities;
using AuthApi.Domain.Interfaces;
using AuthApi.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace AuthApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserModel> _users;

        public UserRepository(IMongoDbContext context)
        {
            _users = context.GetCollection<UserModel>("Users");
        }

        public async Task<UserModel> GetUserByUsernameAsync(string username)
        {
            return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(UserModel user)
        {
            await _users.InsertOneAsync(user);
        }
    }
}
