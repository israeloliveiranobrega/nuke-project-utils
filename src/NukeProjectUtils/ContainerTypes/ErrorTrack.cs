using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace NukeProjectUtils.ContainerTypes;

public record ErrorTrack 
{
    public ImmutableList<string> ContextPath { get; init; } = [];

    public string Error { get; init; }
    public string Local { get; init; }

    private ErrorTrack(string error, string local, ImmutableList<string> path)
    {
        Error = error;
        Local = local;
        ContextPath = path;
    }
    public static ErrorTrack Create(string error, [CallerFilePath] string filePath = "",[CallerMemberName] string methodName = "")
    {
        string? objectName = Path.GetFileNameWithoutExtension(filePath);
        return new(error, methodName, [objectName]);
    }
    public ErrorTrack AddContext([CallerFilePath] string filePath = "")
    {
        string? context = Path.GetFileNameWithoutExtension(filePath);
        return this with { ContextPath = ContextPath.Insert(0, context) };
    }
    public string ShowErrorTrace() => $"{string.Join(".", ContextPath)}: {Error} in {Local}";
}
