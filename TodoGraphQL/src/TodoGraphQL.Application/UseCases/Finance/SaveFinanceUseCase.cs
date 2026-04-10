using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace TodoGraphQL.Application.UseCases.Finance;

public record ExpenseInput(string Id, string Name, decimal Value);

public class SaveFinanceUseCase
{
    private readonly IFinanceRepository _repository;
    private readonly ILogger<SaveFinanceUseCase> _logger;

    public SaveFinanceUseCase(IFinanceRepository repository, ILogger<SaveFinanceUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<FinanceDto> ExecuteAsync(
        string userId,
        decimal salary,
        List<ExpenseInput> fixed_,
        List<ExpenseInput> variable)
    {
        var existing = await _repository.GetByUserIdAsync(userId);

        var fixedExpenses = fixed_.Select(e => new Expense { Id = e.Id, Name = e.Name, Value = e.Value }).ToList();
        var variableExpenses = variable.Select(e => new Expense { Id = e.Id, Name = e.Name, Value = e.Value }).ToList();

        FinanceRecord record;
        if (existing != null)
        {
            existing.Update(salary, fixedExpenses, variableExpenses);
            record = await _repository.UpsertAsync(existing);
        }
        else
        {
            var newRecord = FinanceRecord.Create(userId, salary, fixedExpenses, variableExpenses);
            record = await _repository.UpsertAsync(newRecord);
        }

        _logger.LogInformation("Financeiro salvo para UserId: {UserId}", userId);

        return MapToDto(record);
    }

    public static FinanceDto MapToDto(FinanceRecord r)
    {
        var totalFixed = r.Fixed.Sum(e => e.Value);
        var totalVariable = r.Variable.Sum(e => e.Value);
        var totalExpenses = totalFixed + totalVariable;
        var balance = r.Salary - totalExpenses;
        var margin = r.Salary > 0 ? Math.Round((balance / r.Salary) * 100, 1) : 0;

        return new FinanceDto(
            r.Id,
            r.Salary,
            r.Fixed.Select(e => new ExpenseDto(e.Id, e.Name, e.Value)).ToList(),
            r.Variable.Select(e => new ExpenseDto(e.Id, e.Name, e.Value)).ToList(),
            totalFixed,
            totalVariable,
            totalExpenses,
            balance,
            margin,
            r.UpdatedAt
        );
    }
}