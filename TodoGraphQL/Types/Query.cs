using HotChocolate.Authorization;
using TodoGraphQL.Models;
using TodoGraphQL.Services;

namespace TodoGraphQL.Types;

public class Query
{
    [Authorize]
    public async Task<List<Todo>> GetTodos(
        [Service] TodoRepository repo,
        [Service] UserContext userContext)
        => await repo.GetAll(userContext.GetUserId());
}