using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NukeProjectUtils.ContainerTypes;

public enum VerificationFailure
{
    [Description("The verification code was not provided.")]
    CodeIsNull = 1,

    [Description("The verification code is invalid or does not match.")]
    CodeNotMatch = 2,

    [Description("The verification code has expired.")]
    CodeExpired = 3,
}

public class VerificationResult<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public VerificationFailure? FailureType { get; }

    private VerificationResult(bool isSuccess, T? value, VerificationFailure? failureType)
    {
        IsSuccess = isSuccess;
        Value = value;
        FailureType = failureType;
    }

    public static VerificationResult<T> Success(T value) => new(true, value, null);
    public static VerificationResult<T> Failure(VerificationFailure failureType) => new(false, default, failureType);
}
