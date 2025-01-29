using api.Core.Entities;

namespace api.Core.Interfaces;


public interface ITokenService
{
    string CreateToken(AppUser appUser);
}
