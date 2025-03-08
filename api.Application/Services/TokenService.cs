using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Application.Interfaces;
using api.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace api.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config)
    {
        _config = config;

        var signingKey = _config["JWT:SigningKey"] ?? throw new ArgumentNullException("JWT:SigningKey");

        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
    }


    public string CreateToken(AppUser appUser)
    {
        ArgumentNullException.ThrowIfNull(appUser.Email, nameof(appUser.Email));
        ArgumentNullException.ThrowIfNull(appUser.UserName, nameof(appUser.UserName));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, appUser.Email),
            new(JwtRegisteredClaimNames.GivenName, appUser.UserName)
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}