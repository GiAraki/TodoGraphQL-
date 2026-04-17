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

    // Busca um mês específico
    public async Task<FinanceDto?> ExecuteAsync(string userId, int month, int year)
    {
        var record = await _repository.GetByUserAndMonthAsync(userId, month, year);
        if (record == null) return null;
        return SaveFinanceUseCase.MapToDto(record);
    }

    // Busca todos os meses para o gráfico
    public async Task<List<MonthSummaryDto>> GetAllSummariesAsync(string userId)
    {
        var records = await _repository.GetAllByUserAsync(userId);
        return records.Select(SaveFinanceUseCase.ToSummary).ToList();
    }
}