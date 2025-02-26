using api.Core.Interfaces;
using MediatR;

namespace api.Application.Features.DeleteStock;

public class DeleteStockCommandHandler : IRequestHandler<DeleteStockCommand>
{
    private readonly IStockRepository _stockRepository;
    
    public DeleteStockCommandHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }
    
    public async Task Handle(DeleteStockCommand request, CancellationToken cancellationToken)
    {
        await _stockRepository.DeleteAsync(request.Id);
    }
}