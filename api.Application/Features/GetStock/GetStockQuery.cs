using api.Application.Dtos.Stock;
using api.Common.Helpers;
using MediatR;

namespace api.Application.Features.GetStock;

public record GetStockQuery(QueryObject QueryObject) : IRequest<List<StockDto>>;