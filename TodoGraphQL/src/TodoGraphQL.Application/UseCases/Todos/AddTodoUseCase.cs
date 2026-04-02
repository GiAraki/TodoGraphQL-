using Microsoft.Extensions.Logging;
using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Todos;

public class AddTodoUseCase
{
    private readonly ITodoRepository _repository;
    private readonly ILogger<AddTodoUseCase> _logger;

    public AddTodoUseCase(ITodoRepository repository, ILogger<AddTodoUseCase> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<TodoDto> ExecuteAsync(string title, string userId)
    {
        _logger.LogInformation("Criando todo para UserId: {UserId} | Título: {Title}", userId, title);

        var todo = Todo.Create(title, userId);
        var saved = await _repository.AddAsync(todo);

        _logger.LogInformation("Todo criado com sucesso: {TodoId}", saved.Id);

        return new TodoDto(saved.Id, saved.Title, saved.IsCompleted, saved.CreatedAt);
    }
}