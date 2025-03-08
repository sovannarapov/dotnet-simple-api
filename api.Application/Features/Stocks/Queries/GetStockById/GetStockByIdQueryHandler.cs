using api.Application.Dtos.Stock;
using api.Core.Interfaces.IStock;
using AutoMapper;
using MediatR;

namespace api.Application.Features.Stocks.Queries.GetStockById;

public class GetStockByIdQueryHandler : IRequestHandler<GetStockByIdQuery, StockDto>
{
    private readonly IMapper _mapper;
    private readonly IStockReadRepository _stockRepository;

    public GetStockByIdQueryHandler(IStockReadRepository stockRepository, IMapper mapper)
    {
        _stockRepository = stockRepository;
        _mapper = mapper;
    }

    public async Task<StockDto> Handle(GetStockByIdQuery request, CancellationToken cancellationToken)
    {
        var stock = await _stockRepository.GetByIdAsync(request.Id) ?? throw new Exception("Stock not found.");

        return _mapper.Map<StockDto>(stock);
    }
}