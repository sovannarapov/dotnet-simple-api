using System.ComponentModel.DataAnnotations;

namespace api.Application.Dtos.Account;

public class RegisterAccountRequestDto
{
    [Required]
    public string? Username { get; set; }
    
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public string? Password { get; set; }
}
