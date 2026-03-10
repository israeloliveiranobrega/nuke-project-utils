/*
Meus olhos a procuram em todos os lugares em que eu vou,
meu coração acelera quando você chega, 
a realidade ao seu lado é mais encantadora que qualquer fabula já escrita 
*/

using System.Runtime.CompilerServices;

namespace NukeProjectUtils.Patterns.ResultPattern;

/// <summary>
/// Represents an immutable diagnostic trace for capturing and bubbling up execution failures across component boundaries.
/// </summary>
public record ErrorTrack
{
    /// <summary>
    /// The hierarchical call stack of components that propagated the failure.
    /// </summary>
    public IEnumerable<string> ContextPath { get; init; } = [];

    /// <summary>
    /// The specific failure description or fault identifier.
    /// </summary>
    public string Error { get; init; }

    /// <summary>
    /// The origin member where the failure initially occurred.
    /// </summary>
    public string Location { get; init; }

    private ErrorTrack(string error, string location, IEnumerable<string> path)
    {
        Error = error;
        Location = location;
        ContextPath = path;
    }

    /// <summary>
    /// Instantiates a root trace entry at the point of failure.
    /// </summary>
    /// <param name="error">The specific failure description.</param>
    /// <param name="filePath">The absolute path of the source file containing the caller.</param>
    /// <param name="methodName">The method or property name of the caller.</param>
    /// <returns>A new diagnostic trace anchored to the calling context.</returns>
    public static ErrorTrack Create(string error, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "")
    {
        string? objectName = Path.GetFileNameWithoutExtension(filePath);
        return new(error, methodName, [objectName]);
    }

    /// <summary>
    /// Wraps the existing trace with an additional layer of architectural context during propagation.
    /// </summary>
    /// <param name="filePath">The absolute path of the source file containing the caller.</param>
    /// <returns>A new diagnostic trace including the prepended caller context.</returns>
    public ErrorTrack AddContext([CallerFilePath] string filePath = "")
    {
        string? newContext = Path.GetFileNameWithoutExtension(filePath);

        List<string> path = [newContext];
        path.AddRange(ContextPath);

        return this with { ContextPath = path };
    }

    /// <summary>
    /// Formats the full diagnostic chain for logging or auditing purposes.
    /// </summary>
    /// <returns>The formatted error trace string.</returns>
    public string ShowErrorTrace()
    {
        return $"Error: {Error} in {string.Join(".", ContextPath)}.{Location}";
    }
}