using api.Core.Entities;
using MediatR;

namespace api.Application.Features.Portfolios.Queries.GetPortfolio;

public record GetPortfolioQuery(string Username) : IRequest<List<Stock>>;