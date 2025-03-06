using api.Application.Dtos.Account;
using api.Application.Interfaces;
using api.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Application.Features.Accounts.Commands.AccountLogin;

public class AccountLoginCommandHandler : IRequestHandler<AccountLoginCommand, NewUserDto>
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    public AccountLoginCommandHandler(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }
    
    public async Task<NewUserDto> Handle(AccountLoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(
            user => string.Equals(user.Email, request.LoginRequestDto.Email.ToLower()), CancellationToken.None
        );

        if (user is null) throw new UnauthorizedAccessException("Invalid email");

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.LoginRequestDto.Password, false);

        if (!result.Succeeded) throw new UnauthorizedAccessException("Incorrect email or password");

        return new NewUserDto
        {
            Username = user.UserName!,
            Email = user.Email!,
            Token = _tokenService.CreateToken(user)
        };
    }
}