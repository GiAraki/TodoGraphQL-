using HotChocolate.Authorization;
using System.Security.Claims;
using TodoGraphQL.API.GraphQL.Inputs;
using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Application.UseCases.Finance;

namespace TodoGraphQL.API.GraphQL.Types;

[ExtendObjectType("Mutation")]
public class FinanceMutation
{
    [Authorize]
    public async Task<FinanceDto> SaveFinance(
        SaveFinanceInput input,
        [Service] SaveFinanceUseCase useCase,
        ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var fixed_ = input.Fixed.Select(e => new ExpenseInput(e.Id, e.Name, e.Value)).ToList();
        var variable = input.Variable.Select(e => new ExpenseInput(e.Id, e.Name, e.Value)).ToList();
        return await useCase.ExecuteAsync(userId, input.Month, input.Year, input.Salary, fixed_, variable);
    }
}