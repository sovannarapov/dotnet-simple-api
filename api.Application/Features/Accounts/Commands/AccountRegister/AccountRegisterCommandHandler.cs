using api.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace api.Application.Features.Accounts.Commands.AccountRegister;

public class AccountRegisterCommandHandler : IRequestHandler<AccountRegisterCommand, string>
{
    private readonly UserManager<AppUser> _userManager;

    public AccountRegisterCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<string> Handle(AccountRegisterCommand request, CancellationToken cancellationToken)
    {
        var appUser = new AppUser
        {
            UserName = request.RegisterAccountRequestDto.Username,
            Email = request.RegisterAccountRequestDto.Email
        };
        
        var createdUser = await _userManager.CreateAsync(appUser, request.RegisterAccountRequestDto.Password!);
        
        if (!createdUser.Succeeded) throw new Exception(createdUser.Errors.First().Description);
        
        await _userManager.AddToRoleAsync(appUser, "User");
        
        return "User successfully created!";
    }
}
