using System.ComponentModel.DataAnnotations;

namespace api.Application.Dtos.Account;

public class LoginRequest
{
    [Required] [EmailAddress] public string Email { get; set; }

    [Required] public string Password { get; set; }
}