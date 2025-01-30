using api.Common;
using api.Common.Extensions;
using api.Core.Entities;
using api.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers;

[Route(RouteConstants.PortfolioRoutePrefix)]
[Authorize]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockService _stockService;
    private readonly IPortfolioService _portfolioService;
    
    public PortfolioController(
        UserManager<AppUser> userManager,
        IStockService stockService,
        IPortfolioService portfolioService
    )
    {
        _userManager = userManager;
        _stockService = stockService;
        _portfolioService = portfolioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        if (appUser == null)
        {
            return NotFound("User not found");
        }

        var userPortfolio = await _portfolioService.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }

    [HttpPost]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockService.GetBySymbolAsync(symbol);

        if (stock == null)
        {
            return BadRequest("Stock not found");
        }

        var userPortfolio = await _portfolioService.GetUserPortfolio(appUser);

        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
        {
            return BadRequest("Stock already in portfolio");
        }

        var portfolio = new Portfolio
        {
            AppUserId = appUser.Id,
            StockId = stock.Id
        };

        await _portfolioService.CreateAsync(portfolio);

        if (portfolio == null)
        {
            return BadRequest("Failed to add stock to portfolio");
        }

        return Created("Portfolio created successfully", new
        {
            portfolio.AppUserId, 
            portfolio.StockId, 
            portfolio.Stock
        });
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioService.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio.Where(stock => stock.Symbol.ToLower() == symbol.ToLower()).ToList();

        if (filteredStock.Count == 1)
        {
            await _portfolioService.DeleteAsync(appUser, symbol);
        }
        else
        {
            return BadRequest("Stock not found in portfolio");
        }

        return Ok();
    }
}
