using Microsoft.EntityFrameworkCore;
using TodoGraphQL.Data;

namespace TodoGraphQL.Models;

public class TodoRepository
{
    private readonly AppDbContext _db;

    public TodoRepository(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Todo> GetAll(int userId)
        => _db.Todos.Where(t => t.UserId == userId);

    public async Task<Todo> Add(string title, int userId)
    {
        var todo = new Todo { Title = title, IsCompleted = false, UserId = userId };
        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();
        return todo;
    }

    public async Task<Todo> Complete(int id, int userId)
    {
        var todo = await _db.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId)
            ?? throw new GraphQLException("Tarefa não encontrada.");

        todo.IsCompleted = true;
        await _db.SaveChangesAsync();
        return todo;
    }

    public async Task<bool> Delete(int id, int userId)
    {
        var todo = await _db.Todos
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId)
            ?? throw new GraphQLException("Tarefa não encontrada.");

        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();
        return true;
    }
}