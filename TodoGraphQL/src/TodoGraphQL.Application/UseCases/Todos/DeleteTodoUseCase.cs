using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Todos;

public class DeleteTodoUseCase
{
    private readonly ITodoRepository _repository;

    public DeleteTodoUseCase(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(string id, string userId)
    {
        var deleted = await _repository.DeleteAsync(id, userId);
        if (!deleted)
            throw new DomainException("Tarefa não encontrada.");

        return true;
    }
}