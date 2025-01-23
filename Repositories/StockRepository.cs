using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
        {
            var stocks = _context
                .Stocks
                .Include(stock => stock.Comments)
                .ThenInclude(comment => comment.AppUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryObject.CompanyName))
            {
                stocks = stocks.Where(stock => stock.CompanyName.Contains(queryObject.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.Industry))
            {
                stocks = stocks.Where(stock => stock.Industry.Contains(queryObject.Industry));
            }

            if (!string.IsNullOrWhiteSpace(queryObject.SortBy))
            {
                stocks = queryObject.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)
                    ? (queryObject.IsDescending ? stocks.OrderByDescending(stock => stock.Symbol) : stocks.OrderBy(stock => stock.Symbol))
                    : stocks;
            }

            var skip = (queryObject.PageNumber - 1) * queryObject.PageSize;
            var take = queryObject.PageSize;

            return await stocks.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks
                .Include(stock => stock.Comments)
                .ThenInclude(comment => comment.AppUser)
                .FirstOrDefaultAsync(stock => stock.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(stock => stock.Symbol == symbol);
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(stock => stock.Id == id);
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

            if (existingStock == null)
            {
                return null;
            }

            existingStock.Symbol = updateDto.Symbol;
            existingStock.CompanyName = updateDto.CompanyName;
            existingStock.Purchase = updateDto.Purchase;
            existingStock.LastDiv = updateDto.LastDiv;
            existingStock.Industry = updateDto.Industry;
            existingStock.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();

            return existingStock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

            if (stockModel == null)
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);

            await _context.SaveChangesAsync();

            return stockModel;
        }
    }
}
