using api.Application.Dtos.Stock;
using api.Application.Mappers;
using api.Common;
using api.Common.Helpers;
using api.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers;

[Route(RouteConstants.StockRoutePrefix)]
[Authorize]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockService _stockService;

    public StockController(IStockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
    {
        var stocks = await _stockService.GetAllAsync(queryObject);
        var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

        return Ok(stockDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await _stockService.GetByIdAsync(id);

        if (stock == null) return NotFound();

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var stock = stockDto.ToStockFromCreateDto();

        await _stockService.CreateAsync(stock);

        return CreatedAtAction(nameof(GetById), new { stock.Id }, stock.ToStockWithoutCommentsDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var stock = await _stockService.UpdateAsync(id, updateDto);

        if (stock == null) return NotFound();

        return Ok(stock.ToStockWithoutCommentsDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await _stockService.DeleteAsync(id);

        if (stockModel == null) return NotFound();

        return NoContent();
    }
}
