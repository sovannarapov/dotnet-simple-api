using api.Application.Dtos.Stock;
using api.Application.Features.CreateStock;
using api.Application.Features.GetStock;
using api.Application.Interfaces;
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
    private readonly IStockService _stockService;
    private readonly IMediator _mediator;

    public StockController(IStockService stockService, IMapper mapper, IMediator mediator)
    {
        _stockService = stockService;
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
        var stock = await _stockService.GetByIdAsync(id);

        if (stock == null) return NotFound();

        return Ok(stock);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto createStockDto)
    {
        try
        {
            var command = new CreateStockCommand(createStockDto);
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
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var stock = await _stockService.UpdateAsync(id, updateDto);

        if (stock == null) return NotFound();

        return Ok(stock);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await _stockService.DeleteAsync(id);

        if (stockModel == null) return NotFound();

        return NoContent();
    }
}