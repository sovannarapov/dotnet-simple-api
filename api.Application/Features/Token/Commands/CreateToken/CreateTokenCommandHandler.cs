using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, string>
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;

    public CreateTokenCommandHandler(IConfiguration config)
    {
        _config = config;

        var signingKey = _config["JWT:SigningKey"] ?? throw new ArgumentNullException("JWT:SigningKey");

        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
    }

    public Task<string> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.AppUser.Email, nameof(request.AppUser.Email));
        ArgumentNullException.ThrowIfNull(request.AppUser.UserName, nameof(request.AppUser.UserName));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, request.AppUser.Email),
            new(JwtRegisteredClaimNames.GivenName, request.AppUser.UserName)
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

        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}