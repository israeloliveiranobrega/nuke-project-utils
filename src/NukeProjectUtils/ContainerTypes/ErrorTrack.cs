using System.Runtime.CompilerServices;

namespace NukeProjectUtils.ContainerTypes;

public record ErrorTrack 
{
    private IReadOnlyList<string> ContextPath { get; init; } = [];
    private Enum Error { get; init; }
    private string Local { get ; init; }    
    private ErrorTrack(string obj, Enum error, string local, List<string>? path = null)
    {
        Error = error;
        Local = local;
        ContextPath = path ?? [obj];
    }
    public static ErrorTrack Create(string objectName, Enum error, [CallerMemberName] string methodName = "")
    {
        return new (objectName, error, methodName);
    }
    public ErrorTrack AddContext(string context)
    {
        var newPath = new List<string> { context };
        newPath.AddRange(ContextPath);
        return this with { ContextPath = newPath };
    }
    public override string ToString() =>
        $"{string.Join(".", ContextPath)}: {Error} in {Local}";
}
