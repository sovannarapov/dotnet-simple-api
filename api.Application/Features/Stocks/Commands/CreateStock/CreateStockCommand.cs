using api.Application.Dtos.Stock;
using MediatR;

namespace api.Application.Features.Stocks.Commands.CreateStock;

public record CreateStockCommand(CreateStockRequest CreateStockRequest) : IRequest<StockDto>;