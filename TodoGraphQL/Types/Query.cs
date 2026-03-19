using TodoGraphQL.Models;

namespace TodoGraphQL.Types;

public class Query
{
    // Quando alguém pedir "todos", retorna a lista completa
    public IEnumerable<Todo> GetTodos([Service] TodoRepository repo)
        => repo.GetAll();
}