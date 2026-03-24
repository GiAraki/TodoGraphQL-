using MongoDB.Driver;
using TodoGraphQL.Services;

namespace TodoGraphQL.Models;

public class UserRepository
{
    private readonly MongoDbService _mongo;

    public UserRepository(MongoDbService mongo)
    {
        _mongo = mongo;
    }

    public async Task<User?> FindByEmail(string email)
        => await _mongo.Users
            .Find(u => u.Email == email)
            .FirstOrDefaultAsync();

    public async Task<User> Create(string email, string passwordHash)
    {
        var user = new User { Email = email, PasswordHash = passwordHash };
        await _mongo.Users.InsertOneAsync(user);
        return user;
    }
}