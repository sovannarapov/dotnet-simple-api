using api.Core.Entities;
using MediatR;

public record CreateTokenCommand(AppUser AppUser) : IRequest<string>;