using api.Application.Dtos.Comment;
using api.Application.Mappers;
using api.Core.Entities;
using api.Core.Interfaces;
using api.Presentation.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Times = Moq.Times;

namespace api.Tests.Presentation.Controllers;

public class CommentControllerTests
{
    private readonly CommentController _controller;

    private readonly Mock<ICommentService> _mockCommentService;
    private readonly Mock<IStockService> _mockStockService;
    private readonly Mock<UserManager<AppUser>> _mockUserManager;

    public CommentControllerTests()
    {
        var store = new Mock<IUserStore<AppUser>>();

        _mockUserManager = new Mock<UserManager<AppUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );
        _mockCommentService = new Mock<ICommentService>();
        _mockStockService = new Mock<IStockService>();

        _controller = new CommentController(
            _mockCommentService.Object,
            _mockStockService.Object,
            _mockUserManager.Object
        );
    }

    private static AppUser CreateFakeUser()
    {
        return A.Fake<AppUser>();
    }

    [Fact]
    public async Task GetComments_ReturnsOkResult_WithListOfComments()
    {
        // Arrange
        var comments = A.Fake<List<Comment>>();
        var fakeUser = CreateFakeUser();

        _mockCommentService.Setup(service => service.GetAllAsync()).ReturnsAsync(comments);
        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(fakeUser);

        // Act
        var result = (OkObjectResult)await _controller.GetAll();
        await _mockUserManager.Object.FindByIdAsync("1");

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<List<CommentDto>>();
        _mockUserManager.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task GetCommentById_ReturnsNotFound_WhenCommentDoesNotExist()
    {
        // Arrange
        _mockCommentService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Comment?)null);

        // Act
        var result = (NotFoundResult)await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task CreateComment_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = (BadRequestObjectResult)await _controller.Create(1, new CreateCommentRequestDto());

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task CreateComment_ReturnsBadRequest_WhenStockDoesNotExist()
    {
        // Arrange
        _mockCommentService.Setup(service => service.CreateAsync(It.IsAny<Comment>()));

        // Act
        var result = (BadRequestObjectResult)await _controller.Create(1, new CreateCommentRequestDto());

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task UpdateComment_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Title", "Title is required");

        // Act
        var result = (BadRequestObjectResult)await _controller.Update(1, new UpdateCommentRequestDto());

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task UpdateComment_ReturnsNotFound_WhenCommentDoesNotExist()
    {
        // Arrange
        _mockCommentService.Setup(service => service.UpdateAsync(1, It.IsAny<Comment>())).ReturnsAsync((Comment?)null);

        // Act
        var result = (NotFoundObjectResult)await _controller.Update(1, new UpdateCommentRequestDto());

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task DeleteComment_ReturnsNotFound_WhenCommentDoesNotExist()
    {
        // Arrange
        _mockCommentService.Setup(service => service.DeleteAsync(1)).ReturnsAsync((Comment?)null);

        // Act
        var result = (NotFoundResult)await _controller.Delete(1);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task DeleteComment_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var comment = new Comment { Id = 1, Title = "Title", Content = "Content" };
        _mockCommentService.Setup(service => service.DeleteAsync(1)).ReturnsAsync(comment);

        // Act
        var result = (NoContentResult)await _controller.Delete(1);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }
}
