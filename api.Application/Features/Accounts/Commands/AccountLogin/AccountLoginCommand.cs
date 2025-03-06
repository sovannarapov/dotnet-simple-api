using api.Application.Dtos.Account;
using MediatR;

namespace api.Application.Features.Accounts.Commands.AccountLogin;

public record AccountLoginCommand(LoginRequestDto LoginRequestDto) : IRequest<NewUserDto>;