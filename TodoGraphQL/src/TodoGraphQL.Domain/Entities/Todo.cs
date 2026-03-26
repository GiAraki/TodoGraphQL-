namespace TodoGraphQL.Domain.Entities;

public class Todo
{
    public string Id { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public bool IsCompleted { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    // Construtor privado — só factories criam entidades
    private Todo() { }

    // Factory method — garante que a entidade sempre nasce válida
    public static Todo Create(string title, string userId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("O título não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(userId))
            throw new DomainException("O usuário é obrigatório.");

        return new Todo
        {
            Id = Guid.NewGuid().ToString(),
            Title = title.Trim(),
            IsCompleted = false,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new DomainException("Esta tarefa já está concluída.");

        IsCompleted = true;
    }
}