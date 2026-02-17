using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NukeProjectUtils.ContainerTypes.ErrorEnums;

/// <summary>
/// Defines the specific format and generation strategy for the verification token.
/// </summary>
public enum ValidationTokenType
{
    /// <summary>
    /// Represents a numeric code, typically used for short-term authentication (OTP).
    /// </summary>
    OneTimePassword = 0,

    /// <summary>
    /// Represents a combination of letters and numbers, excluding ambiguous characters.
    /// </summary>
    AlphanumericCode = 1,

    /// <summary>
    /// Represents a token suitable for inclusion in URLs (safe for query strings).
    /// </summary>
    UrlSafe = 2,

    /// <summary>
    /// Represents a long-lived token used to refresh access credentials.
    /// </summary>
    RefreshToken = 3
}

public enum VerificationFailure
{
    TokenRequired = 0,
    Expired = 1,
    InvalidExpirationDate = 2,  
}
public enum VerificationTokenError
{
    [Description("The verification code was not provided.")]
    InvalidOption = 0,

    [Description("The verification code was not provided.")]
    TokenIsNull = 1,

    [Description("The verification code is invalid or does not match.")]
    TokenNotMatch = 2,

    [Description("The verification code has expired.")]
    TokenExpired = 3,
}