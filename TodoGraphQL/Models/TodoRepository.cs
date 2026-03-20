using TodoGraphQL.Data;

namespace TodoGraphQL.Models;

public class TodoRepository
{
    private readonly AppDbContext _db;

    public TodoRepository(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Todo> GetAll() => _db.Todos;

    public async Task<Todo> Add(string title)
    {
        var todo = new Todo { Title = title, IsCompleted = false };
        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();
        return todo;
    }
}