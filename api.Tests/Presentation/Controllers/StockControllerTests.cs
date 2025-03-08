using api.Application.Dtos.Stock;
using api.Application.Interfaces;
using AutoMapper;
using api.Common.Helpers;
using api.Core.Entities;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using api.Presentation.Controllers;

namespace api.Tests.Presentation.Controllers;

public class StockControllerTests
{
    private readonly StockController _controller;
    private readonly Mock<IStockService> _mockStockService;

    public StockControllerTests()
    {
        var mockMapper = new Mock<IMapper>();
        
        _mockStockService = new Mock<IStockService>();
        _controller = new StockController(_mockStockService.Object, mockMapper.Object);
    }

    [Fact]
    public async Task GetStocks_ReturnsOkResult_WithListOfStocks()
    {
        // Arrange
        var queryObject = new QueryObject();
        var stocks = A.Fake<List<StockDto>>();

        _mockStockService.Setup(service => service.GetAllAsync(queryObject)).ReturnsAsync(stocks);

        // Act
        var result = (OkObjectResult) await _controller.GetAll(queryObject);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
        result.Value.Should().BeOfType<List<StockDto>>();
    }

    [Fact]
    public async Task GetStockById_ReturnsNotFound_WhenStockDoesNotExist()
    {
        // Arrange
        _mockStockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((StockDto?)null);

        // Act
        var result = (NotFoundResult)await _controller.GetById(1);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetStockById_ReturnsOk_WithStockDto()
    {
        // Arrange
        var stock = A.Fake<StockDto>();
        _mockStockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(stock);

        // Act
        var result = (OkObjectResult)await _controller.GetById(1);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
        result.Value.Should().BeOfType<StockDto>();
    }

    [Fact]
    public async Task CreateStock_ReturnsCreatedAtAction_WhenValidRequest()
    {
        // Arrange
        var newStockRequest = new CreateStockDto { CompanyName = "MSFT", Purchase = 310 };
        var createdStock = new StockDto { Id = 3, CompanyName = "MSFT", Purchase = 310 };

        _mockStockService.Setup(service => service.CreateAsync(It.IsAny<Stock>())).ReturnsAsync(createdStock);

        // Act
        var result = (CreatedAtActionResult)await _controller.Create(newStockRequest);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
        result.Should().NotBeNull();
        var returnValue = result.Value.Should().BeOfType<StockDto>().Subject;
        returnValue.CompanyName.Should().Be("MSFT");
    }

    [Fact]
    public async Task UpdateStock_ReturnsStock_WhenSuccessful()
    {
        // Arrange
        var updateRequest = A.Fake<UpdateStockDto>();
        var existingStock = A.Fake<StockDto>();

        _mockStockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(existingStock);
        _mockStockService.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<UpdateStockDto>()))
            .ReturnsAsync(existingStock);

        // Act
        var result = (OkObjectResult)await _controller.Update(1, updateRequest);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task DeleteStock_ReturnsNotFound_WhenStockDoesNotExist()
    {
        // Arrange
        _mockStockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((StockDto?)null);

        // Act
        var result = (NotFoundResult)await _controller.Delete(1);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteStock_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var existingStock = new StockDto { Id = 1, CompanyName = "AAPL", Purchase = 150 };

        _mockStockService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(existingStock);
        _mockStockService.Setup(service => service.DeleteAsync(It.IsAny<int>())).ReturnsAsync(existingStock);

        // Act
        var result = (NoContentResult)await _controller.Delete(1);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
