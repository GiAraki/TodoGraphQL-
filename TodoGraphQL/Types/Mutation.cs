using HotChocolate.Authorization;
using TodoGraphQL.Models;

namespace TodoGraphQL.Types;

public class Mutation
{
    [Authorize] // ← só funciona com token válido
    public async Task<Todo> AddTodo(
        string title,
        [Service] TodoRepository repo)
        => await repo.Add(title);
}