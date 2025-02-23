using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Comment;

public class UpdateCommentRequestDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be 5 characters")]
    [MaxLength(255, ErrorMessage = "Title must not over 255 characters")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MinLength(5, ErrorMessage = "Content must be 5 characters")]
    [MaxLength(255, ErrorMessage = "Content must not over 255 characters")]
    public string Content { get; set; } = string.Empty;
}
