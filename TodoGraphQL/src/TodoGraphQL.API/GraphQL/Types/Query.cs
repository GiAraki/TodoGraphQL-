using HotChocolate.Authorization;
using System.Security.Claims;
using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Application.UseCases.Todos;

namespace TodoGraphQL.API.GraphQL.Types;

public class Query
{
    [Authorize]
    public async Task<List<TodoDto>> GetTodos(
        [Service] GetTodosUseCase useCase,
        ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return await useCase.ExecuteAsync(userId);
    }
}