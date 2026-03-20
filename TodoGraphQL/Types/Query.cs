using HotChocolate.Authorization;
using TodoGraphQL.Models;
using TodoGraphQL.Services;

namespace TodoGraphQL.Types;

public class Query
{
    [Authorize]
    public IQueryable<Todo> GetTodos(
        [Service] TodoRepository repo,
        [Service] UserContext userContext)
        => repo.GetAll(userContext.GetUserId());
}