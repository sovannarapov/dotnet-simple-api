using api.Core.Entities;
using api.Core.Interfaces.IPortfolio;
using api.Core.Interfaces.IStock;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace api.Application.Features.Portfolios.Commands.CreatePortfolio;

public class CreatePortfolioCommandHandler : IRequestHandler<CreatePortfolioCommand, Portfolio>
{
    private readonly IPortfolioReadRepository _portfolioReadRepository;
    private readonly IPortfolioWriteRepository _portfolioRepository;
    private readonly IStockReadRepository _stockRepository;
    private readonly UserManager<AppUser> _userManager;

    public CreatePortfolioCommandHandler(
        IStockReadRepository stockRepository,
        IPortfolioWriteRepository portfolioRepository,
        IPortfolioReadRepository portfolioReadRepository,
        UserManager<AppUser> userManager
    )
    {
        _stockRepository = stockRepository;
        _portfolioRepository = portfolioRepository;
        _portfolioReadRepository = portfolioReadRepository;
        _userManager = userManager;
    }

    public async Task<Portfolio> Handle(CreatePortfolioCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByNameAsync(request.Username) ??
                      throw new BadHttpRequestException("User does not exist");
        var stock = await _stockRepository.GetBySymbolAsync(request.Symbol) ??
                    throw new KeyNotFoundException("Stock not found");

        var userPortfolio = await _portfolioReadRepository.GetUserPortfolio(appUser);

        if (userPortfolio.Any(s => string.Equals(s.Symbol.ToLower(), request.Symbol.ToLower())))
            throw new BadHttpRequestException("Stock already in portfolio");

        var portfolio = new Portfolio
        {
            AppUserId = appUser.Id,
            StockId = stock.Id
        };

        await _portfolioRepository.CreateAsync(portfolio);

        if (portfolio is null) throw new BadHttpRequestException("Failed to add stock to portfolio");

        return portfolio;
    }
}