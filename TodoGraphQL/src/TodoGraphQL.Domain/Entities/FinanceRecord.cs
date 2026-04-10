namespace TodoGraphQL.Domain.Entities;

public class Expense
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

public class FinanceRecord
{
    public string Id { get; private set; } = string.Empty;
    public string UserId { get; private set; } = string.Empty;
    public decimal Salary { get; private set; }
    public List<Expense> Fixed { get; private set; } = new();
    public List<Expense> Variable { get; private set; } = new();
    public DateTime UpdatedAt { get; private set; }

    private FinanceRecord() { }

    public static FinanceRecord Create(string userId, decimal salary, List<Expense> fixed_, List<Expense> variable)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new DomainException("UserId é obrigatório.");

        return new FinanceRecord
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Salary = salary,
            Fixed = fixed_,
            Variable = variable,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Update(decimal salary, List<Expense> fixed_, List<Expense> variable)
    {
        Salary = salary;
        Fixed = fixed_;
        Variable = variable;
        UpdatedAt = DateTime.UtcNow;
    }
}