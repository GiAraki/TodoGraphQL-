using MongoDB.Driver;
using TodoGraphQL.Domain.Entities;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Infrastructure.Persistence;

public class FinanceRepository : IFinanceRepository
{
    private readonly MongoDbContext _context;

    public FinanceRepository(MongoDbContext context)
    {
        _context = context;
    }

    public async Task<FinanceRecord?> GetByUserAndMonthAsync(string userId, int month, int year)
        => await _context.Finances
            .Find(f => f.UserId == userId && f.Month == month && f.Year == year)
            .FirstOrDefaultAsync();

    public async Task<List<FinanceRecord>> GetAllByUserAsync(string userId)
        => await _context.Finances
            .Find(f => f.UserId == userId)
            .SortBy(f => f.Year)
            .ThenBy(f => f.Month)
            .ToListAsync();

    public async Task<FinanceRecord> UpsertAsync(FinanceRecord record)
    {
        await _context.Finances.ReplaceOneAsync(
            f => f.UserId == record.UserId && f.Month == record.Month && f.Year == record.Year,
            record,
            new ReplaceOptions { IsUpsert = true });
        return record;
    }
}