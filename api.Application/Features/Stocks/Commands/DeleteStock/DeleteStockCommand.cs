using MediatR;

namespace api.Application.Features.Stocks.Commands.DeleteStock;

public record DeleteStockCommand(int Id) : IRequest;