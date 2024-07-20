using AuthApi.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AuthApi.Infrastructure.Database
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("AuthDb");
        }

        public IMongoCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);
    }
}
