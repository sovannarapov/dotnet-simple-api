using api.Core.Entities;

namespace api.Core.Interfaces.IStock;

public interface IStockWriteRepository
{
    Task<Stock> CreateAsync(Stock stock);
    Task<Stock?> UpdateAsync(int id, Stock stock);
    Task<Stock?> DeleteAsync(int id);
}