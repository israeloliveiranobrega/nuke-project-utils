namespace NukeProjectUtils.Services.TokenServices;

public enum ValidationTokenType
{
    OneTimePassword = 1,
    AlphanumericCode = 2,
    UrlSafe = 3,
    RefreshToken = 4
}
