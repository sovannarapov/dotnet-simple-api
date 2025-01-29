using api.Common.Extensions;
using api.Core.Entities;
using api.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers;

[Route("api/v{apiVersion:apiVersion}/portfolio")]
[Authorize]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepository;
    private readonly IPortfolioRepository _portfolioRepository;
    
    public PortfolioController(
        UserManager<AppUser> userManager, 
        IStockRepository stockRepository, 
        IPortfolioRepository portfolioRepository
    )
    {
        _userManager = userManager;
        _stockRepository = stockRepository;
        _portfolioRepository = portfolioRepository;
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

        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }

    [HttpPost]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepository.GetBySymbolAsync(symbol);

        if (stock == null)
        {
            return BadRequest("Stock not found");
        }

        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
        {
            return BadRequest("Stock already in portfolio");
        }

        var portfolio = new Portfolio
        {
            AppUserId = appUser.Id,
            StockId = stock.Id
        };

        await _portfolioRepository.CreateAsync(portfolio);

        if (portfolio == null)
        {
            return BadRequest("Failed to add stock to portfolio");
        }

        return Created("Portfolio created successfully", new { portfolio.AppUserId, portfolio.StockId, portfolio.Stock });
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

        var filteredStock = userPortfolio.Where(stock => stock.Symbol.ToLower() == symbol.ToLower()).ToList();

        if (filteredStock.Count == 1)
        {
            await _portfolioRepository.DeleteAsync(appUser, symbol);
        }
        else
        {
            return BadRequest("Stock not found in portfolio");
        }

        return Ok();
    }
}
