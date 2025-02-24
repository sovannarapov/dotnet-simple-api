using api.Common.Helpers;
using api.Core.Entities;
using api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace api.Tests.Infrastructure.Data;

public class StockRepositoryTests
{
    private static Task<ApplicationDbContext> GetDatabaseContext() => TestDatabaseHelper.GetDatabaseContext();

    [Fact]
    public async Task AddStock_ShouldAddStock()
    {
        var context = await GetDatabaseContext();
        var stock = new Stock { CompanyName = "Test Stock", Purchase = 100 };
        context.Stocks.Add(stock);
        await context.SaveChangesAsync();

        var addedStock = await context.Stocks.FirstOrDefaultAsync(s => s.CompanyName == "Test Stock");
        Assert.NotNull(addedStock);
        Assert.Equal(100, addedStock.Purchase);
    }

    [Fact]
    public async Task GetStockById_ShouldReturnStock()
    {
        var context = await GetDatabaseContext();
        var stock = new Stock { CompanyName = "Test Stock", Purchase = 100 };
        context.Stocks.Add(stock);
        await context.SaveChangesAsync();

        var retrievedStock = await context.Stocks.FindAsync(stock.Id);
        Assert.NotNull(retrievedStock);
        Assert.Equal("Test Stock", retrievedStock.CompanyName);
    }

    [Fact]
    public async Task UpdateStock_ShouldUpdateStock()
    {
        var context = await GetDatabaseContext();
        var stock = new Stock { CompanyName = "Test Stock", Purchase = 100 };
        context.Stocks.Add(stock);
        await context.SaveChangesAsync();

        stock.Purchase = 200;
        context.Stocks.Update(stock);
        await context.SaveChangesAsync();

        var updatedStock = await context.Stocks.FindAsync(stock.Id);
        Assert.NotNull(updatedStock);
        Assert.Equal(200, updatedStock.Purchase);
    }

    [Fact]
    public async Task DeleteStock_ShouldRemoveStock()
    {
        var context = await GetDatabaseContext();
        var stock = new Stock { CompanyName = "Test Stock", Purchase = 100 };
        context.Stocks.Add(stock);
        await context.SaveChangesAsync();

        context.Stocks.Remove(stock);
        await context.SaveChangesAsync();

        var deletedStock = await context.Stocks.FindAsync(stock.Id);
        Assert.Null(deletedStock);
    }

    [Fact]
    public async Task FilterStocks_ByCompanyNameAndIndustry_ShouldReturnFilteredStocks()
    {
        var context = await GetDatabaseContext();
        context.Stocks.AddRange(
            new Stock { CompanyName = "Tech Corp", Industry = "Technology", Symbol = "TC" },
            new Stock { CompanyName = "Health Inc", Industry = "Healthcare", Symbol = "HI" }
        );
        await context.SaveChangesAsync();

        var repository = new StockRepository(context);
        var queryObject = new QueryObject { CompanyName = "Tech", Industry = "Technology" };
        var filteredStocks = await repository.GetAllAsync(queryObject);

        Assert.Single(filteredStocks);
        Assert.Equal("Tech Corp", filteredStocks.First().CompanyName);
    }

    [Fact]
    public async Task SortStocks_BySymbol_ShouldReturnSortedStocks()
    {
        var context = await GetDatabaseContext();
        context.Stocks.AddRange(
            new Stock { CompanyName = "Tech Corp", Symbol = "TC" },
            new Stock { CompanyName = "Health Inc", Symbol = "HI" }
        );
        await context.SaveChangesAsync();

        var repository = new StockRepository(context);
        var queryObject = new QueryObject { SortBy = "Symbol", IsDescending = false };
        var sortedStocks = await repository.GetAllAsync(queryObject);

        Assert.Equal(10, sortedStocks.Count);
        Assert.Equal("Tesla", sortedStocks.First().CompanyName);
    }

    [Fact]
    public async Task PaginateStocks_ShouldReturnPaginatedStocks()
    {
        var context = await GetDatabaseContext();
        context.Stocks.AddRange(
            new Stock { CompanyName = "Tech Corp", Symbol = "TC" },
            new Stock { CompanyName = "Health Inc", Symbol = "HI" },
            new Stock { CompanyName = "Finance LLC", Symbol = "FL" }
        );
        await context.SaveChangesAsync();

        var repository = new StockRepository(context);
        var queryObject = new QueryObject { PageNumber = 1, PageSize = 2 };
        var paginatedStocks = await repository.GetAllAsync(queryObject);

        Assert.Equal(2, paginatedStocks.Count);
    }
}
