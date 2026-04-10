namespace TodoGraphQL.API.GraphQL.Inputs;

public record ExpenseInputGql(string Id, string Name, decimal Value);

public record SaveFinanceInput(
    decimal Salary,
    List<ExpenseInputGql> Fixed,
    List<ExpenseInputGql> Variable
);