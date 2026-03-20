namespace TodoGraphQL.Models;

public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    // Relacionamento com o usuário
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}