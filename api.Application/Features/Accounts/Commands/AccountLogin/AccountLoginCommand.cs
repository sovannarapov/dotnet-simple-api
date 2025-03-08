using api.Application.Dtos.Account;
using MediatR;

namespace api.Application.Features.Accounts.Commands.AccountLogin;

public record AccountLoginCommand(LoginRequest LoginRequest) : IRequest<UserDto>;