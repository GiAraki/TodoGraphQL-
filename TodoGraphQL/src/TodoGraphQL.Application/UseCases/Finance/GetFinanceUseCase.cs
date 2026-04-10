using TodoGraphQL.Application.DTOs;
using TodoGraphQL.Domain.Interfaces;

namespace TodoGraphQL.Application.UseCases.Finance;

public class GetFinanceUseCase
{
    private readonly IFinanceRepository _repository;

    public GetFinanceUseCase(IFinanceRepository repository)
    {
        _repository = repository;
    }

    public async Task<FinanceDto?> ExecuteAsync(string userId)
    {
        var record = await _repository.GetByUserIdAsync(userId);
        if (record == null) return null;
        return SaveFinanceUseCase.MapToDto(record);
    }
}