namespace TodoGraphQL.API.GraphQL.Inputs;

public record ExpenseInputGql(string Id, string Name, decimal Value);

public record SaveFinanceInput(
    int Month,
    int Year,
    decimal Salary,
    List<ExpenseInputGql> Fixed,
    List<ExpenseInputGql> Variable
);