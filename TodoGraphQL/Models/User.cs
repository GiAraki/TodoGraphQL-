namespace TodoGraphQL.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    // Um usuário tem muitos todos
    public List<Todo> Todos { get; set; } = new();
}