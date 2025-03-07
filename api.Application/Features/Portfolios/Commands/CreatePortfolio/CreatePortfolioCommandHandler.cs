using api.Application.Interfaces;
using api.Core.Entities;
using api.Core.Interfaces.IPortfolio;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace api.Application.Features.Portfolios.Commands.CreatePortfolio;

public class CreatePortfolioCommandHandler : IRequestHandler<CreatePortfolioCommand, Portfolio>
{
    private readonly IPortfolioWriteRepository _portfolioRepository;
    private readonly IPortfolioReadRepository _portfolioReadRepository;
    private readonly IStockService _stockService;
    private readonly UserManager<AppUser> _userManager;

    public CreatePortfolioCommandHandler(
        IStockService stockService,
        IPortfolioWriteRepository portfolioRepository,
        IPortfolioReadRepository portfolioReadRepository,
        UserManager<AppUser> userManager
    )
    {
        _stockService = stockService;
        _portfolioRepository = portfolioRepository;
        _portfolioReadRepository = portfolioReadRepository;
        _userManager = userManager;
    }

    public async Task<Portfolio> Handle(CreatePortfolioCommand request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByNameAsync(request.Username) ?? throw new BadHttpRequestException("User does not exist");
        var stock = await _stockService.GetBySymbolAsync(request.Symbol) ?? throw new KeyNotFoundException("Stock not found");

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