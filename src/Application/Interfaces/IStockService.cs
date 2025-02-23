using Common.Helpers;
using Application.Dtos.Stock;
using Core.Entities;

namespace Application.Interfaces;

public interface IStockService
{
    Task<List<StockDto>> GetAllAsync(QueryObject queryObject);
    Task<StockDto?> GetByIdAsync(int id);
    Task<StockDto?> GetBySymbolAsync(string symbol);
    Task<StockDto> CreateAsync(Stock stock);
    Task<StockDto?> UpdateAsync(int id, UpdateStockRequestDto updateStockRequestDto);
    Task<StockDto?> DeleteAsync(int id);
    Task<bool> StockExists(int id);
}
