/*
Eu penso em você cada segundo de cada dia, 
não existe um único momento ao dia em que você não esteja em minha cabeça, 
a ideia de ficar um único dia sequer sem você atormenta minha alma
*/

namespace NukeProjectUtils.Patterns.ResultPattern;

/// <summary>
/// Encapsulates the outcome of an operation, distinguishing between successful execution and failure states without relying on exceptions for control flow.
/// </summary>
/// <typeparam name="TValue">The type of the encapsulated payload in a successful scenario.</typeparam>
public readonly record struct Result<TValue>
{
    /// <summary>
    /// Indicates whether the operation encountered a domain or logic error.
    /// </summary>
    public bool IsFailure { get; init; }

    private readonly TValue? _value { get; init; }
    private readonly ErrorTrack? _errorTrack { get; init; }

    /// <summary>
    /// The payload of a successful operation.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to access the payload of a failed operation.</exception>
    public TValue Value => !IsFailure ? _value! : throw new InvalidOperationException();

    /// <summary>
    /// The error details explaining why the operation could not be completed.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to access the error track of a successful operation.</exception>
    public ErrorTrack ErrorTrack => IsFailure ? _errorTrack! : throw new InvalidOperationException();

    private Result(bool isFailure, TValue? value, ErrorTrack? errorTrack)
    {
        IsFailure = isFailure;
        _value = value;
        _errorTrack = errorTrack;
    }

    /// <summary>
    /// Creates an outcome representing a completed operation with its associated payload.
    /// </summary>
    /// <param name="value">The payload yielded by the operation.</param>
    /// <returns>A successful outcome.</returns>
    public static Result<TValue> Success(TValue value) => new(false, value, null);

    /// <summary>
    /// Creates an outcome representing an interrupted or invalid operation.
    /// </summary>
    /// <param name="errorTrack">The detailed trace or reason for the failure.</param>
    /// <returns>A failed outcome.</returns>
    public static Result<TValue> Failure(ErrorTrack errorTrack) => new(true, default, errorTrack);
}