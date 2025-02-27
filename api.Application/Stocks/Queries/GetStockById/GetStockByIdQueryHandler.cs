using api.Application.Dtos.Stock;
using api.Core.Interfaces;
using AutoMapper;
using MediatR;

namespace api.Application.Stocks.Queries.GetStockById;

public class GetStockByIdQueryHandler : IRequestHandler<GetStockByIdQuery, StockDto>
{
    private readonly IStockRepository _stockRepository;
    private readonly IMapper _mapper;
    
    public GetStockByIdQueryHandler(IStockRepository stockRepository, IMapper mapper)
    {
        _stockRepository = stockRepository;
        _mapper = mapper;
    }
    
    public async Task<StockDto> Handle(GetStockByIdQuery request, CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByIdAsync(request.Id);
        
        if (stock is null) throw new Exception("Stock not found.");

        return _mapper.Map<StockDto>(stock);
    }
}