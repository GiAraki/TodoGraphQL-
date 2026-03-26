using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TodoGraphQL.Domain.Entities;

namespace TodoGraphQL.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        _database = client.GetDatabase(config["MongoDB:DatabaseName"]);
    }

    public IMongoCollection<Todo> Todos => _database.GetCollection<Todo>("todos");
    public IMongoCollection<User> Users => _database.GetCollection<User>("users");
}