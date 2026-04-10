namespace TodoGraphQL.Application.DTOs;

public record ExpenseDto(string Id, string Name, decimal Value);

public record FinanceDto(
    string Id,
    decimal Salary,
    List<ExpenseDto> Fixed,
    List<ExpenseDto> Variable,
    decimal TotalFixed,
    decimal TotalVariable,
    decimal TotalExpenses,
    decimal Balance,
    decimal MarginPercent,
    DateTime UpdatedAt
);