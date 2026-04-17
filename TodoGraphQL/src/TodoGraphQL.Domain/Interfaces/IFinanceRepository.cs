using TodoGraphQL.Domain.Entities;

namespace TodoGraphQL.Domain.Interfaces;

public interface IFinanceRepository
{
    Task<FinanceRecord?> GetByUserAndMonthAsync(string userId, int month, int year);
    Task<List<FinanceRecord>> GetAllByUserAsync(string userId);
    Task<FinanceRecord> UpsertAsync(FinanceRecord record);
}