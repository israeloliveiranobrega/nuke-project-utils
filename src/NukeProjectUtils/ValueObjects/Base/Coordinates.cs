namespace NukeProjectUtils.ValueObjects.Base;

public record Coordinates
{
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
}