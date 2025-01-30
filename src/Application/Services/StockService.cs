using api.Application.Dtos.Stock;
using api.Common.Helpers;
using api.Core.Entities;
using api.Core.Interfaces;

namespace api.Application.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;

    public StockService(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }
    
    public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
    {
        var stocks = await _stockRepository.GetAllAsync(queryObject);
        
        return stocks;
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        var stock = await _stockRepository.GetByIdAsync(id);
        
        return stock;
    }

    public async Task<Stock> CreateAsync(Stock stock)
    {
        var createdStock = await _stockRepository.CreateAsync(stock);
        
        return createdStock;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockRequestDto)
    {
        var updatedStock = await _stockRepository.UpdateAsync(id, updateStockRequestDto);
        
        return updatedStock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var deletedStock = await _stockRepository.DeleteAsync(id);
        
        return deletedStock;
    }
}