using api.Application.Dtos.Stock;
using api.Core.Entities;
using api.Core.Interfaces;
using AutoMapper;
using MediatR;

namespace api.Application.Stocks.Commands.UpdateStock;

public class UpdateStockCommandHandler : IRequestHandler<UpdateStockCommand, StockDto>
{
    private readonly IStockRepository _stockRepository;
    private readonly IMapper _mapper;
    
    public UpdateStockCommandHandler(IStockRepository stockRepository, IMapper mapper)
    {
        _stockRepository = stockRepository;
        _mapper = mapper;
    }
    
    public async Task<StockDto> Handle(UpdateStockCommand request, CancellationToken cancellationToken)
    {
        var stockToUpdate = _mapper.Map<Stock>(request.UpdateStockDto);
        var updatedStock = await _stockRepository.UpdateAsync(request.Id, stockToUpdate);
        
        if (updatedStock is null) throw new Exception("Stock not found.");

        return _mapper.Map<StockDto>(updatedStock);
    }
}