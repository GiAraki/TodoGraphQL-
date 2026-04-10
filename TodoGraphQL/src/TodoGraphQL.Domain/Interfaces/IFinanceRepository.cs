using TodoGraphQL.Domain.Entities;

namespace TodoGraphQL.Domain.Interfaces;

public interface IFinanceRepository
{
    Task<FinanceRecord?> GetByUserIdAsync(string userId);
    Task<FinanceRecord> UpsertAsync(FinanceRecord record);
}