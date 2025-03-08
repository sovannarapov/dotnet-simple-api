using api.Core.Entities;
using api.Core.Interfaces.IPortfolio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace api.Application.Features.Portfolios.Commands.DeletePortfolio;

public class DeletePortfolioCommandHandler : IRequestHandler<DeletePortfolioCommand, string>
{
    private readonly IPortfolioReadRepository _portfolioReadRepository;
    private readonly IPortfolioWriteRepository _portfolioRepository;
    private readonly UserManager<AppUser> _userManager;

    public DeletePortfolioCommandHandler(
        IPortfolioWriteRepository portfolioRepository,
        UserManager<AppUser> userManager,
        IPortfolioReadRepository portfolioReadRepository)
    {
        _portfolioRepository = portfolioRepository;
        _portfolioReadRepository = portfolioReadRepository;
        _userManager = userManager;
    }

    public async Task<string> Handle(DeletePortfolioCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByNameAsync(request.Username) ??
                      throw new KeyNotFoundException("User not found");
        var userPortfolio = await _portfolioReadRepository.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio
            .Where(stock => string.Equals(stock.Symbol.ToLower(), request.Symbol.ToLower())).ToList();

        if (filteredStock.Count == 1)
            await _portfolioRepository.DeleteAsync(appUser, request.Symbol);
        else
            throw new KeyNotFoundException("Stock not found in portfolio");

        return "Portfolio deleted successfully.";
    }
}