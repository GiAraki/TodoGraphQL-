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
    public int Month { get; private set; }  // ← novo
    public int Year { get; private set; }   // ← novo
    public decimal Salary { get; private set; }
    public List<Expense> Fixed { get; private set; } = new();
    public List<Expense> Variable { get; private set; } = new();
    public DateTime UpdatedAt { get; private set; }

    private FinanceRecord() { }

    public static FinanceRecord Create(
        string userId, int month, int year,
        decimal salary, List<Expense> fixed_, List<Expense> variable)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new DomainException("UserId é obrigatório.");

        if (month < 1 || month > 12)
            throw new DomainException("Mês inválido.");

        if (year < 2000 || year > 2100)
            throw new DomainException("Ano inválido.");

        return new FinanceRecord
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Month = month,
            Year = year,
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