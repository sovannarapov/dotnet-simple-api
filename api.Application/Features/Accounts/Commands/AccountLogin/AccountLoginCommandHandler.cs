using api.Application.Dtos.Account;
using api.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Application.Features.Accounts.Commands.AccountLogin;

public class AccountLoginCommandHandler : IRequestHandler<AccountLoginCommand, UserDto>
{
    private readonly IMediator _mediator;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountLoginCommandHandler(
        UserManager<AppUser> userManager,
        IMediator mediator,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _mediator = mediator;
        _signInManager = signInManager;
    }

    public async Task<UserDto> Handle(AccountLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(
            user => string.Equals(user.Email, request.LoginRequest.Email.ToLower()), CancellationToken.None
        ) ?? throw new UnauthorizedAccessException("Invalid email");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.LoginRequest.Password, false);

        if (!result.Succeeded) throw new UnauthorizedAccessException("Incorrect email or password");

        var command = new CreateTokenCommand(user);

        var token = await _mediator.Send(command, cancellationToken);

        return new UserDto
        {
            Username = user.UserName!,
            Email = user.Email!,
            Token = token
        };
    }
}