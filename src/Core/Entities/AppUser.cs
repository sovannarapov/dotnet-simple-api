using Microsoft.AspNetCore.Identity;

namespace api.Core.Entities;

public class AppUser : IdentityUser
{
    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
}
