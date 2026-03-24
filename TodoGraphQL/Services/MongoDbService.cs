using MongoDB.Driver;
using TodoGraphQL.Models;

namespace TodoGraphQL.Services;

public class MongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IConfiguration config)
    {
        var client = new MongoClient(config["MongoDB:ConnectionString"]);
        _database = client.GetDatabase(config["MongoDB:DatabaseName"]);
    }

    public IMongoCollection<Todo> Todos
        => _database.GetCollection<Todo>("todos");

    public IMongoCollection<User> Users
        => _database.GetCollection<User>("users");
}