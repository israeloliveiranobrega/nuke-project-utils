namespace NukeProjectUtils.Patterns.ResultPattern;

public readonly record struct Result<TValue> 
{
    public bool IsFailure { get; init; }

    private readonly TValue? _value { get; init; }
    private readonly ErrorTrack? _errorTrack { get; init; }

    public TValue Value => !IsFailure? _value! : throw new InvalidOperationException();
    public ErrorTrack ErrorTrack => IsFailure? _errorTrack! : throw new InvalidOperationException();

    private Result(bool isFailure, TValue? value, ErrorTrack? errorTrack)
    {
        IsFailure = isFailure;
        _value = value;
        _errorTrack = errorTrack;
    }

    public static Result<TValue> Success(TValue value) => new (false, value, null);

    public static Result<TValue> Failure(ErrorTrack errorTrack) => new (true, default, errorTrack);
}
