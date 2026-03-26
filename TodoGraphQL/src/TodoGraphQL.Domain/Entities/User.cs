namespace TodoGraphQL.Domain.Entities;

public class User
{
    public string Id { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public static User Create(string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("O email não pode ser vazio.");

        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Email = email.Trim().ToLower(),
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };
    }
}