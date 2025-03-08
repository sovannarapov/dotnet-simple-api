using api.Core.Interfaces.IStock;
using MediatR;

namespace api.Application.Features.Stocks.Commands.DeleteStock;

public class DeleteStockCommandHandler : IRequestHandler<DeleteStockCommand>
{
    private readonly IStockWriteRepository _stockRepository;

    public DeleteStockCommandHandler(IStockWriteRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task Handle(DeleteStockCommand request, CancellationToken cancellationToken)
    {
        await _stockRepository.DeleteAsync(request.Id);
    }
}