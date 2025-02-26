using MediatR;

namespace api.Application.Features.DeleteStock;

public record DeleteStockCommand(int Id) : IRequest;