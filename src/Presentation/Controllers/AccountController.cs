using api.Application.Dtos.Account;
using api.Common;
using api.Core.Entities;
using api.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Presentation.Controllers;

[Route(RouteConstants.AccountRoutePrefix)]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, 
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    [SwaggerResponse(StatusCodes.Status200OK, "Login successful", typeof(NewUserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error occurred. Check the input data.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access. Invalid email or password.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error. An unexpected error occurred.")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var user = await _userManager.Users.FirstOrDefaultAsync(
            user => user.Email == loginRequestDto.Email.ToLower(), CancellationToken.None
        );

        if (user == null) return Unauthorized("Invalid email");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, false);

        if (!result.Succeeded) return Unauthorized("Incorrect email or password");

        return Ok(new NewUserDto
        {
            Username = user.UserName!,
            Email = user.Email!,
            Token = _tokenService.CreateToken(user)
        });
    }

    [HttpPost("register")]
    [SwaggerResponse(StatusCodes.Status200OK, "User successfully created")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error occurred. Check the input data.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access. Authentication is required.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error. An unexpected error occurred.")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountRequestDto registerAccountRequestDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = new AppUser
            {
                UserName = registerAccountRequestDto.Username,
                Email = registerAccountRequestDto.Email
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerAccountRequestDto.Password!);

            if (!createdUser.Succeeded) return StatusCode(500, createdUser.Errors);

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

            return roleResult.Succeeded ? Ok("User successfully created") : StatusCode(500, createdUser.Errors);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }
}
