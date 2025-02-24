using api.Application.Dtos.Stock;
using api.Application.Interfaces;
using api.Core.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using api.Presentation.Controllers;
using Times = Moq.Times;

namespace api.Tests.Presentation.Controllers;

public class PortfolioControllerTests
{
    private readonly PortfolioController _controller;
    private readonly Mock<IPortfolioService> _mockPortfolioService;
    private readonly Mock<IStockService> _mockStockService;
    private readonly Mock<UserManager<AppUser>> _mockUserManager;

    public PortfolioControllerTests()
    {
        var store = new Mock<IUserStore<AppUser>>();
        
        _mockUserManager = new Mock<UserManager<AppUser>>(
            store.Object, null, null, null, null, null, null, null, null);
        _mockStockService = new Mock<IStockService>();
        _mockPortfolioService = new Mock<IPortfolioService>();
        _controller = new PortfolioController(
            _mockUserManager.Object, _mockStockService.Object, _mockPortfolioService.Object);
    }

    private AppUser CreateFakeUser()
    {
        return new AppUser
        {
            Id = "fakeUserId",
            UserName = "fakeUser"
        };
    }

    [Fact]
    public async Task GetUserPortfolio_ReturnsOkResult_WithUserPortfolio()
    {
        // Arrange
        var fakeUser = CreateFakeUser();
        var fakePortfolio = new List<Stock>
        {
            new() { Id = 1, Symbol = "AAPL" },
            new() { Id = 2, Symbol = "GOOGL" }
        };

        _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(fakeUser);
        _mockPortfolioService.Setup(service => service.GetUserPortfolio(fakeUser)).ReturnsAsync(fakePortfolio);

        // Act
        var result = await _controller.GetUserPortfolio();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(fakePortfolio);
        _mockUserManager.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
        _mockPortfolioService.Verify(service => service.GetUserPortfolio(fakeUser), Times.Once);
    }

    [Fact]
    public async Task AddPortfolio_ReturnsCreatedResult_WhenStockIsAdded()
    {
        // Arrange
        var fakeUser = CreateFakeUser();
        var fakeStock = new StockDto { Id = 1, Symbol = "AAPL" };
        var fakeStocks = new List<Stock>();
        var portfolio = new Portfolio();

        _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(fakeUser);
        _mockStockService.Setup(s => s.GetBySymbolAsync(It.IsAny<string>())).ReturnsAsync(fakeStock);
        _mockPortfolioService.Setup(s => s.GetUserPortfolio(fakeUser)).ReturnsAsync(fakeStocks);
        _mockPortfolioService.Setup(s => s.CreateAsync(It.IsAny<Portfolio>())).ReturnsAsync(portfolio);

        // Act
        var result = await _controller.AddPortfolio("AAPL");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
        var createdResult = result as CreatedResult;
        createdResult!.StatusCode.Should().Be(StatusCodes.Status201Created);
        _mockUserManager.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
        _mockStockService.Verify(service => service.GetBySymbolAsync(It.IsAny<string>()), Times.Once);
        _mockPortfolioService.Verify(service => service.GetUserPortfolio(fakeUser), Times.Once);
        _mockPortfolioService.Verify(service => service.CreateAsync(It.IsAny<Portfolio>()), Times.Once);
    }

    [Fact]
    public async Task DeletePortfolio_ReturnsOkResult_WhenStockIsDeleted()
    {
        // Arrange
        var fakeUser = CreateFakeUser();
        var fakeStock = new Stock { Id = 1, Symbol = "AAPL" };
        var fakePortfolio = new List<Stock> { fakeStock };

        _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(fakeUser);
        _mockPortfolioService.Setup(service => service.GetUserPortfolio(fakeUser)).ReturnsAsync(fakePortfolio);
        _mockPortfolioService.Setup(service => service.DeleteAsync(fakeUser, fakeStock.Symbol))
            .ReturnsAsync(new Portfolio());

        // Act
        var result = await _controller.DeletePortfolio(fakeStock.Symbol);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkResult>();
        var okResult = result as OkResult;
        okResult!.StatusCode.Should().Be(StatusCodes.Status200OK);
        _mockUserManager.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
        _mockPortfolioService.Verify(service => service.GetUserPortfolio(fakeUser), Times.Once);
        _mockPortfolioService.Verify(service => service.DeleteAsync(fakeUser, fakeStock.Symbol), Times.Once);
    }

    [Fact]
    public async Task DeletePortfolio_ReturnsBadRequest_WhenStockNotFoundInPortfolio()
    {
        // Arrange
        var fakeUser = CreateFakeUser();
        var fakePortfolio = new List<Stock>();

        _mockUserManager.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(fakeUser);
        _mockPortfolioService.Setup(service => service.GetUserPortfolio(fakeUser)).ReturnsAsync(fakePortfolio);

        // Act
        var result = await _controller.DeletePortfolio("AAPL");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        badRequestResult.Value.Should().Be("Stock not found in portfolio");
        _mockUserManager.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
        _mockPortfolioService.Verify(service => service.GetUserPortfolio(fakeUser), Times.Once);
        _mockPortfolioService.Verify(service => service.DeleteAsync(It.IsAny<AppUser>(), It.IsAny<string>()),
            Times.Never);
    }
}