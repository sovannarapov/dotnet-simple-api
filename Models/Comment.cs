using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        
        [StringLength(120)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public int? StockId { get; set; }
        public Stock? Stock { get; set; }
    }
}
