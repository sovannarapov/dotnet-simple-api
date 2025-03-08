using api.Application.Dtos.Stock;
using api.Core.Interfaces.IStock;
using AutoMapper;
using MediatR;

namespace api.Application.Features.Stocks.Queries.GetStock;

public class GetStockQueryHandler : IRequestHandler<GetStockQuery, List<StockDto>>
{
    private readonly IMapper _mapper;
    private readonly IStockReadRepository _stockRepository;

    public GetStockQueryHandler(IStockReadRepository stockRepository, IMapper mapper)
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