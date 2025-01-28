using api.Application.Dtos.Stock;
using api.Application.Mappers;
using api.Common.Helpers;
using api.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Presentation.Controllers;

[Route("api/v{apiVersion:apiVersion}/stock")]
[Authorize]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<StockController> _logger;
    
    public StockController(IStockRepository stockRepository, ILogger<StockController> logger)
    {
        _stockRepository = stockRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject queryObject)
    {
        _logger.LogInformation("Getting all stocks");
        
        var stocks = await _stockRepository.GetAllAsync(queryObject);
        var stockDto = stocks.Select(s => s.ToStockDto()).ToList();

        return Ok(stockDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        _logger.LogInformation("Getting stock by id");
        
        var stock = await _stockRepository.GetByIdAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        _logger.LogInformation("Creating stock");
        
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stock = stockDto.ToStockFromCreateDto();

        await _stockRepository.CreateAsync(stock);
        
        _logger.LogInformation("Creating stock successful");
        
        return CreatedAtAction(nameof(GetById), new { Id = stock.Id }, stock.ToStockWithoutCommentsDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        _logger.LogInformation("Updating stock by id {id}", id);
        
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stock = await _stockRepository.UpdateAsync(id, updateDto);

        if (stock == null)
        {
            return NotFound();
        }
        
        _logger.LogInformation("Updating stock successful");

        return Ok(stock.ToStockWithoutCommentsDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        _logger.LogInformation("Deleting stock by id {id}", id);
        
        var stockModel = await _stockRepository.DeleteAsync(id);

        if (stockModel == null)
        {
            return NotFound();
        }
        
        _logger.LogInformation("Deleting stock successful");

        return NoContent();
    }
}
