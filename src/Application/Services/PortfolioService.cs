using api.Core.Entities;
using api.Core.Interfaces;

namespace api.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IPortfolioRepository _portfolioRepository;

    public PortfolioService(IPortfolioRepository portfolioRepository)
    {
        _portfolioRepository = portfolioRepository;
    }
    public async Task<List<Stock>> GetUserPortfolio(AppUser appUser)
    {
        var portfolio = await _portfolioRepository.GetUserPortfolio(appUser);

        return portfolio;
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        var createdPortfolio = await _portfolioRepository.CreateAsync(portfolio);

        return createdPortfolio;
    }

    public async Task<Portfolio> DeleteAsync(AppUser appUser, string symbol)
    {
        var deletedPortfolio = await _portfolioRepository.DeleteAsync(appUser, symbol);

        return deletedPortfolio;
    }
}
