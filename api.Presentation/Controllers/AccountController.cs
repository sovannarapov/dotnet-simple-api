using api.Application.Dtos.Account;
using api.Application.Features.Accounts.Commands.AccountLogin;
using api.Application.Features.Accounts.Commands.AccountRegister;
using api.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Presentation.Controllers;

[Route(RouteConstants.AccountRoutePrefix)]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [SwaggerResponse(StatusCodes.Status200OK, "Login successful", typeof(UserDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error occurred. Check the input data.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access. Invalid email or password.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error. An unexpected error occurred.")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var command = new AccountLoginCommand(loginRequest);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error >>> " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost("register")]
    [SwaggerResponse(StatusCodes.Status200OK, "User successfully created")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation error occurred. Check the input data.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized access. Authentication is required.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error. An unexpected error occurred.")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        try
        {
            var command = new AccountRegisterCommand(registerRequest);

            await _mediator.Send(command);

            return Ok("User successfully created!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error >>> " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}