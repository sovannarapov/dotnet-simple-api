using api.Application.Dtos.Stock;
using api.Common.Helpers;
using MediatR;

namespace api.Application.Stocks.Queries.GetStock;

public record GetStockQuery(QueryObject QueryObject) : IRequest<List<StockDto>>;