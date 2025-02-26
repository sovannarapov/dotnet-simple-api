using api.Application.Dtos.Stock;
using MediatR;

namespace api.Application.Features.GetStockById;

public record GetStockByIdQuery(int Id) : IRequest<StockDto>;