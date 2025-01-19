using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Stocks")]
    public class Stock
    {
        public int Id { get; set; }
        
        [StringLength(120)]
        public string Symbol { get; set; } = "";
        
        [StringLength(120)]
        public string CompanyName { get; set; } = "";
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv { get; set; }
        
        [StringLength(120)]
        public string Industry { get; set; } = "";
        
        public long MarketCap { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
