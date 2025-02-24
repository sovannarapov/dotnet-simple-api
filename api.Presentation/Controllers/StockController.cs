using api.Common;
using api.Common.Helpers;
using api.Application.Dtos.Stock;
using api.Application.Interfaces;
using AutoMapper;
using api.Core.Entities;
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

    public StockController(IStockService stockService, IMapper mapper)
    {
        _stockService = stockService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
    {
        var stocks = await _stockService.GetAllAsync(queryObject);
        // var stockDto = stocks.Select(s => s).ToList();
        var stockDto = _mapper.Map<List<StockDto>>(stocks);
        
        return Ok(stockDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await _stockService.GetByIdAsync(id);

        if (stock == null) return NotFound();

        return Ok(stock);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
    
        var stock = _mapper.Map<Stock>(stockDto);
    
        await _stockService.CreateAsync(stock);
    
        return CreatedAtAction(nameof(GetById), new { stock.Id }, stock);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
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
