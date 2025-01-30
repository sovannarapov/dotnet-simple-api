using api.Application.Dtos.Stock;
using api.Common.Helpers;
using api.Core.Entities;

namespace api.Core.Interfaces;

public interface IStockService
{
    Task<List<Stock>> GetAllAsync(QueryObject queryObject);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock?> GetBySymbolAsync(string symbol);
    Task<Stock> CreateAsync(Stock stock);
    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockRequestDto);
    Task<Stock?> DeleteAsync(int id);
    Task<bool> StockExists(int id);
}
