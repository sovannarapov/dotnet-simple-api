using api.Application.Dtos.Stock;
using api.Common.Helpers;
using api.Core.Entities;

namespace api.Application.Interfaces;

public interface IStockService
{
    Task<List<StockDto>> GetAllAsync(QueryObject queryObject);
    Task<StockDto?> GetByIdAsync(int id);
    Task<StockDto?> GetBySymbolAsync(string symbol);
    Task<StockDto> CreateAsync(Stock stock);
    Task<StockDto?> UpdateAsync(int id, UpdateStockDto updateStockRequestDto);
    Task<StockDto?> DeleteAsync(int id);
    Task<bool> StockExists(int id);
}
