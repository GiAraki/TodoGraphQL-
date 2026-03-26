using TodoGraphQL.Domain.Entities;

namespace TodoGraphQL.Domain.Interfaces;

public interface ITodoRepository
{
    Task<List<Todo>> GetAllByUserAsync(string userId);
    Task<Todo?> GetByIdAsync(string id, string userId);
    Task<Todo> AddAsync(Todo todo);
    Task<Todo> UpdateAsync(Todo todo);
    Task<bool> DeleteAsync(string id, string userId);
}