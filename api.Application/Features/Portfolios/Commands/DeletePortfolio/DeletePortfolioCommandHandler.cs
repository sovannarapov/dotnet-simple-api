using api.Core.Entities;
using api.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace api.Application.Features.Portfolios.Commands.DeletePortfolio;

public class DeletePortfolioCommandHandler : IRequestHandler<DeletePortfolioCommand, string>
{
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly UserManager<AppUser> _userManager;

    public DeletePortfolioCommandHandler(IPortfolioRepository portfolioRepository, UserManager<AppUser> userManager)
    {
        _portfolioRepository = portfolioRepository;
        _userManager = userManager;
    }

    public async Task<string> Handle(DeletePortfolioCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByNameAsync(request.Username) ?? throw new KeyNotFoundException("User not found");
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio.Where(stock => string.Equals(stock.Symbol.ToLower(), request.Symbol.ToLower())).ToList();

        if (filteredStock.Count == 1)
            await _portfolioRepository.DeleteAsync(appUser, request.Symbol);
        else
            throw new KeyNotFoundException("Stock not found in portfolio");

        return "Portfolio deleted successfully.";
    }
}