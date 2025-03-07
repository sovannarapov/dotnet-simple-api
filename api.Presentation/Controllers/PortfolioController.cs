using api.Application.Features.Portfolios.Commands.CreatePortfolio;
using api.Application.Features.Portfolios.Commands.DeletePortfolio;
using api.Application.Features.Portfolios.Queries.GetPortfolio;
using api.Common;
using api.Common.Extensions;
using api.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers;

[Route(RouteConstants.PortfolioRoutePrefix)]
[Authorize]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly IMediator _mediator;

    public PortfolioController(
        UserManager<AppUser> userManager,
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserPortfolio()
    {
        try
        {
            var username = User.GetUsername();

            if (username is null) return NotFound("User not found");

            var command = new GetPortfolioQuery(username);

            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine("Error >>> " + ex.Message);
            return StatusCode(StatusCodes.Status404NotFound, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        try
        {
            var username = User.GetUsername();

            if (username is null) return NotFound("User not found");

            var command = new CreatePortfolioCommand(symbol, username);

            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error >>> " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        try {
            var username = User.GetUsername();

            if (username is null) return NotFound("User not found");

            var command = new DeletePortfolioCommand(symbol, username);

            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error >>> " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}