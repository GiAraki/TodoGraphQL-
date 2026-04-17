using Microsoft.Extensions.Logging;
using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Finance;

public record ExpenseInput(string Id, string Name, decimal Value);

public class SaveFinanceUseCase
{
    private static readonly string[] MonthNames = [
        "", "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho",
        "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"
    ];

    private readonly IFinanceRepository _repository;
    private readonly ILogger<SaveFinanceUseCase> _logger;

    public SaveFinanceUseCase(IFinanceRepository repository, ILogger<SaveFinanceUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<FinanceDto> ExecuteAsync(
        string userId, int month, int year,
        decimal salary,
        List<ExpenseInput> fixed_,
        List<ExpenseInput> variable)
    {
        var existing = await _repository.GetByUserAndMonthAsync(userId, month, year);

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
            var newRecord = FinanceRecord.Create(userId, month, year, salary, fixedExpenses, variableExpenses);
            record = await _repository.UpsertAsync(newRecord);
        }

        _logger.LogInformation("Financeiro salvo para UserId: {UserId} | {Month}/{Year}", userId, month, year);

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
            r.Month,
            r.Year,
            $"{MonthNames[r.Month]}/{r.Year}",
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

    public static MonthSummaryDto ToSummary(FinanceRecord r)
    {
        var totalExpenses = r.Fixed.Sum(e => e.Value) + r.Variable.Sum(e => e.Value);
        var balance = r.Salary - totalExpenses;
        return new MonthSummaryDto(
            r.Month, r.Year,
            $"{MonthNames[r.Month]}/{r.Year}",
            r.Salary, totalExpenses, balance
        );
    }
}