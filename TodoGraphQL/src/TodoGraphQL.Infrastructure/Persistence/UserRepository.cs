using MongoDB.Driver;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly MongoDbContext _context;

    public UserRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindByEmailAsync(string email)
        => await _context.Users.Find(u => u.Email == email).FirstOrDefaultAsync();

    public async Task<User> CreateAsync(User user)
    {
        await _context.Users.InsertOneAsync(user);
        return user;
    }
}