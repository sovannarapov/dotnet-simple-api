using api.Application.Dtos.Stock;
using MediatR;

namespace api.Application.Features.Stocks.Commands.UpdateStock;

public record UpdateStockCommand(int Id, UpdateStockDto UpdateStockDto) : IRequest<StockDto>;