using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NukeProjectUtils.DataStructure.OptionsObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NukeProjectUtils.Services.JasonWebServices;

public sealed class JasonWebTokenProvider(IOptions<JasonWebTokenOptions> jwtOptions) : IJasonWebTokenProvider
{
    private readonly JasonWebTokenOptions _jwtOptions = jwtOptions.Value;

    public Task<string> GerateAccessToken(List<Claim> claims)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)), SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireInMinutes),
            signingCredentials);

        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(jwtToken));
    }
}
