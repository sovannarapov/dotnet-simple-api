using MediatR;

namespace api.Application.Stocks.Commands.DeleteStock;

public record DeleteStockCommand(int Id) : IRequest;