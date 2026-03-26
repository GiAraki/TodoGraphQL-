using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Todos;

public class GetTodosUseCase
{
    private readonly ITodoRepository _repository;

    public GetTodosUseCase(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TodoDto>> ExecuteAsync(string userId)
    {
        var todos = await _repository.GetAllByUserAsync(userId);

        return todos.Select(t => new TodoDto(
            t.Id,
            t.Title,
            t.IsCompleted,
            t.CreatedAt
        )).ToList();
    }
}