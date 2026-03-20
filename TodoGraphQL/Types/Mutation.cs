using HotChocolate.Authorization;
using TodoGraphQL.Models;
using TodoGraphQL.Services;

namespace TodoGraphQL.Types;

public class Mutation
{
    [Authorize]
    public async Task<Todo> AddTodo(
        string title,
        [Service] TodoRepository repo,
        [Service] UserContext userContext)
        => await repo.Add(title, userContext.GetUserId());

    [Authorize]
    public async Task<Todo> CompleteTodo(
        int id,
        [Service] TodoRepository repo,
        [Service] UserContext userContext)
        => await repo.Complete(id, userContext.GetUserId());

    [Authorize]
    public async Task<bool> DeleteTodo(
        int id,
        [Service] TodoRepository repo,
        [Service] UserContext userContext)
        => await repo.Delete(id, userContext.GetUserId());
}