using NukeProjectUtils.Exceptions.Base;

namespace NukeProjectUtils.Exceptions;

/// <summary>
/// Thrown when an email address does not meet the required format specifications (e.g. missing '@' or domain).
/// Typically thrown during the instantiation or validation phase of the Email Value Object.
/// </summary>
public class InvalidEmailFormatException : DomainException
{
    public InvalidEmailFormatException()
        : base("The provided email address format is invalid.") { }
}

/// <summary>
/// Thrown when the user provides a code that is syntactically valid but does not match the stored internal value.
/// </summary>
public class EmailVerificationCodeMismatchException : DomainException
{
    public EmailVerificationCodeMismatchException()
        : base("The provided verification code does not match.") { }
}

/// <summary>
/// Thrown when the email verification attempt occurs after the token's Time-To-Live (TTL) has elapsed.
/// </summary>
public class EmailVerificationCodeExpiredException : DomainException
{
    public EmailVerificationCodeExpiredException()
        : base("The email verification code has expired.") { }
}

/// <summary>
/// Thrown when a verification workflow is attempted (e.g., calling VerifyEmail), but no token generation
/// workflow was previously initiated for this instance.
/// </summary>
public class EmailVerificationCodeNotFoundException : DomainException
{
    public EmailVerificationCodeNotFoundException()
        : base("No active verification code was found for this email.") { }
}

/// <summary>
/// Thrown when a request to generate a NEW token is made, but a valid (non-expired) token already exists.
/// This acts as a throttling mechanism to prevent spamming the generation logic.
/// </summary>
public class EmailVerificationCodeAlreadyExistsException : DomainException
{
    // Fix: The previous message said "No code found", which contradicted the class name. 
    // It has been updated to reflect that a code actually exists.
    public EmailVerificationCodeAlreadyExistsException()
        : base("An active verification code already exists. Please wait for the current code to expire.") { }
}