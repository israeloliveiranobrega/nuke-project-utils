namespace NukeProjectUtils.DataStructure.OptionsObjects;

public record Argon2IDOptions
{
    public string Pepper { get; init; } = string.Empty;
    public int SaltSize { get; init; } = 3;
    public int HashSize { get; init; } = 9;
    public int Parallelism { get; init; } = 2;
    public int TimeCost { get; init; } = 4;
    public int MemoryCost { get; init; } = 256000;
}