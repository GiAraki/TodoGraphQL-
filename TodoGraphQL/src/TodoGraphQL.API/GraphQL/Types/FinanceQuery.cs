using HotChocolate.Authorization;
using System.Security.Claims;
using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Application.UseCases.Finance;

namespace TodoGraphQL.API.GraphQL.Types;

[ExtendObjectType("Query")]
public class FinanceQuery
{
    [Authorize]
    public async Task<FinanceDto?> GetFinance(
        [Service] GetFinanceUseCase useCase,
        ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return await useCase.ExecuteAsync(userId);
    }
}