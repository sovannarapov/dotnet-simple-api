using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Account;

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}

