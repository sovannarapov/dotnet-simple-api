using api.Application.Dtos.Stock;
using api.Core.Entities;
using api.Core.Interfaces;
using AutoMapper;
using MediatR;

namespace api.Application.Features.Stocks.Commands.CreateStock;

public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, StockDto>
{
    private readonly IStockRepository _stockRepository;
    private readonly IMapper _mapper;
    
    public CreateStockCommandHandler(IStockRepository stockRepository, IMapper mapper)
    {
        _stockRepository = stockRepository;
        _mapper = mapper;
    }
    
    public async Task<StockDto> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        var stock = _mapper.Map<Stock>(request.CreateStockDto);
        var createdStock = await _stockRepository.CreateAsync(stock);

        return _mapper.Map<StockDto>(createdStock);
    }
}