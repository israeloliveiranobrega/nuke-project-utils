using System.Security.Claims;

namespace NukeProjectUtils.Services.JasonWebServices;

public interface IJsonWebTokenProvider
{
    Task<string> GenerateAccessToken(List<Claim> claims);
}
