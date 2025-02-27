using api.Application.Dtos.Stock;
using api.Core.Interfaces;
using AutoMapper;
using MediatR;

namespace api.Application.Stocks.Queries.GetStock;

public class GetStockQueryHandler : IRequestHandler<GetStockQuery, List<StockDto>>
{
    private readonly IStockRepository _stockRepository;
    private readonly IMapper _mapper;

    public GetStockQueryHandler(IStockRepository stockRepository, IMapper mapper)
    {
        _stockRepository = stockRepository;
        _mapper = mapper;
    }
    
    public async Task<List<StockDto>> Handle(GetStockQuery request, CancellationToken cancellationToken)
    {
        var stocks = await _stockRepository.GetAllAsync(request.QueryObject);
        
        return _mapper.Map<List<StockDto>>(stocks);
    }
}