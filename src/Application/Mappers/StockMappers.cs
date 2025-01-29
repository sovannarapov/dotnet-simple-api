using api.Application.Dtos.Stock;
using api.Core.Entities;

namespace api.Application.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stock)
    {
        return new StockDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            Purchase = stock.Purchase,
            Industry = stock.Industry,
            LastDiv = stock.LastDiv,
            MarketCap = stock.MarketCap,
            Comments = stock.Comments.Select(comment => comment.ToCommentDto()).ToList()
        };
    }

    public static Stock ToStockFromCreateDto(this CreateStockRequestDto createStockRequestDto)
    {
        return new Stock
        {
            Symbol = createStockRequestDto.Symbol,
            CompanyName = createStockRequestDto.CompanyName,
            Purchase = createStockRequestDto.Purchase,
            Industry = createStockRequestDto.Industry,
            LastDiv = createStockRequestDto.LastDiv,
            MarketCap = createStockRequestDto.MarketCap
        };
    }

    public static StockWithoutCommentsDto ToStockWithoutCommentsDto(this Stock stock)
    {
        return new StockWithoutCommentsDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            Purchase = stock.Purchase,
            Industry = stock.Industry,
            LastDiv = stock.LastDiv,
            MarketCap = stock.MarketCap,
        };
    }
}
