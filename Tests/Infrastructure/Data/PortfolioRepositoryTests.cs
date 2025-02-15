using api.Core.Entities;
using api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace api.Tests.Infrastructure.Data;

public class PortfolioRepositoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public PortfolioRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private static Task<ApplicationDbContext> GetDatabaseContext()
    {
        return TestDatabaseHelper.GetDatabaseContext();
    }

    [Fact]
    public async Task AddPortfolio_ShouldAddPortfolio()
    {
        var context = await GetDatabaseContext();
        var portfolio = new Portfolio
            { StockId = 2, AppUserId = "6c0b1d17-0fa8-491c-92ed-029ea7c96889", Stock = new Stock { Symbol = "TS" } };
        context.Portfolios.Add(portfolio);
        await context.SaveChangesAsync();

        var addedPortfolio = await context.Portfolios.FirstOrDefaultAsync(p => p.StockId == portfolio.StockId);
        Assert.NotNull(addedPortfolio);
        Assert.Equal(2, addedPortfolio.StockId);
        Assert.Equal("6c0b1d17-0fa8-491c-92ed-029ea7c96889", addedPortfolio.AppUserId);
        Assert.Equal("TS", addedPortfolio.Stock.Symbol);
    }

    [Fact]
    public async Task GetUserPortfolio_ShouldReturnPortfolio()
    {
        var context = await GetDatabaseContext();
        var user = new AppUser { Id = "6c0b1d17-0fa8-491c-92ed-029ea7c96889" };
        context.Users.Add(user);
        var portfolio = new Portfolio { StockId = 2, AppUserId = user.Id, Stock = new Stock { Symbol = "TS" } };
        context.Portfolios.Add(portfolio);
        await context.SaveChangesAsync();

        var foundedPortfolio = await context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == user.Id);

        Assert.NotNull(foundedPortfolio);
        Assert.Equal(2, foundedPortfolio.StockId);
        Assert.Equal(user.Id, foundedPortfolio.AppUserId);
        Assert.Equal("TS", foundedPortfolio.Stock.Symbol);
    }

    [Fact]
    public async Task DeletePortfolio_ShouldRemovePortfolio()
    {
        var context = await GetDatabaseContext();
        var user = new AppUser { Id = "9a1c6e94-d927-4ecf-9554-815e27b3bc65" };
        var stock = new Stock { Symbol = "TS" };
        var portfolio = new Portfolio { StockId = 2, AppUserId = user.Id, Stock = stock };
        context.Portfolios.Add(portfolio);
        await context.SaveChangesAsync();

        var deletedPortfolio =
            await context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == user.Id && p.Stock.Symbol == stock.Symbol);
        _testOutputHelper.WriteLine("Deleted Portfolio: " + deletedPortfolio);
        context.Portfolios.Remove(deletedPortfolio!);
        await context.SaveChangesAsync();

        var foundedPortfolio =
            await context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == user.Id && p.Stock.Symbol == stock.Symbol);
        Assert.Null(foundedPortfolio);
    }
}
