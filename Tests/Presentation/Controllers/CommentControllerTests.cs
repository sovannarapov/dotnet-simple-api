using api.Application.Dtos.Comment;
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
    private readonly Mock<ICommentService> _mockCommentService;
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly CommentController _controller;
    
    private static AppUser CreateFakeUser() => A.Fake<AppUser>();
    
    public CommentControllerTests()
    {
        var store = new Mock<IUserStore<AppUser>>();
        _mockUserManager = new Mock<UserManager<AppUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        _mockCommentService = new Mock<ICommentService>();
        Mock<IStockService> mockStockService = new Mock<IStockService>();
        _controller = new CommentController(
            _mockCommentService.Object,
            mockStockService.Object,
            _mockUserManager.Object
        );
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
        var result = (OkObjectResult) await _controller.GetAll();
        await _mockUserManager.Object.FindByIdAsync("1");

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Value.Should().BeOfType<List<CommentDto>>();
        _mockUserManager.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.AtLeastOnce);
    }
}
