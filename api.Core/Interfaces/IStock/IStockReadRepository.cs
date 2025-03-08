using api.Common.Helpers;
using api.Core.Entities;

namespace api.Core.Interfaces.IStock;

public interface IStockReadRepository
{
    Task<List<Stock>> GetAllAsync(QueryObject queryObject);
    Task<Stock?> GetByIdAsync(int id);
    Task<Stock?> GetBySymbolAsync(string symbol);
    Task<bool> StockExists(int id);
}