using api.Core.Entities;
using MediatR;

namespace api.Application.Features.Portfolios.Commands.CreatePortfolio;

public record CreatePortfolioCommand(string Symbol, string Username) : IRequest<Portfolio>;