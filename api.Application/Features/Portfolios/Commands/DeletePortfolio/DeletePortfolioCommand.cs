using MediatR;

namespace api.Application.Features.Portfolios.Commands.DeletePortfolio;

public record DeletePortfolioCommand(string Symbol, string Username) : IRequest<string>;