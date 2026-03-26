using MongoDB.Driver;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Infrastructure.Persistence;

public class TodoRepository : ITodoRepository
{
    private readonly MongoDbContext _context;

    public TodoRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Todo>> GetAllByUserAsync(string userId)
        => await _context.Todos.Find(t => t.UserId == userId).ToListAsync();

    public async Task<Todo?> GetByIdAsync(string id, string userId)
        => await _context.Todos.Find(t => t.Id == id && t.UserId == userId).FirstOrDefaultAsync();

    public async Task<Todo> AddAsync(Todo todo)
    {
        await _context.Todos.InsertOneAsync(todo);
        return todo;
    }

    public async Task<Todo> UpdateAsync(Todo todo)
    {
        await _context.Todos.ReplaceOneAsync(t => t.Id == todo.Id, todo);
        return todo;
    }

    public async Task<bool> DeleteAsync(string id, string userId)
    {
        var result = await _context.Todos.DeleteOneAsync(t => t.Id == id && t.UserId == userId);
        return result.DeletedCount > 0;
    }
}