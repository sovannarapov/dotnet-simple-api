using Core.Entities;

namespace Application.Interfaces;


public interface ITokenService
{
    string CreateToken(AppUser appUser);
}
