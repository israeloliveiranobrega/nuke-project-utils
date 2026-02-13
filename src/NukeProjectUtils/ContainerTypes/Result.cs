using System.Net;

namespace NukeProjectUtils.ContainerTypes;
public enum FailureType
{
    BadRequest,

    Fraud,
    LoginIcorrect,

    RevokedSession,
    ExpiredToken,
}
public class Result<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public FailureType? FailureType { get; }

    private Result(bool isSuccess, T? value, FailureType? failureType)
    {
        IsSuccess = isSuccess;
        Value = value;
        FailureType = failureType;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(FailureType failureType) => new (false, default, failureType);
}


