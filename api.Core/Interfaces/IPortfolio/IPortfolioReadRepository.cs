using api.Core.Entities;

namespace api.Core.Interfaces.IPortfolio;

public interface IPortfolioReadRepository
{
    Task<List<Stock>> GetUserPortfolio(AppUser appUser);
}