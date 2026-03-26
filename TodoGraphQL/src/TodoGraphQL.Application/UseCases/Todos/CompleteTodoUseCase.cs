using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Todos;

public class CompleteTodoUseCase
{
    private readonly ITodoRepository _repository;

    public CompleteTodoUseCase(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<TodoDto> ExecuteAsync(string id, string userId)
    {
        var todo = await _repository.GetByIdAsync(id, userId)
            ?? throw new DomainException("Tarefa não encontrada.");

        todo.Complete(); // regra de negócio no Domain

        var updated = await _repository.UpdateAsync(todo);
        return new TodoDto(updated.Id, updated.Title, updated.IsCompleted, updated.CreatedAt);
    }
}