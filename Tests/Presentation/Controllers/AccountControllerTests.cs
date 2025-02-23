using Application.Dtos.Account;
using Application.Interfaces;
using Core.Entities;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MockQueryable;
using Moq;
using Presentation.Controllers;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace api.Tests.Presentation.Controllers;

public class AccountControllerTests
{
    private readonly AccountController _controller;
    private readonly Mock<SignInManager<AppUser>> _mockSignInManager;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<UserManager<AppUser>> _mockUserManager;

    public AccountControllerTests()
    {
        var store = new Mock<IUserStore<AppUser>>();

        _mockUserManager = new Mock<UserManager<AppUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        _mockTokenService = new Mock<ITokenService>();
        _mockSignInManager = new Mock<SignInManager<AppUser>>(
            _mockUserManager.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<AppUser>>().Object,
            null!, null!, null!, null!
        );

        _controller = new AccountController(
            _mockUserManager.Object,
            _mockTokenService.Object,
            _mockSignInManager.Object
        );
    }

    private static AppUser CreateFakeUser()
    {
        return A.Fake<AppUser>();
    }

    [Fact]
    public async Task Login_ReturnsOkResult_WithNewUserDto()
    {
        // Arrange
        var fakeUser = CreateFakeUser();
        var loginRequestDto = new LoginRequestDto
        {
            Email = fakeUser.Email,
            Password = "password"
        };

        var userList = new List<AppUser> { fakeUser }.AsQueryable().BuildMock();
        _mockUserManager.Setup(um => um.Users).Returns(userList);
        _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(fakeUser, loginRequestDto.Password, false))
            .Returns(Task.FromResult(SignInResult.Success));
        _mockTokenService.Setup(ts => ts.CreateToken(fakeUser)).Returns("token");

        // Act
        var result = (OkObjectResult)await _controller.Login(loginRequestDto);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().BeOfType<NewUserDto>();
    }
    
    [Fact]
    public async Task Login_ReturnsBadRequest_WhenPasswordIsNull()
    {
        // Arrange
        var loginRequestDto = new LoginRequestDto
        {
            Email = "user@example.com",
            Password = null
        };

        _controller.ModelState.AddModelError("Password", "Password is required");

        // Act
        var result = (BadRequestObjectResult)await _controller.Login(loginRequestDto);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Register_ReturnsOkResult_WithNewUserDto()
    {
        // Arrange
        var registerAccountRequestDto = A.Fake<RegisterAccountRequestDto>();
        var fakeUser = CreateFakeUser();

        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), registerAccountRequestDto.Password!))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), "User"))
            .ReturnsAsync(IdentityResult.Success);
        _mockTokenService.Setup(ts => ts.CreateToken(It.IsAny<AppUser>())).Returns("token");

        // Act
        var result = (OkObjectResult)await _controller.Register(registerAccountRequestDto);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().BeOfType<NewUserDto>();
    }
    
    [Fact]
    public async Task Register_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = (BadRequestObjectResult)await _controller.Register(A.Fake<RegisterAccountRequestDto>());

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public async Task Register_ReturnsInternalServerError_WhenUserCreationFails()
    {
        // Arrange
        var registerAccountRequestDto = A.Fake<RegisterAccountRequestDto>();
        var fakeUser = CreateFakeUser();

        _mockUserManager.Setup(um => um.CreateAsync(fakeUser, registerAccountRequestDto.Password))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        var result = (ObjectResult)await _controller.Register(registerAccountRequestDto);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task Register_ReturnsInternalServerError_WhenRoleAssignmentFails()
    {
        // Arrange
        var registerAccountRequestDto = A.Fake<RegisterAccountRequestDto>();
        var fakeUser = CreateFakeUser();

        _mockUserManager.Setup(um => um.CreateAsync(fakeUser, registerAccountRequestDto.Password))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.AddToRoleAsync(fakeUser, "User"))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        var result = (ObjectResult)await _controller.Register(registerAccountRequestDto);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task Register_ReturnsInternalServerError_WhenExceptionIsThrown()
    {
        // Arrange
        var registerAccountRequestDto = A.Fake<RegisterAccountRequestDto>();

        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), registerAccountRequestDto.Password!))
            .Throws<Exception>();

        // Act
        var result = (ObjectResult)await _controller.Register(registerAccountRequestDto);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserDoesNotExist()
    {
        // Arrange
        var loginRequestDto = new LoginRequestDto
        {
            Email = "nonexistentuser@example.com",
            Password = "password"
        };

        var userList = new List<AppUser>().AsQueryable().BuildMock();
        _mockUserManager.Setup(um => um.Users).Returns(userList);

        // Act
        var result = (ObjectResult)await _controller.Login(loginRequestDto);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrect()
    {
        // Arrange
        var fakeUser = CreateFakeUser();
        var loginRequestDto = new LoginRequestDto
        {
            Email = fakeUser.Email,
            Password = "wrongpassword"
        };

        var userList = new List<AppUser> { fakeUser }.AsQueryable().BuildMock();
        _mockUserManager.Setup(um => um.Users).Returns(userList);
        _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(fakeUser, loginRequestDto.Password, false))
            .Returns(Task.FromResult(SignInResult.Failed));

        // Act
        var result = (ObjectResult)await _controller.Login(loginRequestDto);

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public async Task Login_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _controller.ModelState.AddModelError("Email", "Email is required");

        // Act
        var result = (BadRequestObjectResult)await _controller.Login(A.Fake<LoginRequestDto>());

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }
}