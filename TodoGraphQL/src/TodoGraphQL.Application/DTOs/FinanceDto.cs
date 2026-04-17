namespace TodoGraphQL.Application.DTOs;

public record ExpenseDto(string Id, string Name, decimal Value);

public record FinanceDto(
    string Id,
    int Month,
    int Year,
    string MonthLabel,
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

// Para o gráfico — retorna apenas os totais por mês
public record MonthSummaryDto(
    int Month,
    int Year,
    string MonthLabel,
    decimal Salary,
    decimal TotalExpenses,
    decimal Balance
);