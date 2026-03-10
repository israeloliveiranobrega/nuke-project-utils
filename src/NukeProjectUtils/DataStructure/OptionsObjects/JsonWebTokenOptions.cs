namespace NukeProjectUtils.DataStructure.OptionsObjects;

public record JsonWebTokenOptions
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public int ExpireInMinutes { get; init; } 
}
