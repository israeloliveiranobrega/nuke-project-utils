namespace NukeProjectUtils.ContainerTypes;

/// <summary>
/// Represents a functional result wrapper that encapsulates the outcome of an operation.
/// This pattern is used to handle success or failure states explicitly without relying on exceptions for control flow.
/// </summary>
/// <typeparam name="TObject">The type of the value returned upon success.</typeparam>
/// <typeparam name="TError">The specific enum type representing potential failure reasons.</typeparam>
public class Result<TObject, TError> 
{
    /// <summary>
    /// Gets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the result value if the operation was successful.
    /// Returns the default value (or null) if the operation failed.
    /// </summary>
    public TObject? Value { get; }

    /// <summary>
    /// Gets the error classification if the operation failed.
    /// Returns null if the operation was successful.
    /// </summary>
    public TError? Failure { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TObject, TError}"/> class.
    /// Private constructor to enforce the use of static factory methods.
    /// </summary>
    /// <param name="isSuccess">Indicates the success state.</param>
    /// <param name="value">The success value, if applicable.</param>
    /// <param name="failure">The error type, if applicable.</param>
    private Result(bool isSuccess, TObject? value, TError? failure)
    {
        IsSuccess = isSuccess;
        Value = value;
        Failure = failure;
    }

    /// <summary>
    /// Creates a successful result containing the computed value.
    /// </summary>
    /// <param name="value">The value returned by the operation.</param>
    /// <returns>A <see cref="Result{TObject, TError}"/> indicating success.</returns>
    public static Result<TObject, TError> Success(TObject value) => new(true, value, default);

    /// <summary>
    /// Creates a failed result containing the specific error reason.
    /// </summary>
    /// <param name="failureType">The enum value describing why the operation failed.</param>
    /// <returns>A <see cref="Result{TObject, TError}"/> indicating failure.</returns>
    public static Result<TObject, TError> Fail(TError failureType) => new(false, default, failureType);
}