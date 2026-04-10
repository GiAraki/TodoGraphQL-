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

    public async Task<FinanceRecord?> GetByUserIdAsync(string userId)
        => await _context.Finances.Find(f => f.UserId == userId).FirstOrDefaultAsync();

    public async Task<FinanceRecord> UpsertAsync(FinanceRecord record)
    {
        await _context.Finances.ReplaceOneAsync(
            f => f.UserId == record.UserId,
            record,
            new ReplaceOptions { IsUpsert = true });
        return record;
    }
}