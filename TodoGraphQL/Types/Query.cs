using HotChocolate.Authorization;
using TodoGraphQL.Models;

namespace TodoGraphQL.Types;

public class Query
{
    [Authorize] // ← só funciona com token válido
    public IEnumerable<Todo> GetTodos([Service] TodoRepository repo)
        => repo.GetAll();
}