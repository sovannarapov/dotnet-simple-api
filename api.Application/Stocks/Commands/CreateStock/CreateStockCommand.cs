using api.Application.Dtos.Stock;
using MediatR;

namespace api.Application.Stocks.Commands.CreateStock;

public record CreateStockCommand(CreateStockDto CreateStockDto) : IRequest<StockDto>;