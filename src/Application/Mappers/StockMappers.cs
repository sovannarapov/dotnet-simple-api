using Application.Dtos.Stock;
using Core.Entities;
using AutoMapper;

namespace Application.Mappers;

public class StockMappers : Profile
{
    public StockMappers()
    {
        CreateMap<Stock, StockDto>().ReverseMap();
        CreateMap<Stock, CreateStockRequestDto>().ReverseMap();
        CreateMap<Stock, UpdateStockRequestDto>().ReverseMap();
    }
}
