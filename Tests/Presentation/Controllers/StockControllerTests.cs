using api.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using api.Presentation.Controllers;
using api.Core.Interfaces;
using api.Core.Entities;
using api.Application.Dtos.Stock;
using FluentAssertions;

namespace api.Tests.Presentation.Controllers;

public class StockControllerTests
{
    private readonly Mock<IStockService> _mockStockService;
    private readonly StockController _controller;
    
    public StockControllerTests()
    {
        _mockStockService = new Mock<IStockService>();
        _controller = new StockController(_mockStockService.Object);
    }
    
    [Fact]
    public async Task GetStocks_ReturnsOkResult_WithListOfStocks()
    {
        var queryObject = new QueryObject();
        // Arrange
        var stocks = new List<Stock>
        {
            new Stock { Id = 1, CompanyName = "AAPL", MarketCap = 150 },
            new Stock { Id = 2, CompanyName = "GOOGL", MarketCap = 2800 }
        };
    
        _mockStockService.Setup(repo => repo.GetAllAsync(queryObject)).ReturnsAsync(stocks);
    
        // Act
        var result = await _controller.GetAll(queryObject);
    
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeOfType<List<StockDto>>();
    }
}
