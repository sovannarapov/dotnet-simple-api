using api.Core.Entities;

namespace api.Core.Interfaces.IPortfolio;

public interface IPortfolioWriteRepository
{
    Task<Portfolio> CreateAsync(Portfolio portfolio);

    Task<Portfolio> DeleteAsync(AppUser appUser, string symbol);
}