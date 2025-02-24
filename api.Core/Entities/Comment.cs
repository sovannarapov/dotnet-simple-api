using System.ComponentModel.DataAnnotations.Schema;

namespace api.Core.Entities;

[Table("Comments")]
public class Comment : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public int? StockId { get; set; }
    public Stock? Stock { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
