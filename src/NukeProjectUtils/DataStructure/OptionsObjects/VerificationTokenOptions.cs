namespace NukeProjectUtils.DataStructure.OptionsObjects;

public record VerificationTokenOptions
{
    public int OneTimePasswordTokenExpireTime { get; init; } = 15;
    public int AlphanumericTokenExpireTime { get; init; } = 15;
    public int UrlSafeTokenExpireTime { get; init; } = 60;
    public int RefreshJwtTokenExpireTime { get; init; } = 7;
}
