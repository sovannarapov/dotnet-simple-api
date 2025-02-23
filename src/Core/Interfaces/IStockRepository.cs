using Common.Helpers;
using Core.Entities;

namespace Core.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync(QueryObject queryObject);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock?> GetBySymbolAsync(string symbol);
    Task<Stock> CreateAsync(Stock stock);
    Task<Stock?> UpdateAsync(int id, Stock stock);
    Task<Stock?> DeleteAsync(int id);
    Task<bool> StockExists(int id);
}
