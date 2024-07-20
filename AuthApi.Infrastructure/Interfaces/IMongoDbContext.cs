using MongoDB.Driver;

namespace AuthApi.Infrastructure.Interfaces
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}