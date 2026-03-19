namespace TodoGraphQL.Models;

// Singleton: uma única instância compartilhada em toda a aplicação
public class TodoRepository
{
    private readonly List<Todo> _todos = new()
    {
        new Todo { Id = 1, Title = "Aprender GraphQL", IsCompleted = false },
        new Todo { Id = 2, Title = "Aprender Docker",  IsCompleted = false },
    };

    private int _nextId = 3;

    // Retorna todas as tarefas
    public IEnumerable<Todo> GetAll() => _todos;

    // Adiciona uma nova tarefa e retorna ela
    public Todo Add(string title)
    {
        var todo = new Todo
        {
            Id = _nextId++,
            Title = title,
            IsCompleted = false
        };
        _todos.Add(todo);
        return todo;
    }
}