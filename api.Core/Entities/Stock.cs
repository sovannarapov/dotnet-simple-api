using System.ComponentModel.DataAnnotations.Schema;

namespace api.Core.Entities;

[Table("Stocks")]
public class Stock : BaseEntity
{
    public string Symbol { get; set; } = "";
    public string CompanyName { get; set; } = "";

    [Column(TypeName = "decimal(18,2)")] public decimal Purchase { get; set; }

    [Column(TypeName = "decimal(18,2)")] public decimal LastDiv { get; set; }

    public string Industry { get; set; } = "";
    public long MarketCap { get; set; }
    public List<Comment> Comments { get; set; } = new();
    public List<Portfolio> Portfolios { get; set; } = new();
}