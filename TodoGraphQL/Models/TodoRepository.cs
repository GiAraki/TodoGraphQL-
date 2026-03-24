using MongoDB.Driver;
using TodoGraphQL.Services;

namespace TodoGraphQL.Models;

public class TodoRepository
{
    private readonly MongoDbService _mongo;

    public TodoRepository(MongoDbService mongo)
    {
        _mongo = mongo;
    }

    public async Task<List<Todo>> GetAll(string userId)
        => await _mongo.Todos
            .Find(t => t.UserId == userId)
            .ToListAsync();

    public async Task<Todo> Add(string title, string userId)
    {
        var todo = new Todo { Title = title, IsCompleted = false, UserId = userId };
        await _mongo.Todos.InsertOneAsync(todo);
        return todo;
    }

    public async Task<Todo> Complete(string id, string userId)
    {
        var filter = Builders<Todo>.Filter.Where(t => t.Id == id && t.UserId == userId);
        var update = Builders<Todo>.Update.Set(t => t.IsCompleted, true);
        var todo = await _mongo.Todos.FindOneAndUpdateAsync(filter, update,
            new FindOneAndUpdateOptions<Todo> { ReturnDocument = ReturnDocument.After });

        return todo ?? throw new GraphQLException("Tarefa não encontrada.");
    }

    public async Task<bool> Delete(string id, string userId)
    {
        var result = await _mongo.Todos.DeleteOneAsync(t => t.Id == id && t.UserId == userId);
        return result.DeletedCount > 0;
    }
}