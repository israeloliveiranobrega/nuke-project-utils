/*
Mentes inteligentes discutem ideias, 
mentes medianas discutem sobre eventos,
mentes fracas discutem sobre pessoas  
*/

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NukeProjectUtils.DataStructure.OptionsObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NukeProjectUtils.Services.JasonWebServices;

/// <summary>
/// Issues JSON Web Tokens based on configured cryptographic parameters to establish authorization contexts.
/// </summary>
public sealed class JsonWebTokenProvider(IOptions<JsonWebTokenOptions> jwtOptions) : IJsonWebTokenProvider
{
    private readonly JsonWebTokenOptions _jwtOptions = jwtOptions.Value;

    /// <summary>
    /// Produces a signed authorization token embedding the specified identity attributes.
    /// </summary>
    /// <param name="claims">The identity attributes defining the authorization scope.</param>
    /// <returns>A signed string representation of the JWT.</returns>
    public Task<string> GenerateAccessToken(List<Claim> claims)
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