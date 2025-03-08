using api.Application.Dtos.Account;
using MediatR;

namespace api.Application.Features.Accounts.Commands.AccountRegister;

public record AccountRegisterCommand(RegisterRequest RegisterRequest) : IRequest<string>;