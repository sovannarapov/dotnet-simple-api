using api.Core.Entities;

namespace api.Core.Interfaces;

public interface IPortfolioService
{
    Task<List<Stock>> GetUserPortfolio(AppUser appUser);
    
    Task<Portfolio> CreateAsync(Portfolio portfolio);

    Task<Portfolio> DeleteAsync(AppUser appUser, string symbol);
}
