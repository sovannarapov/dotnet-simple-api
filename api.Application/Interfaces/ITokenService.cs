using api.Core.Entities;

namespace api.Application.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser appUser);
}