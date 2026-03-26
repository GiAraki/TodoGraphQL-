using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Todos;

public class AddTodoUseCase
{
    private readonly ITodoRepository _repository;

    public AddTodoUseCase(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<TodoDto> ExecuteAsync(string title, string userId)
    {
        var todo = Todo.Create(title, userId); // regra de negócio no Domain
        var saved = await _repository.AddAsync(todo);

        return new TodoDto(saved.Id, saved.Title, saved.IsCompleted, saved.CreatedAt);
    }
}