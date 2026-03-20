using Microsoft.EntityFrameworkCore;
using TodoGraphQL.Data;

namespace TodoGraphQL.Models;

public class UserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<User?> FindByEmail(string email)
        => _db.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User> Create(string email, string passwordHash)
    {
        var user = new User { Email = email, PasswordHash = passwordHash };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }
}