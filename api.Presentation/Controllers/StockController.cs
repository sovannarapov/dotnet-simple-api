using api.Application.Dtos.Stock;
using api.Application.Features.Stocks.Commands.CreateStock;
using api.Application.Features.Stocks.Commands.DeleteStock;
using api.Application.Features.Stocks.Commands.UpdateStock;
using api.Application.Features.Stocks.Queries.GetStock;
using api.Application.Features.Stocks.Queries.GetStockById;
using api.Common;
using api.Common.Helpers;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers;

[Route(RouteConstants.StockRoutePrefix)]
[Authorize]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public StockController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
    {
        try
        {
            var query = new GetStockQuery(queryObject);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var query = new GetStockByIdQuery(id);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequest createStockRequest)
    {
        try
        {
            var command = new CreateStockCommand(createStockRequest);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequest updateRequest)
    {
        try
        {
            var command = new UpdateStockCommand(id, updateRequest);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var command = new DeleteStockCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR >>> " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }
}