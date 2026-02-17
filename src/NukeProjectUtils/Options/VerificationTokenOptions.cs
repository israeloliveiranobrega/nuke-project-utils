namespace NukeProjectUtils.Options;

public record VerificationTokenOptions
{
    public int OneTimePasswordTokenExpireTime { get; init; } = 15;
    public int AlphanumericTokenExpireTime { get; init; } = 15;
    public int UrlSafeTokenExpireTime { get; init; } = 60;
    public int RefreshJwtTokenExpireDays { get; init; } = 7;
}
