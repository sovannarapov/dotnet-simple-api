using api.Application.Dtos.Stock;
using api.Application.Interfaces;
using api.Common.Helpers;
using AutoMapper;
using api.Core.Entities;
using api.Core.Interfaces;

namespace api.Application.Services;

public class StockService : IStockService
{
    private readonly IMapper _mapper;
    private readonly IStockRepository _stockRepository;

    public StockService(IStockRepository stockRepository, IMapper mapper)
    {
        _stockRepository = stockRepository;
        _mapper = mapper;
    }

    public async Task<List<StockDto>> GetAllAsync(QueryObject queryObject)
    {
        var stocks = await _stockRepository.GetAllAsync(queryObject);

        return _mapper.Map<List<StockDto>>(stocks);
    }

    public async Task<StockDto?> GetByIdAsync(int id)
    {
        var stock = await _stockRepository.GetByIdAsync(id);

        return stock != null ? _mapper.Map<StockDto>(stock) : null;
    }

    public async Task<StockDto?> GetBySymbolAsync(string symbol)
    {
        var stock = await _stockRepository.GetBySymbolAsync(symbol);

        return _mapper.Map<StockDto>(stock);
    }

    public async Task<StockDto> CreateAsync(Stock stock)
    {
        var createdStock = await _stockRepository.CreateAsync(stock);

        return _mapper.Map<StockDto>(createdStock);
    }

    public async Task<StockDto?> UpdateAsync(int id, UpdateStockDto updateStockRequestDto)
    {
        var stockToUpdate = _mapper.Map<Stock>(updateStockRequestDto);
        var updatedStock = await _stockRepository.UpdateAsync(id, stockToUpdate);

        return _mapper.Map<StockDto>(updatedStock);
    }

    public async Task<StockDto?> DeleteAsync(int id)
    {
        var deletedStock = await _stockRepository.DeleteAsync(id);

        return _mapper.Map<StockDto>(deletedStock);
    }

    public async Task<bool> StockExists(int id)
    {
        var stockExists = await _stockRepository.StockExists(id);

        return stockExists;
    }
}
