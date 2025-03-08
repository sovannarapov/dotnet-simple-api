using api.Application.Dtos.Stock;
using api.Core.Entities;
using api.Core.Interfaces.IStock;
using AutoMapper;
using MediatR;

namespace api.Application.Features.Stocks.Commands.CreateStock;

public class CreateStockCommandHandler : IRequestHandler<CreateStockCommand, StockDto>
{
    private readonly IMapper _mapper;
    private readonly IStockWriteRepository _stockRepository;

    public CreateStockCommandHandler(IStockWriteRepository stockRepository, IMapper mapper)
    {
        _stockRepository = stockRepository;
        _mapper = mapper;
    }

    public async Task<StockDto> Handle(CreateStockCommand request, CancellationToken cancellationToken)
    {
        var stock = _mapper.Map<Stock>(request.CreateStockRequest);
        var createdStock = await _stockRepository.CreateAsync(stock);

        return _mapper.Map<StockDto>(createdStock);
    }
}