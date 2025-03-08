using api.Application.Dtos.Stock;
using api.Core.Entities;
using api.Core.Interfaces.IStock;
using AutoMapper;
using MediatR;

namespace api.Application.Features.Stocks.Commands.UpdateStock;

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, StockDto>
{
    private readonly IMapper _mapper;
    private readonly IStockWriteRepository _stockRepository;

    public UpdateStockCommandHandler(IStockWriteRepository stockRepository, IMapper mapper)
    {
        _stockRepository = stockRepository;
        _mapper = mapper;
    }

    public async Task<StockDto> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var stockToUpdate = _mapper.Map<Stock>(request.UpdateStockRequest);
        var updatedStock = await _stockRepository.UpdateAsync(request.Id, stockToUpdate) ??
                           throw new Exception("Stock not found.");

        return _mapper.Map<StockDto>(updatedStock);
    }
}