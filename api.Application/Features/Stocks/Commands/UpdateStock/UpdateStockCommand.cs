using api.Application.Dtos.Stock;
using MediatR;

namespace api.Application.Features.Stocks.Commands.UpdateStock;

public record UpdateStockCommand(int Id, UpdateStockRequest UpdateStockRequest) : IRequest<StockDto>;