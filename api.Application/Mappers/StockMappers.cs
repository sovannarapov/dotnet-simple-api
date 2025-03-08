using api.Application.Dtos.Stock;
using api.Core.Entities;
using AutoMapper;

namespace api.Application.Mappers;

public class StockMappers : Profile
{
    public StockMappers()
    {
        CreateMap<Stock, StockDto>().ReverseMap();
        CreateMap<Stock, CreateStockRequest>().ReverseMap();
        CreateMap<Stock, UpdateStockRequest>().ReverseMap();
    }
}