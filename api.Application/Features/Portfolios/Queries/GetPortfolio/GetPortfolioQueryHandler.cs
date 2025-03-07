using api.Core.Entities;
using api.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace api.Application.Features.Portfolios.Queries.GetPortfolio;

public class GetPortfolioQueryHandler : IRequestHandler<GetPortfolioQuery, List<Stock>>
{
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly UserManager<AppUser> _userManager;

    public GetPortfolioQueryHandler(IPortfolioRepository portfolioRepository, UserManager<AppUser> userManager)
    {
        _portfolioRepository = portfolioRepository;
        _userManager = userManager;
    }

    public async Task<List<Stock>> Handle(GetPortfolioQuery request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByNameAsync(request.Username) ?? throw new Exception("User not found");

        return await _portfolioRepository.GetUserPortfolio(appUser);
    }
}