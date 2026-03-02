using System.Security.Claims;

namespace NukeProjectUtils.Services.JasonWebServices;

public interface IJasonWebTokenProvider
{
    Task<string> GerateAccessToken(List<Claim> claims);
}
