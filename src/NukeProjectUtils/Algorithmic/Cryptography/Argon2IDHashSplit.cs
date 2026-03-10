namespace NukeProjectUtils.Algorithmic.Cryptography;

internal class Argon2IDHashSplit
{
    public int MemoryCost { get; init; }
    public int TimeCost { get; init; }
    public int Parallelism { get; init; }
    public string Salt { get; init; } = string.Empty;
    public string Hash { get; init; } = string.Empty;
}
